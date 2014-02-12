using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.System
{
    static class TexHelper //class to generate textures of appropriate sizes so primitives can be drawn with vector positioning. Should not be heavily used once graphics are added.
    {
        public static Texture2D TexMake(int tileSize, GraphicsDevice graphics, Color color)
	    {      
            Texture2D rectangle = new Texture2D(graphics, tileSize, tileSize, false, SurfaceFormat.Color);
            Color[] colorData = new Color[tileSize * tileSize];

            for (int i = 0; i < tileSize * tileSize; i++)
            colorData[i] = color;
            rectangle.SetData<Color>(colorData);
            return rectangle;
	    }


    }
}
