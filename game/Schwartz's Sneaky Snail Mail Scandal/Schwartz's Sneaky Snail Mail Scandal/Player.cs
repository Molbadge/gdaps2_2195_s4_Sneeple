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
        FaceUp,
        FaceDown,
        FaceRight,
        FaceLeft,
        WalkUp,
        WalkDown,
        WalkLeft,
        WalkRight
    }

    class Player
    {
        // Fields

        Vector2 playerLoc;  // tracks player's location
        Texture2D spriteSheet;
        PlayerStates state;

        // Constants for the sprites

        const int WalkFrameCount = 5;
        const int PlayerRectOffsetY = 0;
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

            fps = 0.0;
            timePerFrame = 1.0 / fps;
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
                    frame = 1;                  //Back to 1 for the frames

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
                        DrawFaceDown(SpriteEffects.None, spriteBatch);
                        break;
                    }
                case (PlayerStates.FaceLeft):
                    {
                        DrawFaceLeft(SpriteEffects.None, spriteBatch);
                        break;
                    }
                case (PlayerStates.FaceRight):
                    {
                        DrawFaceRight(SpriteEffects.None, spriteBatch);
                        break;
                    }
                case (PlayerStates.FaceUp):
                    {
                        DrawFaceUp(SpriteEffects.None, spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkDown):
                    {
                        DrawWalkingDown(SpriteEffects.None, spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkLeft):
                    {
                        DrawWalkingLeft(SpriteEffects.None, spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkRight):
                    {
                        DrawWalkingRight(SpriteEffects.None, spriteBatch);
                        break;
                    }
                case (PlayerStates.WalkUp):
                    {
                        DrawWalkingUp(SpriteEffects.None, spriteBatch);
                        break;
                    }
            }


        }


        //Methods for drawing the standing frames
        private void DrawFaceDown(SpriteEffects flipsprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - Texture to draw
                playerLoc,                      // - Location to draw to
                new Rectangle(                  // - Source Rectangle
                    0,                          //   - Specifies where in the
                    PlayerRectOffsetY,          //      sprite image to pull
                    PlayerRectWidth,            //      the drawing from
                    PlayerRectHeight),          //
                Color.White,                    // - Color
                0,                              // - Rotation (Should be none)
                Vector2.Zero,                   // - Origin inside the image (top left of image)
                1.0f,                           // - Scale (100% no change right now)
                flipsprite,                     // - Used to flip image if needed
                0);                             // - Layer depth will implement later
        }

        private void DrawFaceRight(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                spriteSheet,                    // - Texture to draw
                playerLoc,                      // - Location to draw to
                new Rectangle(                  // - Source Rectangle
                    0,                          //   - Specifies where in the
                    PlayerRectOffsetY,          //      sprite image to pull
                    PlayerRectWidth,            //      the drawing from
                    2 * PlayerRectHeight),          //
                Color.White,                    // - Color
                0,                              // - Rotation (Should be none)
                Vector2.Zero,                   // - Origin inside the image (top left of image)
                1.0f,                           // - Scale (100% no change right now)
                flipSprite,                     // - Used to flip image if needed
                0);                             // - Layer depth will implement later
        }

        private void DrawFaceLeft(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                    // - Texture to draw
               playerLoc,                      // - Location to draw to
               new Rectangle(                  // - Source Rectangle
                   0,                          //   - Specifies where in the
                   PlayerRectOffsetY,          //      sprite image to pull
                   PlayerRectWidth,            //      the drawing from
                   3 * PlayerRectHeight),          //
               Color.White,                    // - Color
               0,                              // - Rotation (Should be none)
               Vector2.Zero,                   // - Origin inside the image (top left of image)
               1.0f,                           // - Scale (100% no change right now)
               flipSprite,                     // - Used to flip image if needed
               0);                             // - Layer depth will implement later
        }

        private void DrawFaceUp(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                    // - Texture to draw
               playerLoc,                      // - Location to draw to
               new Rectangle(                  // - Source Rectangle
                   0,                          //   - Specifies where in the
                   PlayerRectOffsetY,          //      sprite image to pull
                   PlayerRectWidth,            //      the drawing from
                   4 * PlayerRectHeight),          //
               Color.White,                    // - Color
               0,                              // - Rotation (Should be none)
               Vector2.Zero,                   // - Origin inside the image (top left of image)
               1.0f,                           // - Scale (100% no change right now)
               flipSprite,                     // - Used to flip image if needed
               0);                             // - Layer depth will implement later
        }

        private void DrawWalkingDown(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                    // - Texture to draw
               playerLoc,                      // - Location to draw to
               new Rectangle(                  // - Source Rectangle
                   frame * PlayerRectWidth,    //   - Specifies where in the
                   PlayerRectOffsetY,          //      sprite image to pull
                   PlayerRectWidth,            //      the drawing from
                   PlayerRectHeight),          //
               Color.White,                    // - Color
               0,                              // - Rotation (Should be none)
               Vector2.Zero,                   // - Origin inside the image (top left of image)
               1.0f,                           // - Scale (100% no change right now)
               flipSprite,                     // - Used to flip image if needed
               0);                             // - Layer depth will implement later
        }

        private void DrawWalkingRight(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                    // - Texture to draw
               playerLoc,                      // - Location to draw to
               new Rectangle(                  // - Source Rectangle
                   frame * PlayerRectWidth,    //   - Specifies where in the
                   PlayerRectOffsetY,          //      sprite image to pull
                   PlayerRectWidth,            //      the drawing from
                   2 * PlayerRectHeight),          //
               Color.White,                    // - Color
               0,                              // - Rotation (Should be none)
               Vector2.Zero,                   // - Origin inside the image (top left of image)
               1.0f,                           // - Scale (100% no change right now)
               flipSprite,                     // - Used to flip image if needed
               0);                             // - Layer depth will implement later
        }

        private void DrawWalkingLeft(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                    // - Texture to draw
               playerLoc,                      // - Location to draw to
               new Rectangle(                  // - Source Rectangle
                   frame * PlayerRectWidth,    //   - Specifies where in the
                   PlayerRectOffsetY,          //      sprite image to pull
                   PlayerRectWidth,            //      the drawing from
                   3 * PlayerRectHeight),          //
               Color.White,                    // - Color
               0,                              // - Rotation (Should be none)
               Vector2.Zero,                   // - Origin inside the image (top left of image)
               1.0f,                           // - Scale (100% no change right now)
               flipSprite,                     // - Used to flip image if needed
               0);                             // - Layer depth will implement later
        }

        private void DrawWalkingUp(SpriteEffects flipSprite, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               spriteSheet,                    // - Texture to draw
               playerLoc,                      // - Location to draw to
               new Rectangle(                  // - Source Rectangle
                   frame * PlayerRectWidth,    //   - Specifies where in the
                   PlayerRectOffsetY,          //      sprite image to pull
                   PlayerRectWidth,            //      the drawing from
                   4 * PlayerRectHeight),          //
               Color.White,                    // - Color
               0,                              // - Rotation (Should be none)
               Vector2.Zero,                   // - Origin inside the image (top left of image)
               1.0f,                           // - Scale (100% no change right now)
               flipSprite,                     // - Used to flip image if needed
               0);                             // - Layer depth will implement later
        }
    }
}
