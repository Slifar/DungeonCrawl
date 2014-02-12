using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DungeonCrawl.Board
{
    class Cell
    {
        private bool passable = false;
        private ArrayList entities = new ArrayList();
        private ArrayList objects = new ArrayList();
        public int TileID { get; set; }

        public Cell(int tile)
        {
            this.TileID = tile;
            this.passable = false;
        }

    }
}
