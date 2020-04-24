using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO; // Needed for file IO

namespace Level_Editor
{
	public partial class levelEditorForm : Form
	{
		// File IO variables
		FileStream readStream;
		StreamWriter writer;
		StreamReader reader;

		// List to store all PictureBox objects used to represent map tiles.
		List<PictureBox> tileList;

		// Variables to replace magic numbers
		Color wallColor = Color.DarkSlateGray;
		Color floorColor = Color.DarkGray;
		Color professorColor = Color.Fuchsia;
		Color emptySquareColor = Color.White;

		// Adjust these based on the map's size.
		int mapLength = 22;
		int mapHeight = 22;

		public levelEditorForm()
		{
			InitializeComponent();
			tileList = new List<PictureBox>();
		}

		// Loading and creating all controls that were not created via WYSIWYG
		private void Form1_Load(object sender, EventArgs e)
		{
			// Drawing a grid of PictureBox objects
			for (int rows = 0; rows < mapHeight; rows++)
			{
				for (int columns = 0; columns < mapLength; columns++)
				{
					PictureBox newPictureBox = new PictureBox();

					// Set PictureBox's parameters
					newPictureBox.Width = 20; // When dealing with bigger maps, might need to 
					newPictureBox.Height = 20; // lower these values so all PictureBoxes fit onscreen.
					newPictureBox.BackColor = floorColor;
					// Offset PictureBoxes from each other by 10 pixels
					// Note how type Point works; an instance must be created 
					//		before assignation
					newPictureBox.Location = new Point(
						columns * (2 + newPictureBox.Width) + 51,
						rows * (2 + newPictureBox.Height) + 30);

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
									newPictureBox.BackColor = wallColor;
									messageLabel.ForeColor = Color.Black;
									messageLabel.Text =
										"Select a tile type from the dropdown. Click on the grid to place tiles.";
									break;
								}
							case ("floor"):
								{
									newPictureBox.BackColor = floorColor;
									messageLabel.ForeColor = Color.Black;
									messageLabel.Text =
										"Select a tile type from the dropdown. Click on the grid to place tiles.";
									break;
								}
							case ("professor"):
								{
									newPictureBox.BackColor = professorColor;
									messageLabel.ForeColor = Color.Black;
									messageLabel.Text =
										"Select a tile type from the dropdown. Click on the grid to place tiles.";
									break;
								}
							default:
								{
									messageLabel.ForeColor = Color.Black;
									messageLabel.Text =
										"Select a tile type from the dropdown first.";
									break;
								}
						}
					}

