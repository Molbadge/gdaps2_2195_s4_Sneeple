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
        WalkLeft = 5,
        WalkRight = 6,
        WalkUp = 7
    }

    class Player
    {
        // Fields

        Vector2 playerLoc;  // tracks player's location
        Texture2D spriteSheet;
        PlayerStates state;

        // Constants for the sprites

        const int WalkFrameCount = 5;
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

        public Player(Texture2D spriteSheet, Vector2 playerLoc, PlayerStates startingState)
        {
            this.spriteSheet = spriteSheet;
            this.playerLoc = playerLoc;
            this.state = startingState;

            fps = 5.0;
            timePerFrame = .5 / fps;
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            //Handle the animation timing and cycling
            //Adds to the time counter to check if enough time ha passed

            //Time passing
            timeCounter += gameTime.ElapsedGameTime.TotalSeconds;

            //If enough time has passed for each cycle
            if(timeCounter >= timePerFrame)
            {
                frame += 1;                     //Increments the frame images

                if (frame > WalkFrameCount)     //Checking the bounds of the wlak cycle
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
            }
        }
        // Methods for drawing the standing frames
        // Can someone label these idk which one is which
        private void DrawStanding(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,
                playerLoc,
                new Rectangle(
                    0,
                    (int)state* PlayerRectHeight,
                    PlayerRectWidth,
                    PlayerRectHeight),
                Color.White,
                0,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                0);
        }

        private void DrawWalking( SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                                     // - Texture to draw
               playerLoc,                                       // - Location to draw to
               new Rectangle(                                   // - Source Rectangle
                   frame * PlayerRectWidth,                     //   - Specifies where in the
                   ((int)state - 5) * PlayerRectHeight,         //      sprite image to pull
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
