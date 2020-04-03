using System.Windows.Forms;

namespace Level_Editor
{
	partial class levelEditorForm
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
			this.tileSelectionComboBox = new System.Windows.Forms.ComboBox();
			this.saveLayoutButton = new System.Windows.Forms.Button();
			this.messageLabel = new System.Windows.Forms.Label();
			this.loadLayoutButton = new System.Windows.Forms.Button();
			this.currentMapLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// tileSelectionComboBox
			// 
			this.tileSelectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.tileSelectionComboBox.FormattingEnabled = true;
			this.tileSelectionComboBox.Items.AddRange(new object[] {
            "wall",
            "floor"});
			this.tileSelectionComboBox.Location = new System.Drawing.Point(54, 388);
			this.tileSelectionComboBox.Name = "tileSelectionComboBox";
			this.tileSelectionComboBox.Size = new System.Drawing.Size(121, 21);
			this.tileSelectionComboBox.TabIndex = 3;
			// 
			// saveLayoutButton
			// 
			this.saveLayoutButton.Location = new System.Drawing.Point(54, 415);
			this.saveLayoutButton.Name = "saveLayoutButton";
			this.saveLayoutButton.Size = new System.Drawing.Size(75, 23);
			this.saveLayoutButton.TabIndex = 4;
			this.saveLayoutButton.Text = "Save Layout";
			this.saveLayoutButton.UseVisualStyleBackColor = true;
			this.saveLayoutButton.Click += new System.EventHandler(this.SaveLayoutButton_Click);
			// 
			// messageLabel
			// 
			this.messageLabel.AutoSize = true;
			this.messageLabel.Location = new System.Drawing.Point(51, 367);
			this.messageLabel.Name = "messageLabel";
			this.messageLabel.Size = new System.Drawing.Size(323, 13);
			this.messageLabel.TabIndex = 5;
			this.messageLabel.Text = "Select a tile type from the dropdown. Click on the grid to place tiles.";
			// 
			// loadLayoutButton
			// 
			this.loadLayoutButton.Location = new System.Drawing.Point(135, 415);
			this.loadLayoutButton.Name = "loadLayoutButton";
			this.loadLayoutButton.Size = new System.Drawing.Size(75, 23);
			this.loadLayoutButton.TabIndex = 6;
			this.loadLayoutButton.Text = "Load Layout";
			this.loadLayoutButton.UseVisualStyleBackColor = true;
			this.loadLayoutButton.Click += new System.EventHandler(this.LoadLayoutButton_Click);
			// 
			// currentMapLabel
			// 
			this.currentMapLabel.AutoSize = true;
			this.currentMapLabel.Location = new System.Drawing.Point(51, 9);
			this.currentMapLabel.Name = "currentMapLabel";
			this.currentMapLabel.Size = new System.Drawing.Size(155, 13);
			this.currentMapLabel.TabIndex = 7;
			this.currentMapLabel.Text = "Currently working on: New Map";
			// 
			// levelEditorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.currentMapLabel);
			this.Controls.Add(this.loadLayoutButton);
			this.Controls.Add(this.messageLabel);
			this.Controls.Add(this.saveLayoutButton);
			this.Controls.Add(this.tileSelectionComboBox);
			this.Name = "levelEditorForm";
			this.Text = "Level Editor";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox tileSelectionComboBox;
		private System.Windows.Forms.Button saveLayoutButton;
		private Label messageLabel;
		private Button loadLayoutButton;
		private Label currentMapLabel;
	}
}

