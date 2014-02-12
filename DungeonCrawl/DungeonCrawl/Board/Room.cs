using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DungeonCrawl.Board
{
    class Room
    {
        private int width = 5;
        private int height = 5;
        private ArrayList x = new ArrayList();
        private ArrayList y = new ArrayList();
        private Room northNeigh = null;
        private Room westNeigh = null;
        private Room eastNeigh = null;
        private Room southNeigh = null;
        public Room(int w, int h)
        {
            this.width = w;
            this.height = h;
        }
        /*private Cell[,] cells;
        public Room(int pwidth, int pheight)
        {
            cells = new Cell[pwidth, pheight];
        }
        public Room()
        {
            cells = new Cell[width, height];
        }*/

    }
}
