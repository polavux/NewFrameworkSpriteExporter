using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.IO;
using System.Diagnostics;

namespace NewFrameworkSpriteExporter
{
    public partial class NewFrameworkSpriteExporterForm : Form
    {
        public NewFrameworkSpriteExporterForm()
        {
            InitializeComponent();
            Text = $"NewFrameworkSpriteExporter v{Application.ProductVersion}";

            // load settings and previous files/folders selected
            allowNonIntegerActorPositioningToolStripMenuItem.Checked = Properties.Settings.Default.AllowNonIntegerActorPositioning;
            ignoreStageLengthToolStripMenuItem.Checked = Properties.Settings.Default.IgnoreStageLength;
            ignoreActorDimensionsToolStripMenuItem.Checked = Properties.Settings.Default.IgnoreActorDimensions;

            spriteName.Text = Path.GetFileName(Properties.Settings.Default.SpriteName);
            textureFolderName.Text = Path.GetFileName(Properties.Settings.Default.TextureFolderPath);
            outputFolderName.Text = Path.GetFileName(Properties.Settings.Default.OutputFolderPath);
        }

        private void LoadSpritesButton_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Sprite JSON|*.json|All files|*.*";
            ofd.Title = "Select a sprite JSON file";

            if (ofd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(ofd.FileName))
            {
                string sprite = Path.GetFileName(ofd.FileName);
                spriteName.Text = sprite;

                Properties.Settings.Default.SpriteName = sprite;
                Properties.Settings.Default.SpriteFolderPath = Path.GetDirectoryName(ofd.FileName);
                Properties.Settings.Default.Save();
            }
        }

        private void LoadTexturesFolderButton_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string folder = fbd.SelectedPath;
                textureFolderName.Text = Path.GetFileName(folder);

                Properties.Settings.Default.TextureFolderPath = folder;
                Properties.Settings.Default.Save();
            }
        }

        private void OutputFolderButton_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string folder = fbd.SelectedPath;
                outputFolderName.Text = Path.GetFileName(folder);

                Properties.Settings.Default.OutputFolderPath = folder;
                Properties.Settings.Default.Save();
            }
        }

        private void RenderSingleFrameButton_Click(object sender, EventArgs e)
        {
            // todo: add a way to specify which frame of an animation to render (requires calculating the maximum animation length first)
            Program.ProcessSingleFrame(Properties.Settings.Default.SpriteName, 0, statusStrip, statusProgBar, progBarLabel, spritePreview);
        }

        private void RenderAnimationButton_Click(object sender, EventArgs e)
        {
            Program.ProcessAnimation(Properties.Settings.Default.SpriteName, statusStrip, statusProgBar, progBarLabel, spritePreview);
        }

        private void AllowNonIntegerActorPositioningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.AllowNonIntegerActorPositioning = allowNonIntegerActorPositioningToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void IgnoreActorDimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IgnoreActorDimensions = ignoreActorDimensionsToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void IgnoreStageLengthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.IgnoreStageLength = ignoreStageLengthToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var aboutForm = new AboutForm();
            aboutForm.ShowDialog(this);
        }
    }
}
