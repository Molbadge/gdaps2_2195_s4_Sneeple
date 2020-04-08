namespace MainMenu
{
    partial class MainMenu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Title = new System.Windows.Forms.Label();
            this.Play = new System.Windows.Forms.Button();
            this.Sneeple = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Argon Tight", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Title.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Title.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Title.Location = new System.Drawing.Point(15, 123);
            this.Title.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(1038, 59);
            this.Title.TabIndex = 0;
            this.Title.Text = "Schwartz\'s Sneaky Snail Mail Scandal";
            this.Title.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Play
            // 
            this.Play.AutoSize = true;
            this.Play.BackColor = System.Drawing.Color.Black;
            this.Play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Play.Font = new System.Drawing.Font("Argon Tight", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Play.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Play.Location = new System.Drawing.Point(134, 500);
            this.Play.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(800, 181);
            this.Play.TabIndex = 1;
            this.Play.Text = "Play";
            this.Play.UseVisualStyleBackColor = false;
            this.Play.Click += new System.EventHandler(this.OnMouseLeavePlay);
            // 
            // Sneeple
            // 
            this.Sneeple.AutoSize = true;
            this.Sneeple.Font = new System.Drawing.Font("Argon Tight", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sneeple.Location = new System.Drawing.Point(398, 279);
            this.Sneeple.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.Sneeple.Name = "Sneeple";
            this.Sneeple.Size = new System.Drawing.Size(268, 49);
            this.Sneeple.TabIndex = 2;
            this.Sneeple.Text = "By Sneeple";
            this.Sneeple.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.ClientSize = new System.Drawing.Size(1081, 930);
            this.Controls.Add(this.Sneeple);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.Title);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "MainMenu";
            this.Text = "MainMenu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.Label Sneeple;
    }
}

