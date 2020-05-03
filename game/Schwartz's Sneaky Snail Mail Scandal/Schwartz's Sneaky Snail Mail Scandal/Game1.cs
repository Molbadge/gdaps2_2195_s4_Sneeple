using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO; // Needed for file IO

namespace Schwartz_s_Sneaky_Snail_Mail_Scandal
{
	//   enum Rooms
	//{
	//	Hallway,
	//	Office
	//}
	enum Professors
	{
		Erika,	
		Erin,
		Schwartz,
		Luis,
		None // Represented by an extra enum because enums are non-nullable.
	}

	enum GameStates
	{
		StartScreen,
		SchwartzIntroScreen,
		Playing,
		FinalSelectionScreen1, // The screens where players pick the culprit out I can't 
		FinalSelectionScreen2, //		think of names rn lol forgive me i have sinned
		WinScreen,
		LoseScreen
	}

	enum PlayerActivity
	{
		Idle,
		Active
	}
	
	/// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
		#region Fields
		GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Player to draw based on state
        Player player;

        // Constant to hold player speed when moving
        const int PlayerSpeed = 2;

        // Constant to hold player speed when stationary (0)
        const int PlayerStationarySpeed = 0;

        // Constant that controls the rate at which player bounces off items 
        //		when colliding with them
        const float BounceFactor = 0.5f;

        //Map to draw based on state
        List<Map> worldMap = new List<Map>();

        // Variables to store screen size
        int windowWidth = 0;
        int windowHeight = 0;

		//Rectangle for player collision tracking
		Rectangle playerTracker;

        // File IO variables
        FileStream readStream;
        StreamReader reader;

        // List to store all PictureBox objects used to represent map tiles.
        List<TileStates> tileList = new List<TileStates>();
        List<Rectangle> wallBoundaries = new List<Rectangle>();
        int mapWidth = 0;
        int mapHeight = 0;

		//// Keep track of the room the player is currently in.
		////Rooms currentRoom = Rooms.Hallway;
		
		// Variable to store directory location of game map
		string mapDirectory = null;

		//// Regions that, when entered, trigger the loading of the next room
		//RoomMovement hallwayExit = null;
		//RoomMovement officeExit = null;

		// Variables to keep track of game assets
		Texture2D spriteSheet;
		Texture2D tileSheet;
		Texture2D startScreen;
		Texture2D introScreen;
		Texture2D finalSelectionScreen1;
		Texture2D finalSelectionScreen2;
		Texture2D winScreen;
		Texture2D loseScreen;
		Texture2D schwartzDialogue;
		Texture2D erikaDialogue;
		Texture2D erinDialogue;
		Texture2D luisDialogue;
		Texture2D erikaSnailshot;
		Texture2D erinSnailshot;
		Texture2D luisSnailshot;

		// List to hold Rectangles surrounding all professor tiles
		List<Rectangle> professorTileRectangles = new List<Rectangle>();

		// Variables to track the current state of the game and player
		GameStates gameState = GameStates.StartScreen;
		PlayerActivity playerActivity = PlayerActivity.Idle;

		// Variable for spritefont
		SpriteFont arial20;

		// Enum variable to track professor tile player is interacting with
		Professors activeProfessor;

		// Bool for checking professor interaction
		bool professorInteraction = false;

		// Variable to keep track of previous keyboard state
		KeyboardState prevKBState;

		// Variable to keep track of player starting position
		Vector2 playerLoc = new Vector2(1225, 50);

		// Variables to control the regions of the map that get drawn.
		//int drawFrom;
		//int drawTo;
		#endregion


		#region Constructor
		public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
		#endregion

		#region Methods
		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = Map.tileWidth * 44;
            graphics.PreferredBackBufferHeight = Map.tileHeight * 22;
            graphics.ApplyChanges();

            windowWidth = graphics.GraphicsDevice.Viewport.Width;
            windowHeight = graphics.GraphicsDevice.Viewport.Height;

			//hallwayExit = new RoomMovement(
			//	0, Map.tileHeight, Map.tileWidth, Map.tileHeight * 20);
			//officeExit = new RoomMovement(
			//	windowWidth - Map.tileWidth, Map.tileHeight * 14, Map.tileWidth, Map.tileHeight * 7);

