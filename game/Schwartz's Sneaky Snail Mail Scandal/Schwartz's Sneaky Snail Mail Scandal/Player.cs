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
    enum States
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
        States playerState;

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
        public States PlayerState
        {
            get { return playerState; }
            set { playerState = value; }
        }

        public Player(Texture2D spriteSheet, Vector2 playerLoc /*PlayerState startingState*/)
        {
            this.spriteSheet = spriteSheet;
            this.playerLoc = playerLoc;
            //this.playerState = playerState; to be added when PlayerState is working properly

            fps = 10.0;
            timePerFrame = 1.0 / fps;
        }

        public void Update(GameTime gameTime)
        {
            // FSM for player animation goes here
            switch (playerState)
            {
                case (States.FaceDown):
                    {
                        break;
                    }
                case (States.FaceLeft):
                    {
                        break;
                    }
                case (States.FaceRight):
                    {
                        break;
                    }
                case (States.FaceUp):
                    {
                        break;
                    }
                case (States.WalkDown):
                    {
                        break;
                    }
                case (States.WalkLeft):
                    {
                        break;
                    }
                case (States.WalkRight):
                    {
                        break;
                    }
                case (States.WalkUp):
                    {
                        break;
                    }
            }
        }

        public void Draw(GameTime gameTime)
        {
            // FSM for player animation goes here
            switch (playerState)
            {
                case (States.FaceDown):
                    {
                        break;
                    }
                case (States.FaceLeft):
                    {
                        break;
                    }
                case (States.FaceRight):
                    {
                        break;
                    }
                case (States.FaceUp):
                    {
                        break;
                    }
                case (States.WalkDown):
                    {
                        break;
                    }
                case (States.WalkLeft):
                    {
                        break;
                    }
                case (States.WalkRight):
                    {
                        break;
                    }
                case (States.WalkUp):
                    {
                        break;
                    }
            }


        }
    }
}
