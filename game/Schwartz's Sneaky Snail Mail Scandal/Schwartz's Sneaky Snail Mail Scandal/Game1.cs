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
        Map wallTile;
        Map floorTile;
        Map professorTile;
        List<Map> worldMap = new List<Map>();


        // Variables to store screen size
        int windowWidth;
        int windowHeight;

        // Variables to store wooden square texture/dimensions
        Texture2D woodenSquare;
        Rectangle woodenSquareRectangle;

        Rectangle playerTracker;

        // File IO variables
        FileStream readStream;
        StreamWriter writer;
        StreamReader reader;

        // List to store all PictureBox objects used to represent map tiles.
        List<TileStates> tileList;
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
                string writeFile = "bob is a donk";
                StreamWriter writer = new StreamWriter(writeFile);

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
                for (int i = 0; i < tileList.Count; i++)
                {
                    tileList[i] = AssignTile(tileTypeList[i]);
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
            tileList = LoadFromFile("../../../../WallTest");

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            Texture2D spriteSheet = Content.Load<Texture2D>("Ritchie");     //Spritesheet for ritchie
            Texture2D tileSheet = Content.Load<Texture2D>("Dungeon_Crawler_Sheet"); //Spritesheet for map

            for (int row = 0; row < mapHeight; row++)
            {
                for (int column = 0; column < mapWidth; column++)
                {
                    int tileIndex = mapWidth * row + column;

                    TileStates tempState = tileList[tileIndex];

                    Vector2 tempVector = new Vector2(row * 32, column * 32);

                    worldMap.Add(new Map(tileSheet, tempVector, tempState));
                }
            }

            // TODO: use this.Content to load your game content here
            Vector2 playerLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            //Vector2 wallLoc = new Vector2(10,72);
            //Vector2 floorLoc = new Vector2(10, 10);
            //Vector2 professorLoc = new Vector2(10, 144);


            player = new Player(spriteSheet, playerLoc, PlayerStates.FaceDown);
            //wallTile = new Map(tileSheet, wallLoc, TileStates.Wall); //Light grey
            //floorTile = new Map(tileSheet, floorLoc, TileStates.Floor); //Dark grey
            //professorTile = new Map(tileSheet, professorLoc, TileStates.Professor); //Ruby Gem

            woodenSquare = Content.Load<Texture2D>("woodenSquare");
            woodenSquareRectangle = new Rectangle(windowWidth / 2 - 80, windowHeight / 2 - 30, 70, 70);


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

            //Switching on player.States to check for movement of walking and 
            //		collisions
            switch (player.State)
            {
                //Walking down = moving in the positive direction of the y-axis.
                case (PlayerStates.WalkDown):
                    {
                        // Checking if player is touching the edges of the screen
                        //		or wooden square.
                        if (player.Y >= windowHeight - player.PlayerHeight)
                        {
                            player.State = PlayerStates.BorderCollisionDown;
                            Console.WriteLine("WALKDOWN BORDER COLLISION");
                        }
                        if (playerTracker.Intersects(woodenSquareRectangle))
                        {
                            player.State = PlayerStates.WallCollisionDown;
                            Console.WriteLine("WALKDOWN WALL COLLISION");
                        }
                        player.Y += PlayerSpeed;
                        break;
                    }
                //Walking up = moving in the negative direction of the y-axis.
                case (PlayerStates.WalkUp):
                    {
                        // Checking if player is touching the edges of the screen or 
                        //     wooden square.
                        if (player.Y <= 0)
                        {
                            player.State = PlayerStates.BorderCollisionUp;
                            Console.WriteLine("WALKUP BORDER COLLISION");
                        }

                        if (playerTracker.Intersects(woodenSquareRectangle))
                        {
                            player.State = PlayerStates.WallCollisionUp;
                            Console.WriteLine("WALKUP WALL COLLISION");
                        }

                        player.Y -= PlayerSpeed;
                        break;
                    }
                //Positive X integer for walking right
                case (PlayerStates.WalkRight):
                    {
                        // Checking if player is touching the edges of the screen
                        if (player.X >= windowWidth - player.PlayerWidth)
                        {
                            player.State = PlayerStates.BorderCollisionRight;
                            Console.WriteLine("WALKRIGHT BORDER COLLISION");
                        }

                        if (playerTracker.Intersects(woodenSquareRectangle))
                        {
                            player.State = PlayerStates.WallCollisionRight;
                            Console.WriteLine("WALKRIGHT WALL COLLISION");
                        }
                        player.X += PlayerSpeed;
                        break;
                    }
                //Negative X integer for walking left
                case (PlayerStates.WalkLeft):
                    {
                        // Checking if player is touching the edges of the screen
                        // or wooden square
                        if (player.X <= 0)
                        {
                            player.State = PlayerStates.BorderCollisionLeft;
                            Console.WriteLine("WALKLEFT BORDER COLLISION");
                        }
                        if (playerTracker.Intersects(woodenSquareRectangle))
                        {
                            player.State = PlayerStates.WallCollisionLeft;
                            Console.WriteLine("WALKLEFT WALL COLLISION");
                        }
                        player.X -= PlayerSpeed;
                        break;
                    }
                case (PlayerStates.BorderCollisionDown):
                    {
                        player.Y += PlayerStationarySpeed;
                        break;
                    }
                case (PlayerStates.BorderCollisionUp):
                    {
                        player.Y -= PlayerStationarySpeed;
                        break;
                    }
                case (PlayerStates.BorderCollisionLeft):
                    {
                        player.X -= PlayerStationarySpeed;
                        break;
                    }
                case (PlayerStates.BorderCollisionRight):
                    {
                        player.X += PlayerStationarySpeed;
                        break;
                    }
                case (PlayerStates.WallCollisionDown):
                    {
                        if (player.Y + player.PlayerHeight > woodenSquareRectangle.Y)
                        {
                            // Readjusting position of player rectangle by  
                            //		"bouncing" them slightly outward, so 
                            //		overlapping no longer occurs
                            player.Y -= BounceFactor;
                        }
                        break;
                    }
                case (PlayerStates.WallCollisionUp):
                    {
                        if (player.Y < woodenSquareRectangle.Y + woodenSquareRectangle.Height)
                        {
                            player.Y += BounceFactor;
                        }
                        break;
                    }

                case (PlayerStates.WallCollisionLeft):
                    {
                        if (player.X < woodenSquareRectangle.X + woodenSquareRectangle.Width)
                        {
                            player.X += BounceFactor;
                        }
                        break;
                    }

                case (PlayerStates.WallCollisionRight):
                    {
                        if (player.X + player.PlayerWidth > woodenSquareRectangle.X)
                        {
                            player.X -= BounceFactor;
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

            player.Draw(spriteBatch);
            spriteBatch.Draw(woodenSquare, woodenSquareRectangle, Color.White);
            //wallTile.DrawWall(spriteBatch);
            //floorTile.DrawFloor(spriteBatch);
            //professorTile.DrawProfessor(spriteBatch);
            foreach (Map tile in worldMap)
            {

                tile.Draw(spriteBatch);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