					// Manually subscribing the form to these click events
					// This must be done because the PictureBoxes only exist 
					//		AFTER the form is created - WYSIWYG was not used.
					newPictureBox.Click += newPictureBox_Click;
				}
			}
		}

		/// <summary>
		/// Helper method to check if all tiles on the map grid are filled.
		/// </summary>
		/// <returns>
		/// Returns true if all tiles on the map grid are filled, and false
		///		otherwise.
		/// </returns>
		private bool CheckAllTilesFilled()
		{
			foreach (PictureBox tile in tileList)
			{
				if (tile.BackColor == emptySquareColor)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Writes all changes made on the map grid to a save file.
		/// </summary>
		/// <param name="filename">
		/// The location to write save data to.
		/// </param>
		private void SaveToFile(string filename)
		{
			try
			{
				writer = new StreamWriter(filename);

				// Variable track the number of characters written in a line.
				int lineBreakCtrl = 0;

				// Looping through tileList and taking each PictureBox's 
				//		BackColor to guide save file writing
				foreach (PictureBox tile in tileList)
				{
					if (tile.BackColor == wallColor)
					{
						// Separate chars using spaces for now
						writer.Write("W");
					}
					else if (tile.BackColor == floorColor)
					{
						// Separate chars using spaces for now
						writer.Write("F");
					}
					else if (tile.BackColor == professorColor)
					{
						writer.Write("P");
					}

					lineBreakCtrl++;

					// Create a new line every time map length limit is reached.
					if (lineBreakCtrl == mapLength)
					{
						writer.WriteLine();
						lineBreakCtrl = 0;
					}
					// Otherwise, separate characters with a space character.
					else
					{
						writer.Write(" ");
					}	
				}

				// Alter currentMapLabel to display the filepath of the map currently 
				//		being worked on.
				currentMapLabel.Text = "Currently working on: " + filename;

				// Make messageLabel display a confirmation message.
				messageLabel.ForeColor = Color.Green;
				messageLabel.Text = "Save successful - Saved file to: " + filename;
			}
			catch
			{
				// Make messageLabel display an error message.
				messageLabel.ForeColor = Color.Red;
				messageLabel.Text = "Error encountered while attempting to save data.";
			}
			finally
			{
				if (writer != null)
				{
					writer.Close();
				}
			}
		}

		/// <summary>
		/// Helper method to interpret save file data.
		/// </summary>
		/// <param name="letter">
		/// Each individual letter from the save file.
		/// </param>
		/// <returns>
		/// Returns one of three colours depending on the letter supplied from 
		///		the save file.
		/// </returns>
		private Color AnalyzeColor(string letter)
		{
			switch (letter)
			{
				case ("F"):
					{
						return floorColor;
					}
				case ("W"):
					{
						return wallColor;
					}
				case ("P"):
					{
						return professorColor;
					}
				// Any other character results in the default square colour 
				//		and an error message.
				default:
					{
						messageLabel.ForeColor = Color.Red;
						messageLabel.Text = "Error - save file data invalid or corrupted.";
						return emptySquareColor;
					}
			}
		}

		/// <summary>
		/// Reads map grid data from a save file and displays the result on 
		///		the screen.
		/// </summary>
		/// <param name="filename">
		/// The location of the save file to read from.
		/// </param>
		private void LoadFromFile(string filename)
		{
			try
			{
				readStream = File.OpenRead(filename);
				reader = new StreamReader(readStream);
				string lineOfText = null;

				// List to keep a running inventory of all results of the 
				//		split.
				List<string> tileTypeList = new List<string>();

				while ((lineOfText = reader.ReadLine()) != null)
				{
					// Array to hold the results of the currently split line.
					string[] splitArray = lineOfText.Split(' ');

					for (int i = 0; i < splitArray.Length; i++)
					{
						// Adding to the running inventory in the list.
						tileTypeList.Add(splitArray[i]);
					}
				}

				// Loop through the list of PictureBoxes, and applying the 
				//		appropriate BackColor.
				// A foreach loop was unsuitable, since indices were important.
				for (int i = 0; i < tileList.Count; i++)
				{
					tileList[i].BackColor = AnalyzeColor(tileTypeList[i]);
				}

				// Alter currentMapLabel to display the filepath of the map currently 
				//		being worked on.
				currentMapLabel.Text = "Currently working on: " + filename;

				// Make messageLabel display a confirmation message.
				messageLabel.ForeColor = Color.Green;
				messageLabel.Text = "Load successful - now working on: " + filename;
			}
			catch
			{
				// Make messageLabel display an error message.
				messageLabel.ForeColor = Color.Red;
				messageLabel.Text = "Error - save file data incompatible or corrupted.";
			}
			finally
			{
				if (reader != null)
				{
					reader.Close();
				}
			}
		}

		private void SaveLayoutButton_Click(object sender, EventArgs e)
		{
			// Do not allow user to save work if tiles are not all filled.
			if (!CheckAllTilesFilled())
			{
				messageLabel.ForeColor = Color.Red;
				messageLabel.Text = "All squares must be filled!";
				return;
			}

			SaveFileDialog fileDialog = new SaveFileDialog();
			fileDialog.ShowDialog();
			string filename = fileDialog.FileName;
			this.SaveToFile(filename);
		}

		private void LoadLayoutButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog fileDialog = new OpenFileDialog();
			fileDialog.ShowDialog();
			string filename = fileDialog.FileName;
			this.LoadFromFile(filename);
		}
	}
}
