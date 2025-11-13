using ImageMagick;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NewFrameworkSpriteExporter
{
    static class Program
    {
        public static int SpriteScale = 4; // multiplies the X and Y components of all actors' Position values
        public static Dictionary<string, Dictionary<string, MagickImage>> Cells = [];
        public static Dictionary<string, Sprite> LoadedSprites = [];
        public static Dictionary<string, MagickImage> LoadedTextures = [];
        public static Dictionary<string, Dictionary<string, Dictionary<string, uint>>> LoadedAtlases = [];

        /// <summary>
        /// converts FrameInformation in an XML to a nested dictionary. Cell names are keys of the outer dictionary, Cell x, y, w, h, ax, ay, aw, and ah are keys of the inner dictionary, and the string values are converted to integers.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, uint>> XmlToDict(XElement root)
        {
            Dictionary<string, Dictionary<string, uint>> dict = [];
            foreach (var elem in root.Descendants("Cell"))
            {
                dict[elem.Attribute("name").Value] = elem.Attributes().Where(b => uint.TryParse(b.Value, out uint num)).ToDictionary(
                    b => b.Name.LocalName, b => uint.Parse(b.Value)
                );
            }

            return dict;
        }

        /// <summary>
        /// Calculates the bounds of a sprite at the given time.
        /// </summary>
        /// <param name="spriteName"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static double[] GetSpriteSizeAtTime(string spriteName, double time)
        {
            Sprite sprite = LoadedSprites[spriteName];

            // loop sprites that have a shorter stagelength than the main sprite
            double myTime = time % sprite._stageLength;

            foreach (Actor actor in sprite.actors)
            {
                double[] bounds = actor.type == 2 ? GetSpriteSizeAtTime(actor.sprite, time) :
                    [-Cells[sprite._spriteInfo[actor.sprite]][actor.sprite].Width / 2d,
                    Cells[sprite._spriteInfo[actor.sprite]][actor.sprite].Width / 2d,
                    -Cells[sprite._spriteInfo[actor.sprite]][actor.sprite].Height / 2d,
                    Cells[sprite._spriteInfo[actor.sprite]][actor.sprite].Height / 2d];
                
                // if this actor has a timeline, use it to setup, otherwise use the actor's default setup
                Actor actorAtTime = (sprite._timelines.TryGetValue(actor.uid, out StageItem[] timeline) && timeline != null) ? Actor.GetActorAtTime(timeline, myTime) : actor;

                if (actorAtTime.Scale[0] != 0.0f && actorAtTime.Scale[1] != 0.0f && actorAtTime.Shown)
                {
                    double
                        sin = Math.Sin(actorAtTime.Angle * Math.PI / 180d),
                        cos = Math.Cos(actorAtTime.Angle * Math.PI / 180d),
                        minX = bounds[0] * Math.Abs(actorAtTime.Scale[0]),
                        maxX = bounds[1] * Math.Abs(actorAtTime.Scale[0]),
                        minY = bounds[2] * Math.Abs(actorAtTime.Scale[1]),
                        maxY = bounds[3] * Math.Abs(actorAtTime.Scale[1]),
                        halfWidth = (maxX - minX) / 2d,
                        halfHeight = (maxY - minY) / 2d,
                        axisX = -(minX + maxX) / 2d,
                        axisY = -(minY + maxY) / 2d;

                    if      (actorAtTime.Alignment[0] == 1)   axisX -= halfWidth; // center is on left
                    else if (actorAtTime.Alignment[0] == 2)   axisX += halfWidth; // center is on right
                    if      (actorAtTime.Alignment[1] == 3)   axisY -= halfHeight; // center is on top
                    else if (actorAtTime.Alignment[1] == 4)   axisY += halfHeight; // center is on bottom

                    if (actorAtTime.Flip == 1 || actorAtTime.Flip == 3) axisX *= -1d;
                    if (actorAtTime.Flip == 2 || actorAtTime.Flip == 3) axisY *= -1d;

                    double[,] localCorners = {
                        { -halfWidth, -halfHeight }, // top left
                        { halfWidth, -halfHeight }, // top right
                        { halfWidth, halfHeight }, // bottom right
                        { -halfWidth, halfHeight } // bottom left
                    };

                    // calculate rotation of each corner around 0,0 then adjust position based on the axis
                    for (int i = 0; i < 4; i++)
                    {
                        // rotate corners around 0,0, then move corners to where they would be relative to the axis of rotation, then translate
                        double cornerX = ((localCorners[i, 0] * cos) - (localCorners[i, 1] * sin)) - ((axisX * cos) - (axisY * sin)) + (actorAtTime.Position[0] * SpriteScale);
                        double cornerY = ((localCorners[i, 0] * sin) + (localCorners[i, 1] * cos)) - ((axisX * sin) + (axisY * cos)) + (actorAtTime.Position[1] * SpriteScale);
                        sprite.minX = Math.Min(sprite.minX, cornerX);
                        sprite.maxX = Math.Max(sprite.maxX, cornerX);
                        sprite.minY = Math.Min(sprite.minY, cornerY);
                        sprite.maxY = Math.Max(sprite.maxY, cornerY);
                    }
                }
            }
                    
            return [sprite.minX, sprite.maxX, sprite.minY, sprite.maxY];
        }

        /// <summary>
        /// Generates an image of a sprite at the given time.
        /// </summary>
        /// <param name="spriteName"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private static MagickImage GetSpriteAtTime(string spriteName, double time)
        {
            Sprite sprite = LoadedSprites[spriteName];

            MagickImage output = new(MagickColors.Transparent, (uint)Math.Round(sprite.maxX - sprite.minX), (uint)Math.Round(sprite.maxY - sprite.minY));

            // loop sprites that have a shorter stagelength than the main sprite
            double myTime = time % sprite._stageLength;

            foreach (Actor actor in sprite.actors)
            {
                // if this actor has a timeline, use it to setup, otherwise use the actor's default setup
                Actor actorAtTime = (sprite._timelines.TryGetValue(actor.uid, out StageItem[] timeline) && timeline != null) ? Actor.GetActorAtTime(timeline, myTime) : actor;

                // don't bother drawing invisible actors
                if (actorAtTime.Scale[0] != 0f && actorAtTime.Scale[1] != 0f && actorAtTime.Shown)
                {
                    using MagickImage output2 = new(actor.type == 2 ? GetSpriteAtTime(actor.sprite, time) : new(Cells[sprite._spriteInfo[actor.sprite]][actor.sprite]));

                    // color multiplication
                    output2.Evaluate(Channels.Red, EvaluateOperator.Multiply, (actorAtTime.Colour & 0xFF) / 255d);
                    output2.Evaluate(Channels.Green, EvaluateOperator.Multiply, ((actorAtTime.Colour >> 8) & 0xFF) / 255d);
                    output2.Evaluate(Channels.Blue, EvaluateOperator.Multiply, ((actorAtTime.Colour >> 16) & 0xFF) / 255d);

                    // the alpha variable and alpha channel seem to be averaged rather than multiplied
                    output2.Evaluate(Channels.Alpha, EvaluateOperator.Multiply, (actorAtTime.Alpha + ((actorAtTime.Colour >> 24) & 0xFF) / 255d) / 2d);

                    // NewFramework's sprite renderer seems to scale actors to integer coordinates, despite allowing subpixel translation
                    // you can see this by zooming out on the bloon inflation factory in BMC - the bar graph in the bottom-right corner of the sprite becomes choppy since it only uses integer scaling, but the cells that use interpolated translation move smoothly
                    output2.Resize(new MagickGeometry()
                    {
                        Width = (uint)Math.Round(output2.Width * Math.Abs(actorAtTime.Scale[0])),
                        Height = (uint)Math.Round(output2.Height * Math.Abs(actorAtTime.Scale[1])),
                        IgnoreAspectRatio = true
                    });

                    double posX = 0, posY = 0;

                    // imagemagick does not allow you to scale an image by a negative value, so this has to flip it if it's negative before applying the rotation/alignment
                    if (actorAtTime.Scale[0] < 0) output2.Flop();
                    if (actorAtTime.Scale[1] < 0) output2.Flip();

                    if (actor.type == 2)
                    {
                        // there's probably a better way of doing this. but i hate geometry
                        double[] bounds = GetSpriteSizeAtTime(actor.sprite, myTime);
                        posX = (actorAtTime.Position[0] * SpriteScale) - ((sprite.minX + sprite.maxX) / 2d) + ((bounds[0] + bounds[1]) / 2d * actorAtTime.Scale[0] * (actorAtTime.Flip == 1 || actorAtTime.Flip == 3 ? -1 : 1));
                        posY = (actorAtTime.Position[1] * SpriteScale) - ((sprite.minY + sprite.maxY) / 2d) + ((bounds[2] + bounds[3]) / 2d * actorAtTime.Scale[1] * (actorAtTime.Flip == 2 || actorAtTime.Flip == 3 ? -1 : 1));
                    }
                    else
                    {
                        posX = (actorAtTime.Position[0] * SpriteScale) - ((sprite.minX + sprite.maxX) / 2d);
                        posY = (actorAtTime.Position[1] * SpriteScale) - ((sprite.minY + sprite.maxY) / 2d);

                        // to simulate aligning an image to a non-center axis, this doubles the width and/or height of the image anchored to an edge or corner depending on the alignment values
                        switch (actorAtTime.Alignment)
                        {
                            case [1, 0]: output2.Extent(output2.Width * 2, output2.Height, Gravity.East); break;
                            case [2, 0]: output2.Extent(output2.Width * 2, output2.Height, Gravity.West); break;
                            case [0, 3]: output2.Extent(output2.Width, output2.Height * 2, Gravity.South); break;
                            case [0, 4]: output2.Extent(output2.Width, output2.Height * 2, Gravity.North); break;

                            case [1, 3]: output2.Extent(output2.Width * 2, output2.Height * 2, Gravity.Southeast); break;
                            case [1, 4]: output2.Extent(output2.Width * 2, output2.Height * 2, Gravity.Northeast); break;
                            case [2, 3]: output2.Extent(output2.Width * 2, output2.Height * 2, Gravity.Southwest); break;
                            case [2, 4]: output2.Extent(output2.Width * 2, output2.Height * 2, Gravity.Northwest); break;
                        }
                    }

                    if (actorAtTime.Flip == 1 || actorAtTime.Flip == 3) output2.Flop();
                    if (actorAtTime.Flip == 2 || actorAtTime.Flip == 3) output2.Flip();

                    if (actorAtTime.Angle != 0d) output2.Rotate(actorAtTime.Angle);

                    if (Properties.Settings.Default.AllowNonIntegerActorPositioning && (posX % 1 != 0 || posY % 1 != 0))
                    {
                        // use ImageMagick's distort feature to translate by non-integer amounts
                        using MagickImage distortedSprite = new(MagickColors.Transparent, output.Width, output.Height);
                        distortedSprite.Composite(output2, Gravity.Center, (int)Math.Round(posX), (int)Math.Round(posY), CompositeOperator.SrcOver);
                        distortedSprite.VirtualPixelMethod = VirtualPixelMethod.Transparent;
                        distortedSprite.Interpolate = PixelInterpolateMethod.Bilinear;
                        /*DrawableAffine af = new();
                        af.TransformRotation(actorAtTime.Angle);
                        af.TransformOrigin(posX - offsX, posY - offsY);
                        new Drawables()
                            .Gravity(Gravity.Northwest)
                            .Composite((output.Width - output2.Width) / 2d, (output.Height - output2.Height) / 2d, CompositeOperator.SrcOver, output2)
                            .Affine(af.ScaleX, af.ScaleY, af.ShearX, af.ShearY, af.TranslateX, af.TranslateY)
                            .Draw(output);*/
                        distortedSprite.Distort(DistortMethod.ScaleRotateTranslate, [
                            0d, 0d, 1d, 0d, posX - (int)Math.Round(posX), posY - (int)Math.Round(posY)
                        ]);
                        output.Composite(distortedSprite, Gravity.Center, CompositeOperator.SrcOver);
                    }
                    else
                    {
                        output.Composite(output2, Gravity.Center, (int)Math.Round(posX), (int)Math.Round(posY), CompositeOperator.SrcOver);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Loads every texture atlas required by this sprite and all child sprites, and splits them into cells based on their XML data.
        /// </summary>
        /// <param name="spriteName"></param>
        public static void LoadResources(string spriteName)
        {
            string filePath = Path.Combine(Properties.Settings.Default.SpriteFolderPath, spriteName);
            string text = File.ReadAllText(filePath);
            Sprite sprite = JsonSerializer.Deserialize<Sprite>(text);
            LoadedSprites[spriteName] = sprite;

            foreach (var actor in sprite.actors)
            {
                // recursive call to load sub-sprite textures and atlases
                if (actor.type == 2)
                    LoadResources(actor.sprite);
                else
                {
                    string textureName = sprite._spriteInfo[actor.sprite];

                    // load new texture and atlas if not loaded already
                    if (!LoadedTextures.TryGetValue(textureName, out MagickImage texture))
                    {
                        texture = new(Path.Combine(Properties.Settings.Default.TextureFolderPath, textureName + ".png"));
                        LoadedTextures[textureName] = texture;

                        XElement root = XDocument.Load(Path.Combine(Properties.Settings.Default.TextureFolderPath, textureName + ".xml")).Root;
                        LoadedAtlases[textureName] = XmlToDict(root);

                        Cells[textureName] = [];
                    }

                    // create this cell if not done already
                    if (!Cells[textureName].ContainsKey(actor.sprite))
                    {
                        var dimensions = LoadedAtlases[textureName][actor.sprite];
                        MagickImage cell = new(texture);

                        // x, y, w, h are the dimensions of the cell to cut out
                        cell.Crop(new MagickGeometry((int)dimensions["x"], (int)dimensions["y"], dimensions["w"], dimensions["h"]));

                        // ax, ay, aw, ah are the dimensions of the actor containing the cell
                        if (!Properties.Settings.Default.IgnoreActorDimensions)
                            cell.Extent(new MagickGeometry(-(int)dimensions["ax"], -(int)dimensions["ay"], dimensions["aw"], dimensions["ah"]), Gravity.Northwest, MagickColors.Transparent);

                        Cells[textureName][actor.sprite] = cell;
                    }

                }
            }
        }

        /// <summary>
        /// Renders and saves one frame of an animated sprite.
        /// </summary>
        /// <param name="spriteName"></param>
        /// <param name="frame"></param>
        /// <param name="toolStrip"></param>
        /// <param name="progBar"></param>
        /// <param name="progBarLabel"></param>
        /// <param name="spritePreview"></param>
        public async static void ProcessSingleFrame(string spriteName, int frame, ToolStrip toolStrip, ToolStripProgressBar progBar, ToolStripStatusLabel progBarLabel, PictureBox spritePreview)
        {
            SpriteScale = Properties.Settings.Default.TextureFolderPath switch
            {
                "Low" => 1,
                "High" => 2,
                "Tablet" => 2,
                "Ultra" => 4,
                _ => 4
            };

            progBar.Value = 0;
            progBar.Maximum = 4;

            var progress = new Progress<int>(value =>
            {
                // This lambda will be executed on the UI thread
                progBar.Value = value;
                toolStrip.Update();
            });

            DateTime startParallel = DateTime.Now;

            await Task.Run(() =>
            {
                // load all the necessary assets
                progBarLabel.Text = $"Loading resources";
                LoadResources(Properties.Settings.Default.SpriteName);

                // get bounds of this frame
                progBarLabel.Text = $"Calculating sprite bounds of frame {frame+1}";
                ((IProgress<int>)progress).Report(1);
                double[] rectAtTime = GetSpriteSizeAtTime(spriteName, frame);

                // generate image
                progBarLabel.Text = $"Processing frame {frame+1}";
                ((IProgress<int>)progress).Report(2);
                using MagickImage sprite = GetSpriteAtTime(spriteName, frame);
                sprite.Format = MagickFormat.Png;
                sprite.Write(Path.Combine(Properties.Settings.Default.OutputFolderPath, $"{spriteName}_{frame+1}.png"));

                // image preview
                using (var memStream = new MemoryStream())
                {
                    sprite.Write(memStream);
                    spritePreview.Image = new System.Drawing.Bitmap(memStream);
                }

                // flush all the loaded assets
                progBarLabel.Text = $"Cleaning up";
                ((IProgress<int>)progress).Report(3);
                LoadedSprites.Clear();
                LoadedTextures.Clear();
                LoadedAtlases.Clear();
                Cells.Clear();
            });

            TimeSpan parallelTime = DateTime.Now - startParallel;

            progBarLabel.Text = $"Finished in {parallelTime.Hours} hours {parallelTime.Minutes} minutes {parallelTime.Seconds} seconds {parallelTime.Nanoseconds}";
            progBar.Value = progBar.Maximum;
            toolStrip.Update();
        }

        /// <summary>
        /// Renders and saves all animations of the sprite in parallel.
        /// </summary>
        /// <param name="spriteName"></param>
        /// <param name="toolStrip"></param>
        /// <param name="progBar"></param>
        /// <param name="progBarLabel"></param>
        /// <param name="spritePreview"></param>
        public async static void ProcessAnimation(string spriteName, ToolStrip toolStrip, ToolStripProgressBar progBar, ToolStripStatusLabel progBarLabel, PictureBox spritePreview)
        {
            SpriteScale = Properties.Settings.Default.TextureFolderPath switch
            {
                "Low" => 1,
                "High" => 2,
                "Tablet" => 2,
                "Ultra" => 4,
                _ => 4
            };

            progBar.Value = 0;

            var progress = new Progress<int>(value =>
            {
                progBar.Value = value;
                toolStrip.Update();
            });

            var progressBounds = new Progress<int>(value =>
            {
                progBar.Value = value;
                progBarLabel.Text = $"Calculated sprite bounds of frame {progBar.Value}/{progBar.Maximum}";
                toolStrip.Update();
            });

            var progressRendering = new Progress<int>(value =>
            {
                progBar.Value += 1;
                progBarLabel.Text = $"Processed frame {value} - {progBar.Value}/{progBar.Maximum}";
                toolStrip.Update();
            });

            var progressMax = new Progress<int>(value =>
            {
                progBar.Maximum = value;
            });

            DateTime startParallel = DateTime.Now;

            await Task.Run(() =>
            {
                // load all the necessary assets
                progBarLabel.Text = $"Loading resources";
                LoadResources(Properties.Settings.Default.SpriteName);

                // determine the duration of the animation - this is either the stageLength of the sprite or the longest timeline the sprite uses
                double duration = Properties.Settings.Default.IgnoreStageLength
                    ? duration = LoadedSprites.Max(p => p.Value.GetLongestTimeline())
                    : duration = LoadedSprites[spriteName]._stageLength;

                int durationInFrames = Math.Max((int)Math.Ceiling(duration * 60), 1);
                ((IProgress<int>)progressMax).Report(durationInFrames);

                // get bounds of the entire animation
                for (int i = 0; i < durationInFrames; i++)
                {
                    double[] rectAtTime = GetSpriteSizeAtTime(spriteName, i / 60d);
                    ((IProgress<int>)progressBounds).Report(i+1);
                }

                // generate all the images in parallel
                ((IProgress<int>)progressBounds).Report(0);
                progBarLabel.Text = $"Processing frames";
                Parallel.For(0, durationInFrames, i =>
                {
                    using MagickImage image = GetSpriteAtTime(spriteName, i / 60d);
                    image.Format = MagickFormat.Png;

                    image.Write(Path.Combine(Properties.Settings.Default.OutputFolderPath, $"{spriteName}_{i+1}.png"));
                    ((IProgress<int>)progressRendering).Report(i+1);

                    // image preview
                    using (var memStream = new MemoryStream())
                    {
                        image.Write(memStream);
                        spritePreview.Image = new System.Drawing.Bitmap(memStream);
                    }
                });

                // flush all the loaded assets
                progBarLabel.Text = $"Cleaning up";
                LoadedSprites.Clear();
                LoadedTextures.Clear();
                LoadedAtlases.Clear();
                Cells.Clear();
            });

            TimeSpan parallelTime = DateTime.Now - startParallel;
            progBarLabel.Text = $"Finished in {parallelTime.Hours} hours {parallelTime.Minutes} minutes {parallelTime.Seconds} seconds {parallelTime.Nanoseconds} - rendered {progBar.Maximum} frames";
            progBar.Value = progBar.Maximum;
            toolStrip.Update();
        }

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NewFrameworkSpriteExporterForm());
        }
    }
}
