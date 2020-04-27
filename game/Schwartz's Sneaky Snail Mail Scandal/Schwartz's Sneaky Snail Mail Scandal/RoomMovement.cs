using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Schwartz_s_Sneaky_Snail_Mail_Scandal
{
	class RoomMovement
	{
		private Rectangle detectionRectangle;

		// This class defines rectangles that will be used to determine when to move the player into a different room
		public RoomMovement(int x, int y, int width, int height)
		{
			detectionRectangle = new Rectangle(x, y, width, height);
		}

		// Detects whether the player rectangle intersects with detectionRectangle.
		public bool TriggerExit(Rectangle playerRectangle)
		{
			if (detectionRectangle.Intersects(playerRectangle))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

	}
}
