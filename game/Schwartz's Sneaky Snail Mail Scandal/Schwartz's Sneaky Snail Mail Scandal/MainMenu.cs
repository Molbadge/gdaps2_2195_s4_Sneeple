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
    // States are either waiting for player or launching game
    enum MenuStates
    {
        Waiting,
        Launching
    }

    class MainMenu
    {
        // Fields
        // texture for the main menu image
        Texture2D menuScreen;

        // Constructor
        public MainMenu(Texture2D menuScreen)
        {
            this.menuScreen = menuScreen;
        }

        // Methods
        public void Draw(Texture2D mainMenu)
        {
            
        }

        private void DrawMenu(Texture2D mainMenu)
        {

        }


    }
}
