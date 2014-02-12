using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawl.Board
{
    static class Tile
    {
        static public int tileSize { get; set; }
        static public Texture2D TileTex;

        static public Rectangle GetRec(int index)
        {
            return new Rectangle(index * tileSize, 0, tileSize, tileSize);
        }
    }
}
