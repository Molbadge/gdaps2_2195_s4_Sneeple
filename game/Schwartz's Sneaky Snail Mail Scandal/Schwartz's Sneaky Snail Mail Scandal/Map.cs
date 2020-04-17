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
    enum TileStates // Enum for tiles
    {
        Wall = 0, //Dark grey
        Floor = 1, //Light Gray
        Professor = 2, //Ruby Tile
    }

    class Map
    {
        


        //Fields
        Vector2 tileLoc;
        Texture2D tileSprite;
        bool canWalk = true; //Determining if the player can walk on the tile

        public const int tileWidth = 32;
        public const int tileHeight = 32;

        TileStates tilePlaced;          //variable to hold enum for placing tiles  

        //Constants for tile sprites
        const int TileRectWidth = 32;
        const int TileRectHeight = 32;

        //Properties

        //Get property to return X and Y coordinates of tiles
        public float X
        {
            get { return this.tileLoc.X; }
        }

        public float Y
        {
            get { return this.tileLoc.Y; }
        }

        //Get/Set property to return and set tile state for types of tiles to place
        public TileStates TilePlaced
        {
            get { return tilePlaced; }
            set { tilePlaced = value; }
        }

        //Get properties to return tile width and height constants
        public int TileWidth
        {
            get { return TileRectWidth; }
        }

        public int TileHeight
        {
            get { return TileRectHeight; }
        }

        //Constructor
        public Map(Texture2D spritesheet, Vector2 tileLocation, TileStates startingState)
        {
            this.tileSprite = spritesheet;
            this.tileLoc = tileLocation;
            this.tilePlaced = startingState;
        }

        //Drawing Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (tilePlaced)
            {
                case TileStates.Wall:
                    DrawWall(spriteBatch);
                    break;

                case TileStates.Floor:
                    DrawFloor(spriteBatch);
                    break;

                case TileStates.Professor:
                    DrawProfessor(spriteBatch);
                    break;
            }
        }
        
        public void DrawWall(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               tileSprite,                                     // - Texture to draw
               tileLoc,                                       // - Location to draw to
               new Rectangle(                                   // - Source Rectangle
                   7*TileRectWidth,
                   3*TileRectHeight,
                   TileRectWidth,
                   TileRectHeight),
               Color.White,                                     // - Color
               0,                                               // - Rotation (Should be none)
               Vector2.Zero,                                    // - Origin inside the image (top left of image)
               2.0f,                                            // - Scale (200%)
               SpriteEffects.None,                              // - Used to flip image if needed
               0);                                              // - Layer depth will implement later

            canWalk = false;
        }

        public void DrawFloor(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               tileSprite,                                     // - Texture to draw
               tileLoc,                                       // - Location to draw to
               new Rectangle(                                   // - Source Rectangle
                   6 * TileRectWidth,
                   2 * TileRectHeight,
                   TileRectWidth,
                   TileRectHeight),
               Color.White,                                     // - Color
               0,                                               // - Rotation (Should be none)
               Vector2.Zero,                                    // - Origin inside the image (top left of image)
               2.0f,                                            // - Scale (200%)
               SpriteEffects.None,                              // - Used to flip image if needed
               0);                                              // - Layer depth will implement later

            canWalk = true;
        }

        public void DrawProfessor(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
               tileSprite,                                     // - Texture to draw
               tileLoc,                                       // - Location to draw to
               new Rectangle(                                   // - Source Rectangle
                   2 * TileRectWidth,
                   1 * TileRectHeight,
                   TileRectWidth,
                   TileRectHeight),
               Color.White,                                     // - Color
               0,                                               // - Rotation (Should be none)
               Vector2.Zero,                                    // - Origin inside the image (top left of image)
               2.0f,                                            // - Scale (200%)
               SpriteEffects.None,                              // - Used to flip image if needed
               0);                                              // - Layer depth will implement later

            canWalk = false;

        }
    }
}
