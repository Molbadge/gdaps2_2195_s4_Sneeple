using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

// TO-DO:
// Ensure that all squares are filled before saving
// Load from preset template
// Get rid of all the magic numbers (use enums for writer chars, maybe use for colours too)

namespace Level_Editor
{
	public partial class levelEditorForm : Form
	{
		StreamWriter writer;

		// List to store all PictureBox objects used to represent map tiles.
		List<PictureBox> tileList;

		Color wallColor = Color.Gray;
		Color floorColor = Color.Blue;

		public levelEditorForm()
		{
			InitializeComponent();
			tileList = new List<PictureBox>();
		}
			
		private void Form1_Load(object sender, EventArgs e)
		{
			// Drawing a grid of PictureBox objects
			// i = rows, j = columns
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					PictureBox newPictureBox = new PictureBox();

					// Set PictureBox's parameters
					newPictureBox.Name = "tile" + i + ":" + j;
					newPictureBox.Width = 50;
					newPictureBox.Height = 50;
					newPictureBox.BackColor = Color.White;
					// Offset PictureBoxes from each other by 10 pixels
					// Note how type Point works; an instance must be created 
					//		before assignation
					newPictureBox.Location = new Point(
						j * (2 + newPictureBox.Width) + 10,
						i * (2 + newPictureBox.Height) + 10);

					tileList.Add(newPictureBox);

					// Important! Without this, controls will not display!
					Controls.Add(newPictureBox);

					// Adding click event handlers to each PictureBox manually
					// Different var names must be used for object and EventArgs,
					//		because this is nested in a method that already uses
					//		object sender and EventArgs e. Any alt name will do.
					// Also, apparently access modifiers are not allowed on 
					//		"local functions", whatever that means. I think it's
					//		because this method is being declared within another 
					//		that C# isn't allowing me to declare it as private.
					void newPictureBox_Click(object newSender, EventArgs newE)
					{
						// Change colour of PictureBoxes when they are clicked on
						//		depending on the selected ComboBox value
						switch (tileSelectionComboBox.Text)
						{
							case ("wall"):
								{
									newPictureBox.BackColor = Color.Gray;
									break;
								}
							case ("floor"):
								{
									newPictureBox.BackColor = Color.Blue;
									break;
								}
							default:
								{
									Console.WriteLine("This shouldn't happen");
									break;
								}
						}
					}

					// Manually subscribing the form to these click events
					// This must be done because the PictureBoxes only exist 
					//		AFTER the form is created - WYSIWYG was not used.
					newPictureBox.Click += new EventHandler(newPictureBox_Click);
				}
			}
		}

		private void SaveToFile(string filename)
		{
			try
			{
				writer = new StreamWriter(filename);

				int lineBreakCtrl = 0;

				// Looping through tileList and taking each PictureBox's 
				//		BackColor to guide save file writing
				foreach (PictureBox tile in tileList)
				{
					// Create a new line every 5 chars
					if (lineBreakCtrl == 5)
					{
						writer.WriteLine();
						lineBreakCtrl = 0;
					}

					if (tile.BackColor == wallColor)
					{
						// Separate using spaces for now
						writer.Write("W ");
					}
					else if (tile.BackColor == floorColor)
					{
						// Separate using spaces for now
						writer.Write("F ");
					}
					
					lineBreakCtrl++;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
			}
		}

		private void SaveLayoutButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.ShowDialog();
			string filename = fileDialog.FileName;
			this.SaveToFile(filename);
		}
	}
}
