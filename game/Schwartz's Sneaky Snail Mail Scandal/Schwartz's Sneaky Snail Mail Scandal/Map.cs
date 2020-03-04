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
    enum TileAssets
    {
        wall = 0,
        floor = 1,
    }


    class Map
    {
        //Fields
        Texture2D tileSprite;
        bool canWalk = true; //Determining if the player can walk on the tile

        public const int tileWidth = 32;
        public const int tileHeight = 32;

        TileAssets tilePlaced;

        //Constructor
        public Map(Texture2D spritesheet)
        {
            this.tileSprite = spritesheet;
        }


    }
}
