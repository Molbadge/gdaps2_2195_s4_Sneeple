﻿using System;
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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();

            windowWidth = graphics.GraphicsDevice.Viewport.Width;
            windowHeight = graphics.GraphicsDevice.Viewport.Height;
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
                    throw new System.ArgumentException(letter + " was not a wall or floor. Please check file input.");
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
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
			// Loading the level from a preset location in the file directory
			tileList = LoadFromFile("../../../../Content/igmHallway");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Texture2D spriteSheet = Content.Load<Texture2D>("Ritchie");     //Spritesheet for ritchie
            Texture2D tileSheet = Content.Load<Texture2D>("Dungeon_Crawler_Sheet"); //Spritesheet for map
            //Adds tiles to a list using file data
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int tileIndex = mapWidth * y + x;

                    TileStates tempState = tileList[tileIndex];

                    Vector2 tempVector = new Vector2(x * 32, y * 32);

                    worldMap.Add(new Map(tileSheet, tempVector, tempState));

                    //Adds walls to a list of rectangles for border collision
                    if (tempState == TileStates.Wall)
                    {
                        wallBoundaries.Add(new Rectangle(x * 32, y * 32, 32, 32));
                    }
                }
            }

            // TODO: use this.Content to load your game content here
            Vector2 playerLoc = new Vector2(50, 50);

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

            //Switch statement for Walking states
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

            //Logic for determining if hitting a wall in the list of walls
            bool WallCollided = false;

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
                        //Checks for player collision with wall boundaries
                        else
                        {
                            foreach (Rectangle wall in wallBoundaries)
                            {
                                //If player is intersecting with wall, set to wall collision state so player can't move
                                if (playerTracker.Intersects(wall))
                                {
                                    player.State = PlayerStates.WallCollisionDown;
                                    player.Y = wall.Y - player.PlayerHeight - BounceFactor;     //Bounces player out of the wall collision
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
                        //Checks for player collision with window boundary
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
                                    player.Y = wall.Y + wall.Height + BounceFactor;          //Bounces player out of the wall collision
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
                        //Checks for player collision with window boundary
                        else
                        {
                            foreach (Rectangle wall in wallBoundaries)
                            {
                                //If player is intersecting with wall, set to wall collision state so player can't move
                                if (playerTracker.Intersects(wall))
                                {
                                    player.State = PlayerStates.WallCollisionRight;
                                    player.X = wall.X - player.PlayerWidth - BounceFactor;          //Bounces the player out of the collision
                                    WallCollided = true;

                                }
                            }
                            //if not colliding then player can walk
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
                        //Checks for player collision with wall boundaries
                        else
                        {
                            foreach (Rectangle wall in wallBoundaries)
                            {
                                //If player is intersecting with wall, set to wall collision state so player can't move
                                if (playerTracker.Intersects(wall))
                                {
                                    player.State = PlayerStates.WallCollisionLeft;
                                    player.X = wall.X + wall.Width + BounceFactor;          //Bounces player out of collision
                                    WallCollided = true;

                                }
                            }
                            //if not colliding then player can walk
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


            //spriteBatch.Draw(woodenSquare, woodenSquareRectangle, Color.White);
            //wallTile.DrawWall(spriteBatch);
            //floorTile.DrawFloor(spriteBatch);
            //professorTile.DrawProfessor(spriteBatch);
            foreach (Map tile in worldMap)
            {
                tile.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
