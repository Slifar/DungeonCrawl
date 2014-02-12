using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.System
{
    static class Camera
        /*
         * The camera object. Effectively determines what gets rendered and where it gets rendered based on the player's position
         * Creates the scrolling effect.
         * Tries to keep the player in the center of the screen at all times, if possible, unless a border is reached.
         */
    {
        static public Vector2 Location = Vector2.Zero;

        internal static void Track(Entities.PC Player, int width, int height, int tileSize, int boardWidth, int boardHeight)
        {
            float playerX = Player.getLoc().X;
            float playerY = Player.getLoc().Y;
            //Camera.Location.X = MathHelper.Clamp(playerX - ((width - 5) * tileSize / 2), 0, (boardWidth * tileSize) - ((width - 5) * tileSize));
            //Camera.Location.Y = MathHelper.Clamp(playerY - ((height - 5) * tileSize / 2), 0, (boardHeight * tileSize) - ((height - 5) * tileSize) - 24);
            Camera.Location.X = MathHelper.Clamp(playerX - (width / 2), 0, (boardWidth * tileSize) - width);
            Camera.Location.Y = MathHelper.Clamp(playerY - (height  / 2), 0, (boardHeight * tileSize) - height);
            //Console.WriteLine((boardWidth * tileSize) - ((width - 5) * tileSize)); //Debugging text!
        }

    }
}
