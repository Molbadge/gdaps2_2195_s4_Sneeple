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
    enum PlayerStates
    {        
        FaceDown = 0,
        FaceRight = 1,
        FaceLeft = 2,
        FaceUp = 3,
        
        WalkDown = 4,
        WalkRight = 5,
        WalkLeft = 6,
        WalkUp = 7,

		WallCollisionDown = 8,
		WallCollisionRight = 9,
		WallCollisionLeft = 10,
		WallCollisionUp = 11,
	}

    class Player
    {
        // Fields

        private Vector2 playerLoc;  // tracks player's location
        private Texture2D spriteSheet;
        private PlayerStates state;

        // Constants for the sprites
        const int WalkFrameCount = 3;
        const int PlayerRectHeight = 200;
        const int PlayerRectWidth = 125;

        // Animation
        int frame;
        double timeCounter;
        double fps;
        double timePerFrame;

        // Properties
        public float X
        {
            get { return this.playerLoc.X; }
            set { this.playerLoc.X = value; }
        }

        public float Y
        {
            get { return this.playerLoc.Y; }
            set { this.playerLoc.Y = value; }
        }

        public PlayerStates State
        {
            get { return state; }
            set { state = value; }
        }

        // get properties to return player rectangle height/width constants
        public int PlayerWidth
        {
            get { return PlayerRectWidth; }
        }

        public int PlayerHeight
        {
            get { return PlayerRectHeight; }
        }

        // Constructor
        public Player(Texture2D spriteSheet, Vector2 playerLoc, PlayerStates startingState)
        {
            this.spriteSheet = spriteSheet;
            this.playerLoc = playerLoc;
            this.state = startingState;

            fps = 4.0;
            timePerFrame = 1.0 / fps;
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            //Handle the animation timing and cycling
            //Adds to the time counter to check if enough time has passed

            //Time passing
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            //If enough time has passed for each cycle
            if(timeCounter >= timePerFrame)
            {
                frame += 1;                     //Increments the frame images

                if (frame > WalkFrameCount)     //Checking the bounds of the walk cycle
                    frame = 0;                  //Back to 1 for the frames

                timeCounter -= timePerFrame;    //removes time used, helps keep time passing
            }

        }

        //Method for drawing of each state
        public void Draw(SpriteBatch spriteBatch)
        {
            // FSM for player animation goes here
            switch (state)
            {
                case (PlayerStates.FaceDown):
                    {
                        DrawStanding(spriteBatch);
                        break;
                    }
                case (PlayerStates.FaceLeft):
                    {
                        DrawStanding(spriteBatch);
                        break;
                    }
                case (PlayerStates.FaceRight):
                    {
                        DrawStanding(spriteBatch);
                        break;
                    }
                case (PlayerStates.FaceUp):
                    {
                        DrawStanding(spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkDown):
                    {
                        DrawWalking(spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkLeft):
                    {
                        DrawWalking(spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkRight):
                    {
                        DrawWalking(spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkUp):
                    {
                        DrawWalking(spriteBatch);
                        break;
                    }
				case (PlayerStates.WallCollisionDown):
					{
						DrawStanding(spriteBatch);
						break;
					}
				case (PlayerStates.WallCollisionUp):
					{
						DrawStanding(spriteBatch);
						break;
					}
				case (PlayerStates.WallCollisionLeft):
					{
						DrawStanding(spriteBatch);
						break;
					}
				case (PlayerStates.WallCollisionRight):
					{
						DrawStanding(spriteBatch);
						break;
					}
			}
        }
        // Methods for drawing the standing frames
        //Draw Standing is used for all the standing states
        private void DrawStanding(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
                playerLoc,
                new Rectangle(
                    0,
                    ((int)state % 4 * PlayerRectHeight),
                    PlayerRectWidth,
                    PlayerRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0);
        }

        //Draw Walking is used for all the walking states
        private void DrawWalking(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                                     // - Texture to draw
               playerLoc,                                       // - Location to draw to
               new Rectangle(                                   // - Source Rectangle
                   frame * PlayerRectWidth,                     //   - Specifies where in the
                   ((int)state % 4) * PlayerRectHeight,         //      sprite image to pull
                   PlayerRectWidth,                             //      the drawing from
                   PlayerRectHeight),                           //
               Color.White,                                     // - Color
               0,                                               // - Rotation (Should be none)
               Vector2.Zero,                                    // - Origin inside the image (top left of image)
               1.0f,                                            // - Scale (100% no change right now)
               SpriteEffects.None,                              // - Used to flip image if needed
               0);                                              // - Layer depth will implement later
        }

        
    }
}
