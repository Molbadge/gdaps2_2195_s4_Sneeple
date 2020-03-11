using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        const int playerSpeed = 2;

		// Constant to hold player speed when stationary (0)
		const int playerStationarySpeed = 0;

        //Map to draw based on state
        Map map;

        // Variables to store screen size
        int windowWidth;
        int windowHeight;

        // Variables to store wooden square texture/dimensions
        Texture2D woodenSquare;
        Rectangle woodenSquareRectangle;

		// Variable to track player position
		Vector2 playerVelocity;

        Rectangle playerTracker;

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

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Vector2 playerLoc = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            Vector2 tileLoc = new Vector2(10, 10);

            Texture2D spriteSheet = Content.Load<Texture2D>("Ritchie");     //Spritesheet for ritchie
            Texture2D tileSheet = Content.Load<Texture2D>("Dungeon_Crawler_Sheet"); //Spritesheet for map

            player = new Player(spriteSheet, playerLoc, PlayerStates.FaceDown);
            map = new Map(tileSheet, tileLoc, TileStates.Wall);

            woodenSquare = Content.Load<Texture2D>("woodenSquare");
            woodenSquareRectangle = new Rectangle(windowWidth / 2 - 80, windowHeight / 2 - 30, 70, 70);

			playerVelocity = new Vector2(player.X, player.Y);
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
                    if(kbState.IsKeyDown(Keys.W))
                    {
                        player.State = PlayerStates.FaceUp;
                    }
                    if(kbState.IsKeyDown(Keys.A))
                    {
                        player.State = PlayerStates.FaceLeft;
                    }
                    if(kbState.IsKeyDown(Keys.D))
                    {
                        player.State = PlayerStates.FaceRight;
                    }
                    //-----Transition to walking state---------
                    if(kbState.IsKeyDown(Keys.S))
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
                    if(kbState.IsKeyDown(Keys.A))
                    {
                        player.State = PlayerStates.FaceLeft;
                    }
                    if(kbState.IsKeyDown(Keys.S))
                    {
                        player.State = PlayerStates.FaceDown;
                    }
                    //-----Transition to walking state---------
                    if(kbState.IsKeyDown(Keys.D))
                    {
                        player.State = PlayerStates.WalkRight;
                    }
                    break;

                //Case for facing Left
                case PlayerStates.FaceLeft:
                    //-----Transition to standing states--------
                    if(kbState.IsKeyDown(Keys.W))
                    {
                        player.State = PlayerStates.FaceUp;
                    }
                    if(kbState.IsKeyDown(Keys.S))
                    {
                        player.State = PlayerStates.FaceDown;
                    }
                    if(kbState.IsKeyDown(Keys.D))
                    {
                        player.State = PlayerStates.FaceRight;
                    }
                    //-----Transition to walking state---------
                    if(kbState.IsKeyDown(Keys.A))
                    {
                        player.State = PlayerStates.WalkLeft;
                    }
                    break;

                //Case for Facing Up
                case PlayerStates.FaceUp:
                    //-----Transition to standing states--------
                    if(kbState.IsKeyDown(Keys.A))
                    {
                        player.State = PlayerStates.FaceLeft;
                    }
                    if(kbState.IsKeyDown(Keys.S))
                    {
                        player.State = PlayerStates.FaceDown;
                    }
                    if(kbState.IsKeyDown(Keys.D))
                    {
                        player.State = PlayerStates.FaceRight;
                    }
                    //-----Transition to walking state---------
                    if(kbState.IsKeyDown(Keys.W))
                    {
                        player.State = PlayerStates.WalkUp;
                    }
                    break;


                //Case for walking Down
                case PlayerStates.WalkDown:
                    if(kbState.IsKeyDown(Keys.S))
                    {
                        player.State = PlayerStates.WalkDown;        //Keeps Walking down
                    }
                    if(kbState.IsKeyUp(Keys.S))
                    {
                        player.State = PlayerStates.FaceDown;        //Changes to facing down state
                    }
                    break;

                //Case for walking right
                case PlayerStates.WalkRight:
                    if(kbState.IsKeyDown(Keys.D))
                    {
                        player.State = PlayerStates.WalkRight;          //Keeps walking right
                    }
                    if(kbState.IsKeyUp(Keys.D))
                    {
                        player.State = PlayerStates.FaceRight;          //Changes to facing right state
                    }
                    break;

                //Case for walking left
                case PlayerStates.WalkLeft:
                    if(kbState.IsKeyDown(Keys.A))
                    {
                        player.State = PlayerStates.WalkLeft;           //Keeps walking left
                    }
                    if(kbState.IsKeyUp(Keys.A))
                    {
                        player.State = PlayerStates.FaceLeft;           //Changes to left facing state
                    }
                    break;

                //Case for walking up
                case PlayerStates.WalkUp:
                    if(kbState.IsKeyDown(Keys.W))
                    {
                        player.State = PlayerStates.WalkUp;             //Keeps walking up
                    }
                    if(kbState.IsKeyUp(Keys.W))
                    {
                        player.State = PlayerStates.FaceUp;             //Changes to facing up
                    }
                    break;


				// Collision cases are similar to standing cases, but the 
				//		player cannot walk in direction of the boundary.
				// Case for downwards collision
				case (PlayerStates.CollisionDown):
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

				// Case for upwards collision
				case (PlayerStates.CollisionUp):
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

				// Case for left collision
				case (PlayerStates.CollisionLeft):
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

				// Case for right collision
				case (PlayerStates.CollisionRight):
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
							player.State = PlayerStates.CollisionDown;
							Console.WriteLine("WALKDOWN COLLISION");
						}

						if (playerTracker.Intersects(woodenSquareRectangle))
						{
							player.State = PlayerStates.CollisionDown;
							Console.WriteLine("WALKDOWN COLLISION");
						}

						playerVelocity.Y = playerSpeed;
						player.Y += playerVelocity.Y;

						break;
					}
				//Walking up = moving in the negative direction of the y-axis.
				case (PlayerStates.WalkUp):
					{
						// Checking if player is touching the edges of the screen or 
						//     wooden square.
						if (player.Y <= 0)
						{
							player.State = PlayerStates.CollisionUp;
							Console.WriteLine("WALKUP COLLISION");
						}

						if (playerTracker.Intersects(woodenSquareRectangle))
						{
							player.State = PlayerStates.CollisionUp;
							Console.WriteLine("WALKUP COLLISION");
						}

						playerVelocity.Y = playerSpeed;
						player.Y -= playerVelocity.Y;

						break;
					}
				//Positive X integer for walking right
				case (PlayerStates.WalkRight):
					{
						// Checking if player is touching the edges of the screen
						if (player.X >= windowWidth - player.PlayerWidth)
						{
							player.State = PlayerStates.CollisionRight;
							Console.WriteLine("WALKRIGHT COLLISION");
						}

						if (playerTracker.Intersects(woodenSquareRectangle))
						{
							player.State = PlayerStates.CollisionRight;
							Console.WriteLine("WALKRIGHT COLLISION");
						}

						playerVelocity.X = playerSpeed;
						player.X += playerVelocity.X;

						break;
					}
				//Negative X integer for walking left
				case (PlayerStates.WalkLeft):
					{
						// Checking if player is touching the edges of the screen
						// or wooden square
						if (player.X <= 0)
						{
							player.State = PlayerStates.CollisionLeft;
							Console.WriteLine("WALKLEFT COLLISION");
						}

						if (playerTracker.Intersects(woodenSquareRectangle))
						{
							player.State = PlayerStates.CollisionLeft;
							Console.WriteLine("WALKLEFT COLLISION");
						}

						playerVelocity.X = playerSpeed;
						player.X -= playerVelocity.X;
						break;
					}

					// Maybe implement a fall-through case here, since scenarios 
					//		are very similar
				case (PlayerStates.CollisionDown):
					{
						playerVelocity.Y = playerStationarySpeed;
						player.Y -= 2;
						player.Y -= playerStationarySpeed;
						player.State = PlayerStates.FaceDown;
						break;
					}

				case (PlayerStates.CollisionUp):
					{
						playerVelocity.Y = playerStationarySpeed;
						player.Y += 2;
						player.Y += playerStationarySpeed;
						player.State = PlayerStates.FaceUp;
						break;
					}

				case (PlayerStates.CollisionLeft):
					{
						playerVelocity.X = playerStationarySpeed;
						player.X += 2;
						player.X += playerStationarySpeed;
						player.State = PlayerStates.FaceLeft;
						break;
					}

				case (PlayerStates.CollisionRight):
					{
						playerVelocity.X = playerStationarySpeed;
						player.X -= 2;
						player.X -= playerStationarySpeed;
						player.State = PlayerStates.FaceRight;
						break;
					}
			}

            //if (playerTracker.Intersects(woodenSquareRectangle))
            //{
            //    player.X += 0;
            //    player.Y += 0;
            //}

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

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            player.Draw(spriteBatch);
            spriteBatch.Draw(woodenSquare, woodenSquareRectangle, Color.White);
            map.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
