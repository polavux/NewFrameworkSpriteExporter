
namespace NewFrameworkSpriteExporter
{
    partial class NewFrameworkSpriteExporterForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            loadSpritesButton = new System.Windows.Forms.Button();
            statusStrip = new System.Windows.Forms.StatusStrip();
            statusProgBar = new System.Windows.Forms.ToolStripProgressBar();
            progBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            loadTexturesFolderButton = new System.Windows.Forms.Button();
            spritePreview = new System.Windows.Forms.PictureBox();
            spriteName = new System.Windows.Forms.Label();
            textureFolderName = new System.Windows.Forms.Label();
            renderAnimationButton = new System.Windows.Forms.Button();
            outputFolderButton = new System.Windows.Forms.Button();
            outputFolderName = new System.Windows.Forms.Label();
            menuStrip = new System.Windows.Forms.MenuStrip();
            optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            allowNonIntegerActorPositioningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ignoreActorDimensionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ignoreStageLengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            renderSingleFrameButton = new System.Windows.Forms.Button();
            aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)spritePreview).BeginInit();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // loadSpritesButton
            // 
            loadSpritesButton.AllowDrop = true;
            loadSpritesButton.Location = new System.Drawing.Point(19, 43);
            loadSpritesButton.Name = "loadSpritesButton";
            loadSpritesButton.Size = new System.Drawing.Size(200, 93);
            loadSpritesButton.TabIndex = 0;
            loadSpritesButton.Text = "Choose sprite...";
            loadSpritesButton.UseVisualStyleBackColor = true;
            loadSpritesButton.Click += LoadSpritesButton_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statusProgBar, progBarLabel });
            statusStrip.Location = new System.Drawing.Point(0, 647);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new System.Drawing.Size(1262, 26);
            statusStrip.TabIndex = 1;
            // 
            // statusProgBar
            // 
            statusProgBar.Name = "statusProgBar";
            statusProgBar.Size = new System.Drawing.Size(100, 18);
            // 
            // progBarLabel
            // 
            progBarLabel.Name = "progBarLabel";
            progBarLabel.Size = new System.Drawing.Size(34, 20);
            progBarLabel.Text = "Idle";
            // 
            // loadTexturesFolderButton
            // 
            loadTexturesFolderButton.AllowDrop = true;
            loadTexturesFolderButton.Location = new System.Drawing.Point(19, 142);
            loadTexturesFolderButton.Name = "loadTexturesFolderButton";
            loadTexturesFolderButton.Size = new System.Drawing.Size(200, 93);
            loadTexturesFolderButton.TabIndex = 2;
            loadTexturesFolderButton.Text = "Choose textures folder...";
            loadTexturesFolderButton.UseVisualStyleBackColor = true;
            loadTexturesFolderButton.Click += LoadTexturesFolderButton_Click;
            // 
            // spritePreview
            // 
            spritePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            spritePreview.Location = new System.Drawing.Point(662, 31);
            spritePreview.Name = "spritePreview";
            spritePreview.Size = new System.Drawing.Size(600, 600);
            spritePreview.TabIndex = 3;
            spritePreview.TabStop = false;
            // 
            // spriteName
            // 
            spriteName.AutoSize = true;
            spriteName.Location = new System.Drawing.Point(226, 77);
            spriteName.Name = "spriteName";
            spriteName.Size = new System.Drawing.Size(120, 20);
            spriteName.TabIndex = 4;
            spriteName.Text = "(no file selected)";
            // 
            // textureFolderName
            // 
            textureFolderName.AutoSize = true;
            textureFolderName.Location = new System.Drawing.Point(226, 180);
            textureFolderName.Name = "textureFolderName";
            textureFolderName.Size = new System.Drawing.Size(139, 20);
            textureFolderName.TabIndex = 5;
            textureFolderName.Text = "(no folder selected)";
            // 
            // renderAnimationButton
            // 
            renderAnimationButton.Location = new System.Drawing.Point(12, 615);
            renderAnimationButton.Name = "renderAnimationButton";
            renderAnimationButton.Size = new System.Drawing.Size(152, 29);
            renderAnimationButton.TabIndex = 6;
            renderAnimationButton.Text = "Render animation";
            renderAnimationButton.UseVisualStyleBackColor = true;
            renderAnimationButton.Click += RenderAnimationButton_Click;
            // 
            // outputFolderButton
            // 
            outputFolderButton.Location = new System.Drawing.Point(19, 241);
            outputFolderButton.Name = "outputFolderButton";
            outputFolderButton.Size = new System.Drawing.Size(200, 93);
            outputFolderButton.TabIndex = 7;
            outputFolderButton.Text = "Choose output folder...";
            outputFolderButton.UseVisualStyleBackColor = true;
            outputFolderButton.Click += OutputFolderButton_Click;
            // 
            // outputFolderName
            // 
            outputFolderName.AutoSize = true;
            outputFolderName.Location = new System.Drawing.Point(226, 278);
            outputFolderName.Name = "outputFolderName";
            outputFolderName.Size = new System.Drawing.Size(139, 20);
            outputFolderName.TabIndex = 8;
            outputFolderName.Text = "(no folder selected)";
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { optionsToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(1262, 28);
            menuStrip.TabIndex = 9;
            menuStrip.Text = "menuStrip";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { allowNonIntegerActorPositioningToolStripMenuItem, ignoreActorDimensionsToolStripMenuItem, ignoreStageLengthToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new System.Drawing.Size(124, 24);
            optionsToolStripMenuItem.Text = "Render options";
            // 
            // allowNonIntegerActorPositioningToolStripMenuItem
            // 
            allowNonIntegerActorPositioningToolStripMenuItem.CheckOnClick = true;
            allowNonIntegerActorPositioningToolStripMenuItem.Name = "allowNonIntegerActorPositioningToolStripMenuItem";
            allowNonIntegerActorPositioningToolStripMenuItem.Size = new System.Drawing.Size(329, 26);
            allowNonIntegerActorPositioningToolStripMenuItem.Text = "Allow non-integer actor positioning";
            allowNonIntegerActorPositioningToolStripMenuItem.ToolTipText = "Uses ImageMagick distortion to interpolate sprite translation. Increases render time by a lot, but is more accurate.";
            allowNonIntegerActorPositioningToolStripMenuItem.Click += AllowNonIntegerActorPositioningToolStripMenuItem_Click;
            // 
            // ignoreActorDimensionsToolStripMenuItem
            // 
            ignoreActorDimensionsToolStripMenuItem.CheckOnClick = true;
            ignoreActorDimensionsToolStripMenuItem.Name = "ignoreActorDimensionsToolStripMenuItem";
            ignoreActorDimensionsToolStripMenuItem.Size = new System.Drawing.Size(329, 26);
            ignoreActorDimensionsToolStripMenuItem.Text = "Ignore actor dimensions";
            ignoreActorDimensionsToolStripMenuItem.Click += IgnoreActorDimensionsToolStripMenuItem_Click;
            // 
            // ignoreStageLengthToolStripMenuItem
            // 
            ignoreStageLengthToolStripMenuItem.CheckOnClick = true;
            ignoreStageLengthToolStripMenuItem.Name = "ignoreStageLengthToolStripMenuItem";
            ignoreStageLengthToolStripMenuItem.Size = new System.Drawing.Size(329, 26);
            ignoreStageLengthToolStripMenuItem.Text = "Ignore stage length";
            ignoreStageLengthToolStripMenuItem.Click += IgnoreStageLengthToolStripMenuItem_Click;
            // 
            // renderSingleFrameButton
            // 
            renderSingleFrameButton.Location = new System.Drawing.Point(12, 580);
            renderSingleFrameButton.Name = "renderSingleFrameButton";
            renderSingleFrameButton.Size = new System.Drawing.Size(152, 29);
            renderSingleFrameButton.TabIndex = 10;
            renderSingleFrameButton.Text = "Render single frame";
            renderSingleFrameButton.UseVisualStyleBackColor = true;
            renderSingleFrameButton.Click += RenderSingleFrameButton_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new System.Drawing.Size(64, 24);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += AboutToolStripMenuItem_Click;
            // 
            // NewFrameworkSpriteExporterForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1262, 673);
            Controls.Add(renderSingleFrameButton);
            Controls.Add(outputFolderName);
            Controls.Add(outputFolderButton);
            Controls.Add(renderAnimationButton);
            Controls.Add(textureFolderName);
            Controls.Add(spriteName);
            Controls.Add(spritePreview);
            Controls.Add(loadTexturesFolderButton);
            Controls.Add(statusStrip);
            Controls.Add(menuStrip);
            Controls.Add(loadSpritesButton);
            MainMenuStrip = menuStrip;
            Name = "NewFrameworkSpriteExporterForm";
            Text = "---";
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)spritePreview).EndInit();
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loadSpritesButton;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripProgressBar statusProgBar;
        private System.Windows.Forms.Button loadTexturesFolderButton;
        private System.Windows.Forms.PictureBox spritePreview;
        private System.Windows.Forms.Label spriteName;
        private System.Windows.Forms.Label textureFolderName;
        private System.Windows.Forms.ToolStripStatusLabel progBarLabel;
        private System.Windows.Forms.Button outputFolderButton;
        private System.Windows.Forms.Label outputFolderName;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowNonIntegerActorPositioningToolStripMenuItem;
        private System.Windows.Forms.Button renderSingleFrameButton;
        private System.Windows.Forms.Button renderAnimationButton;
        private System.Windows.Forms.ToolStripMenuItem ignoreActorDimensionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ignoreStageLengthToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    }
}