			// Making cursor visible in the window.
			this.IsMouseVisible = true;

            base.Initialize();
        }
        ///<summary>
        ///Helper method to assign tile enum to saved files
        /// 
        /// </summary>
        private TileStates AssignTile(string letter)
        {
            switch (letter)
            {
                case ("F"):
                    {
                        return TileStates.Floor;
                    }
                case ("W"):
                    {
                        return TileStates.Wall;
                    }
				case ("P"):
					{
						return TileStates.Professor;
					}
                default:
                    throw new ArgumentException(letter + " was not a wall or floor. Please check file input.");
            }
        }

        /// <summary>
        /// Reads map grid data from a save file and displays the result on 
        ///		the screen.
        /// </summary>
        /// <param name="filename">
        /// The location of the save file to read from.
        /// </param>
        private List<TileStates> LoadFromFile(string filename)
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
                    //saves line length for making map width
                    if (mapWidth == 0)
                    {
                        mapWidth = splitArray.Length;
                    }

                    for (int i = 0; i < splitArray.Length; i++)
                    {
                        // Adding to the running inventory in the list.
                        tileTypeList.Add(splitArray[i]);
                    }
                }

                // Loop through the list of PictureBoxes, and applying the 
                //		appropriate tile enum.
                // A foreach loop was unsuitable, since indices were important.
                for (int i = 0; i < tileTypeList.Count; i++)
                {
                    tileList.Add(AssignTile(tileTypeList[i]));
                }
                if (reader != null)
                {
                    reader.Close();
                }
                //Assigns map height
                mapHeight = tileList.Count / mapWidth;

                return tileList;
            }
            catch
            {
                // Make messageLabel display an error message.                
                Console.WriteLine("Error - save file data incompatible or corrupted.");
                if (reader != null)
                {
                    reader.Close();
                }
                return null;
            }
        }

		/// <summary>
		/// Adds tiles to worldMap using file data generated by the map editor.
		/// </summary>
		/// <param name="directory">
		/// The save location of the file generated by the map editor.
		/// </param>
		/// <param name="mapSheet">
		/// The sprite sheet used to draw the map.
		/// </param>
		public void PopulateMap(string directory, Texture2D mapSheet)
		{
			tileList = LoadFromFile(directory);

			for (int y = 0; y < mapHeight; y++)
			{
				for (int x = 0; x < mapWidth; x++)
				{
					int tileIndex = mapWidth * y + x;

					TileStates tempState = tileList[tileIndex];

					Vector2 tempVector = new Vector2(x * 32, y * 32);

					worldMap.Add(new Map(mapSheet, tempVector, tempState));

					//Adds walls to a list of rectangles for border collision
					if (tempState == TileStates.Wall)
					{
						wallBoundaries.Add(new Rectangle(x * 32, y * 32, 32, 32));
					}
				}
			}

			// Populating professorTileRectangles
			foreach (Map tile in worldMap)
			{
				if (tile.TilePlaced == TileStates.Professor)
				{
					professorTileRectangles.Add(new Rectangle(
						(int)tile.X, 
						(int)tile.Y, 
						Map.tileWidth, 
						Map.tileHeight));
				}
			}
		}

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			// Initialising map using the file generated using the Map Editor
			mapDirectory = "../../../../Content/gameMap";

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

			//Spritesheet for ritchie
			spriteSheet = Content.Load<Texture2D>("spriteSheets/Ritchie");

			//Spritesheet for map
			tileSheet = Content.Load<Texture2D>("spriteSheets/Dungeon_Crawler_Sheet");

			// Menu screen images
			startScreen = Content.Load<Texture2D>("menuScreens/SSSMSMenu");
			introScreen = Content.Load<Texture2D>("menuScreens/SSSMSIntro");
			finalSelectionScreen1 = Content.Load<Texture2D>("menuScreens/FinalSelectionScreen1");
			finalSelectionScreen2 = Content.Load<Texture2D>("menuScreens/FinalSelectionScreen2");
			winScreen = Content.Load<Texture2D>("menuScreens/winScreen");
			loseScreen = Content.Load<Texture2D>("menuScreens/loseScreen");

			// Dialogue boxes
			schwartzDialogue = Content.Load<Texture2D>("dialogueBoxes/SchwartzDialogue");
			erikaDialogue = Content.Load<Texture2D>("dialogueBoxes/ErikaDialogue");
			erinDialogue = Content.Load<Texture2D>("dialogueBoxes/ErinDialogue");
			luisDialogue = Content.Load<Texture2D>("dialogueBoxes/LuisDialogue");

			// "Snailshots" lol
			erikaSnailshot = Content.Load<Texture2D>("snailShots/erikaSnailshot");
			erinSnailshot = Content.Load<Texture2D>("snailShots/erinSnailshot");
			luisSnailshot = Content.Load<Texture2D>("snailShots/luisSnailshot");

			// Loads font
			arial20 = Content.Load<SpriteFont>("Arial20");
	

			PopulateMap(mapDirectory, tileSheet);

			// TODO: use this.Content to load your game content here

            player = new Player(spriteSheet, playerLoc, PlayerStates.FaceDown);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            //Handles animation
            player.UpdateAnimation(gameTime);

            //Get User input
            KeyboardState kbState = Keyboard.GetState();

			#region Current Game State FSM - also see corresponding FSM in Draw()
			switch (gameState)
			{
				case (GameStates.StartScreen):
					{
						// If Enter is pressed (once), show Schwartz's dialogue intro.
						if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
						{
							gameState = GameStates.SchwartzIntroScreen;
						}

						break;
					}
				case (GameStates.SchwartzIntroScreen):
					{
						// If Enter is pressed (once), start the game.
						if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
						{
							gameState = GameStates.Playing;
						}

						break;
					}
				case (GameStates.Playing):
					{
						// "Activate" the player FSM
						playerActivity = PlayerActivity.Active;
						break;
					}
				case (GameStates.FinalSelectionScreen1):
					{
						// "Dectivate" the player FSM
						playerActivity = PlayerActivity.Idle;

						// If Y is pressed (once), go to the second final selection screen.
						if (kbState.IsKeyDown(Keys.Y) && prevKBState.IsKeyUp(Keys.Y))
						{
							gameState = GameStates.FinalSelectionScreen2;
						}
						// If N is pressed (once), return to the game.
						else if (kbState.IsKeyDown(Keys.N) && prevKBState.IsKeyUp(Keys.N))
						{
							// Nudge the player away from Schwartz's tile so 
							//		FinalSelectionScreen1 doesn't continuously trigger.
							player.X += 50;
							gameState = GameStates.Playing;
						}

						break;
					}
				case (GameStates.FinalSelectionScreen2):
					{

						// If E is pressed (once), the player loses.
						if (kbState.IsKeyDown(Keys.E) && prevKBState.IsKeyUp(Keys.E))
						{
							gameState = GameStates.LoseScreen;
						}
						// If B is pressed (once), the player wins.
						else if (kbState.IsKeyDown(Keys.B) && prevKBState.IsKeyUp(Keys.B))
						{
							gameState = GameStates.WinScreen;
						}
						// If H is pressed (once), the player loses.
						else if (kbState.IsKeyDown(Keys.H) && prevKBState.IsKeyUp(Keys.H))
						{
							gameState = GameStates.LoseScreen;
						}
						// If C is pressed (once), the player loses.
						else if (kbState.IsKeyDown(Keys.C) && prevKBState.IsKeyUp(Keys.C))
						{
							gameState = GameStates.LoseScreen;
						}

						break;
					}
				case (GameStates.WinScreen):
					{
						// If Enter is pressed (once), the game exits.
						if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
						{
							Exit();
						}
						// If R is pressed (once), return to the start screen
						else if (kbState.IsKeyDown(Keys.R) && prevKBState.IsKeyUp(Keys.R))
						{
							// Move player into starting position. Useful if 
							//		game is restarted from win/lose screens.
							player.X = playerLoc.X;
							player.Y = playerLoc.Y;
							gameState = GameStates.StartScreen;
						}
						break;
					}
				case (GameStates.LoseScreen):
					{
						// If Enter is pressed (once), the game exits.
						if (kbState.IsKeyDown(Keys.Enter) && prevKBState.IsKeyUp(Keys.Enter))
						{
							Exit();
						}
						// If R is pressed (once), return to the start screen
						else if (kbState.IsKeyDown(Keys.R) && prevKBState.IsKeyUp(Keys.R))
						{
							// Move player into starting position. Useful if 
							//		game is restarted from win/lose screens.
							player.X = playerLoc.X;
							player.Y = playerLoc.Y;
							gameState = GameStates.StartScreen;
						}
						break;
					}
			}
			#endregion

			//Fail safe for if the player ends up outside the window screen
			if(playerTracker.X < 0)
			{
				player.X = 50;
				player.Y = 50;
			}
			if(playerTracker.Y < 0)
			{
				player.X = 50;
				player.Y = 50;
			}
			if(playerTracker.X > windowWidth)
			{
				player.X = 50;
				player.Y = 50;
			}
			if(playerTracker.Y > windowHeight)
			{
				player.X = 50;
				player.Y = 50;
			}

			#region Walk States FSM
			//Switch statement for Walking states
			switch (playerActivity)
			{
				case (PlayerActivity.Idle):
					{
						// Do nothing - player character is idle.
						// This prevents the player from moving their character 
						//		while the start screen is showing.
						break;
					}
				case (PlayerActivity.Active):
					{
						// Player is now active and has access to movement.
						switch (player.State)
						{
							//Case for facing Down
							case PlayerStates.FaceDown:
								//-----Transition to standing states--------
								if (kbState.IsKeyDown(Keys.W))
								{
									player.State = PlayerStates.FaceUp;
								}
								if (kbState.IsKeyDown(Keys.A))
								{
									player.State = PlayerStates.FaceLeft;
								}
								if (kbState.IsKeyDown(Keys.D))
								{
									player.State = PlayerStates.FaceRight;
								}
								//-----Transition to walking state---------
								if (kbState.IsKeyDown(Keys.S))
								{
									player.State = PlayerStates.WalkDown;
								}
								break;

							//Case for facing right
							case PlayerStates.FaceRight:
								//-----Transition to standing states--------
								if (kbState.IsKeyDown(Keys.W))
								{
									player.State = PlayerStates.FaceUp;
								}
								if (kbState.IsKeyDown(Keys.A))
								{
									player.State = PlayerStates.FaceLeft;
								}
								if (kbState.IsKeyDown(Keys.S))
								{
									player.State = PlayerStates.FaceDown;
								}
								//-----Transition to walking state---------
								if (kbState.IsKeyDown(Keys.D))
								{
									player.State = PlayerStates.WalkRight;
								}
								break;

							//Case for facing Left
							case PlayerStates.FaceLeft:
								//-----Transition to standing states--------
								if (kbState.IsKeyDown(Keys.W))
								{
									player.State = PlayerStates.FaceUp;
								}
								if (kbState.IsKeyDown(Keys.S))
								{
									player.State = PlayerStates.FaceDown;
								}
								if (kbState.IsKeyDown(Keys.D))
								{
									player.State = PlayerStates.FaceRight;
								}
								//-----Transition to walking state---------
								if (kbState.IsKeyDown(Keys.A))
								{
									player.State = PlayerStates.WalkLeft;
								}
								break;

							//Case for Facing Up
							case PlayerStates.FaceUp:
								//-----Transition to standing states--------
								if (kbState.IsKeyDown(Keys.A))
								{
									player.State = PlayerStates.FaceLeft;
								}
								if (kbState.IsKeyDown(Keys.S))
								{
									player.State = PlayerStates.FaceDown;
								}
								if (kbState.IsKeyDown(Keys.D))
								{
									player.State = PlayerStates.FaceRight;
								}
								//-----Transition to walking state---------
								if (kbState.IsKeyDown(Keys.W))
								{
									player.State = PlayerStates.WalkUp;
								}
								break;


							//Case for walking Down
							case PlayerStates.WalkDown:
								if (kbState.IsKeyDown(Keys.S))
								{
									player.State = PlayerStates.WalkDown;        //Keeps Walking down
								}
								if (kbState.IsKeyUp(Keys.S))
								{
									player.State = PlayerStates.FaceDown;        //Changes to facing down state
								}
								break;

							//Case for walking right
							case PlayerStates.WalkRight:
								if (kbState.IsKeyDown(Keys.D))
								{
									player.State = PlayerStates.WalkRight;          //Keeps walking right
								}
								if (kbState.IsKeyUp(Keys.D))
								{
									player.State = PlayerStates.FaceRight;          //Changes to facing right state
								}
								break;

							//Case for walking left
							case PlayerStates.WalkLeft:
								if (kbState.IsKeyDown(Keys.A))
								{
									player.State = PlayerStates.WalkLeft;           //Keeps walking left
								}
								if (kbState.IsKeyUp(Keys.A))
								{
									player.State = PlayerStates.FaceLeft;           //Changes to left facing state
								}
								break;

							//Case for walking up
							case PlayerStates.WalkUp:
								if (kbState.IsKeyDown(Keys.W))
								{
									player.State = PlayerStates.WalkUp;             //Keeps walking up
								}
								if (kbState.IsKeyUp(Keys.W))
								{
									player.State = PlayerStates.FaceUp;             //Changes to facing up
								}
								break;

							// Collision cases are similar to standing cases, but the 
							//		player cannot walk in direction of the boundary.
							// Case for downwards border collision
							case (PlayerStates.BorderCollisionDown):
								{
									if (kbState.IsKeyDown(Keys.W))
									{
										player.State = PlayerStates.FaceUp;
									}

									if (kbState.IsKeyDown(Keys.A))
									{
										player.State = PlayerStates.FaceLeft;
									}

									if (kbState.IsKeyDown(Keys.D))
									{
										player.State = PlayerStates.FaceRight;
									}

									break;
								}

							// Case for upwards border collision
							case (PlayerStates.BorderCollisionUp):
								{
									if (kbState.IsKeyDown(Keys.S))
									{
										player.State = PlayerStates.FaceDown;
									}

									if (kbState.IsKeyDown(Keys.A))
									{
										player.State = PlayerStates.FaceLeft;
									}

									if (kbState.IsKeyDown(Keys.D))
									{
										player.State = PlayerStates.FaceRight;
									}

									break;
								}

							// Case for left border collision
							case (PlayerStates.BorderCollisionLeft):
								{
									if (kbState.IsKeyDown(Keys.W))
									{
										player.State = PlayerStates.FaceUp;
									}

									if (kbState.IsKeyDown(Keys.S))
									{
										player.State = PlayerStates.FaceDown;
									}

									if (kbState.IsKeyDown(Keys.D))
									{
										player.State = PlayerStates.FaceRight;
									}

									break;
								}

							// Case for right border collision
							case (PlayerStates.BorderCollisionRight):
								{
									if (kbState.IsKeyDown(Keys.W))
									{
										player.State = PlayerStates.FaceUp;
									}

									if (kbState.IsKeyDown(Keys.A))
									{
										player.State = PlayerStates.FaceLeft;
									}

									if (kbState.IsKeyDown(Keys.S))
									{
										player.State = PlayerStates.FaceDown;
									}

									break;
								}

							// Case for downwards wall collision
							case (PlayerStates.WallCollisionDown):
								{
									if (kbState.IsKeyDown(Keys.W))
									{
										player.State = PlayerStates.FaceUp;
									}

									if (kbState.IsKeyDown(Keys.A))
									{
										player.State = PlayerStates.FaceLeft;
									}

									if (kbState.IsKeyDown(Keys.D))
									{
										player.State = PlayerStates.FaceRight;
									}

									break;
								}

							// Case for upwards wall collision
							case (PlayerStates.WallCollisionUp):
								{
									if (kbState.IsKeyDown(Keys.S))
									{
										player.State = PlayerStates.FaceDown;
									}

									if (kbState.IsKeyDown(Keys.A))
									{
										player.State = PlayerStates.FaceLeft;
									}

									if (kbState.IsKeyDown(Keys.D))
									{
										player.State = PlayerStates.FaceRight;
									}

									break;
								}

							// Case for left wall collision
							case (PlayerStates.WallCollisionLeft):
								{
									if (kbState.IsKeyDown(Keys.W))
									{
										player.State = PlayerStates.FaceUp;
									}

									if (kbState.IsKeyDown(Keys.S))
									{
										player.State = PlayerStates.FaceDown;
									}

									if (kbState.IsKeyDown(Keys.D))
									{
										player.State = PlayerStates.FaceRight;
									}

									break;
								}

							// Case for right wall collision
							case (PlayerStates.WallCollisionRight):
								{
									if (kbState.IsKeyDown(Keys.W))
									{
										player.State = PlayerStates.FaceUp;
									}

									if (kbState.IsKeyDown(Keys.A))
									{
										player.State = PlayerStates.FaceLeft;
									}

									if (kbState.IsKeyDown(Keys.S))
									{
										player.State = PlayerStates.FaceDown;
									}

									break;
								}
						}
						break;
					}
			}

			
			#endregion

			#region Collision FSM
			//Logic for determining if hitting a wall in the list of walls
			bool WallCollided = false;

			// Refresh this variable each time so previous frames do not 
			//		interfere with the current.
			professorInteraction = false;


			//Switching on player.States to check for movement of walking and 
			//		collisions
			switch (player.State)
            {
                //Walking down = moving in the positive direction of the y-axis.
                case (PlayerStates.WalkDown):
                    {
                        //Checks for player collision with window boundary
                        if (player.Y >= windowHeight - player.PlayerHeight)
                        {
                            player.State = PlayerStates.BorderCollisionDown;
                        }
                        //Checks for player collision with wall boundaries and professor tiles
                        else
                        {
                            foreach (Rectangle wall in wallBoundaries)
                            {
                                //If player is intersecting with wall, set to wall collision state so player can't move
                                if (playerTracker.Intersects(wall))
                                {
                                    player.State = PlayerStates.WallCollisionDown;
                                    player.Y = wall.Y - player.PlayerHeight - BounceFactor; //Bounces player out of the wall collision
                                    WallCollided = true;
                                    break;
                                }
                            }

                            //if not colliding then player can walk
                            if (WallCollided == false)
                            {
                                player.Y += PlayerSpeed;
                            }
                        }
                        break;
                    }
                //Walking up = moving in the negative direction of the y-axis.
                case (PlayerStates.WalkUp):
                    {
                        //Checks for player collision with wall boundaries and professor tiles
                        if (player.Y <= 0)
                        {
                            player.State = PlayerStates.BorderCollisionUp;
                        }
                        //Checks for player collision with window boundary
                        else
                        {
                            foreach (Rectangle wall in wallBoundaries)
                            {
                                //If player is intersecting with wall, set to wall collision state so player can't move
                                if (playerTracker.Intersects(wall))
                                {
                                    player.State = PlayerStates.WallCollisionUp;
									player.Y = wall.Y + wall.Height + BounceFactor; //Bounces player out of the wall collision
                                    WallCollided = true;
                                    break;
                                }
                            }

							//if not colliding then player can walk
							if (WallCollided == false)
                            {
                                player.Y -= PlayerSpeed;
                            }
                        }
                        break;
                    }
                //Positive X integer for walking right
                case (PlayerStates.WalkRight):
                    {
                        //Checks for player collision with window boundary
                        if (player.X >= windowWidth - player.PlayerWidth)
                        {
                            player.State = PlayerStates.BorderCollisionRight;
                        }
                        //Checks for player collision with wall boundaries and professor tiles
                        else
                        {
                            foreach (Rectangle wall in wallBoundaries)
                            {
                                //If player is intersecting with wall, set to wall collision state so player can't move
                                if (playerTracker.Intersects(wall))
                                {
                                    player.State = PlayerStates.WallCollisionRight;
                                    player.X = wall.X - player.PlayerWidth - BounceFactor; //Bounces the player out of the collision
                                    WallCollided = true;

                                }
                            }

							if (WallCollided == false)
                            {
                                player.X += PlayerSpeed;
                            }
                        }
                        break;
                    }
                //Negative X integer for walking left
                case (PlayerStates.WalkLeft):
                    {
                        //Checks for player collision with window boundary
                        if (player.X <= 0)
                        {
                            player.State = PlayerStates.BorderCollisionLeft;
                        }
                        //Checks for player collision with wall boundaries and professor tiles
                        else
                        {
                            foreach (Rectangle wall in wallBoundaries)
                            {
                                //If player is intersecting with wall, set to wall collision state so player can't move
                                if (playerTracker.Intersects(wall))
                                {
                                    player.State = PlayerStates.WallCollisionLeft;
                                    player.X = wall.X + wall.Width + BounceFactor; //Bounces player out of collision
                                    WallCollided = true;
                                }
                            }

							if (WallCollided == false)
                            {
                                player.X -= PlayerSpeed;
                            }
                        }
                        break;
                    }
            }
            // Rectangle to track player's current position
            playerTracker = new Rectangle((int)player.X, (int)player.Y, player.PlayerWidth, player.PlayerHeight);
			#endregion

			#region (Not working) Room FSM
			//switch (currentRoom)
			//{
			//	case (Rooms.Hallway):
			//		{
			//			if (hallwayExit.TriggerExit(playerTracker))
			//			{
			//				currentRoom = Rooms.Office;
			//			}
			//			break;
			//		}
			//	case (Rooms.Office):
			//		{
			//			if (officeExit.TriggerExit(playerTracker))
			//			{
			//				currentRoom = Rooms.Hallway;
			//			}
			//			break;
			//		}
			//}
			#endregion

			// This loops and checks to see if the player Rectangle contains a
			//		professor Rectangle. If it does, the corresponding professor
			//		is set as "active" and the Draw portion of the code will
			//		draw the corresponding dialogue box to the screen.
			for (int i = 0; i < professorTileRectangles.Count; i++)
			{
				// If player Rectangle contains a professor tile...
				if (playerTracker.Contains(professorTileRectangles[i].Location))
				{
					// Set professorInteraction to true to acknowledge the interaction.
					// This triggers Draw() to draw the dialogue to the screen.
					professorInteraction = true;

					// Because the indices of entries in professorTileRectangles
					//		correspond to those in enum Professors, parse the respective
					//		Professor's name from the index using Enum.TryParse.
					activeProfessor = (Professors)Enum.Parse(typeof(Professors), i.ToString());
				}
			}

			// Keyboard state at the end of this frame becomes the previous keyboard state for the next.
			prevKBState = kbState;

			base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

			#region (Not working) Draw portion for Room FSM
			//switch (currentRoom)
			//{
			//	case (Rooms.Hallway):
			//		{
			//			drawFrom = 22;
			//			drawTo = mapWidth;

			//			break;
			//		}
			//	case (Rooms.Office):
			//		{
			//			drawFrom = 0;
			//			drawTo = 21;

			//			break;
			//		}
			//}
			#endregion

			#region Draw portion for Game States FSM
			switch (gameState)
			{
				case (GameStates.StartScreen):
					{
						spriteBatch.Draw(
							startScreen, 
							new Rectangle(0, 0, windowWidth, windowHeight), 
							Color.White);
						break;
					}
				case (GameStates.SchwartzIntroScreen):
					{
						spriteBatch.Draw(
							introScreen,
							new Rectangle(0, 0, windowWidth, windowHeight),
							Color.White);
						break;
					}
				case (GameStates.Playing):
					{
						foreach (Map tile in worldMap)
						{
							tile.Draw(spriteBatch);
						}

						// Labels to distinguish professor tiles from each other
						spriteBatch.DrawString(
							arial20, 
							"Erika", 
							new Vector2(
								professorTileRectangles[(int)Professors.Erika].X - 5, 
								professorTileRectangles[(int)Professors.Erika].Y + 40), 
							Color.Black);

						spriteBatch.DrawString(
							arial20,
							"Erin",
							new Vector2(
								professorTileRectangles[(int)Professors.Erin].X - 50,
								professorTileRectangles[(int)Professors.Erin].Y + 10),
							Color.Black);

						spriteBatch.DrawString(
							arial20,
							"Schwartz",
							new Vector2(
								professorTileRectangles[(int)Professors.Schwartz].X - 30,
								professorTileRectangles[(int)Professors.Schwartz].Y - 20),
							Color.Black);

						spriteBatch.DrawString(
							arial20,
							"Luis",
							new Vector2(
								professorTileRectangles[(int)Professors.Luis].X - 3,
								professorTileRectangles[(int)Professors.Luis].Y - 20),
							Color.Black);

						player.Draw(spriteBatch);

						break;
					}
				case (GameStates.FinalSelectionScreen1):
					{
						spriteBatch.Draw(
							finalSelectionScreen1,
							new Rectangle(0, 0, windowWidth, windowHeight),
							Color.White);
						break;
					}
				case (GameStates.FinalSelectionScreen2):
					{
						spriteBatch.Draw(
							finalSelectionScreen2,
							new Rectangle(0, 0, windowWidth, windowHeight),
							Color.White);
						activeProfessor = Professors.None;
						break;
					}
				case (GameStates.WinScreen):
					{
						spriteBatch.Draw(
							winScreen,
							new Rectangle(0, 0, windowWidth, windowHeight),
							Color.White);
						activeProfessor = Professors.None;
						break;
					}
				case (GameStates.LoseScreen):
					{
						spriteBatch.Draw(
							loseScreen,
							new Rectangle(0, 0, windowWidth, windowHeight),
							Color.White);
						activeProfessor = Professors.None;
						break;
					}
			}
			#endregion
			// When the player interacts with a professor...
			switch (professorInteraction)
			{
				case (true):
					{
						// ...draw the corresponding professor's dialogue box
						//			to the screen.
						switch (activeProfessor)
						{
							// Erika's dialogue + snailshot
							case Professors.Erika:
								spriteBatch.Draw(erikaDialogue,
									new Rectangle(windowHeight / 7, 0, windowWidth / 3, windowHeight / 7),
									Color.White);
								spriteBatch.Draw(erikaSnailshot,
									new Rectangle(0, 0, windowHeight / 7, windowHeight / 7),
									Color.White);
								break;

							// Final selection screen triggered
							case Professors.Schwartz:
								gameState = GameStates.FinalSelectionScreen1;
								break;

							// Erin's dialogue + snailshot
							case Professors.Erin:
								spriteBatch.Draw(
									erinDialogue,
									new Rectangle(windowHeight / 7, 0, windowWidth / 3, windowHeight / 7),
									Color.White);
								spriteBatch.Draw(erinSnailshot,
									new Rectangle(0, 0, windowHeight / 7, windowHeight / 7),
									Color.White);
								break;

							// Luis's Dialogue + snailshot
							case Professors.Luis:
								spriteBatch.Draw(
									luisDialogue,
									new Rectangle(windowHeight / 7, 0, windowWidth / 3, windowHeight / 7),
									Color.White);
								spriteBatch.Draw(luisSnailshot,
									new Rectangle(0, 0, windowHeight / 7, windowHeight / 7),
									Color.White);
								break;

							// Do nothing; the only time this will be possible 
							//		if the player is in the final selection 
							//		screen, where they are not allowed to return
							//		to the game anymore.
							case Professors.None:
								break;
						}
						break;
					}
			}

			//spriteBatch.Draw(woodenSquare, woodenSquareRectangle, Color.White);
			//wallTile.DrawWall(spriteBatch);
			//floorTile.DrawFloor(spriteBatch);
			//professorTile.DrawProfessor(spriteBatch);

			//for (int i = 0; i < mapHeight; i++)
			//{
			//	for (int j = drawFrom; j < drawTo; j++)
			//	{
			//		Console.WriteLine(j + i * 44);
			//		worldMap[j + i * 44].Draw(spriteBatch);
			//	}
			//}

            spriteBatch.End();
            base.Draw(gameTime);
        }
		#endregion
	}
}
