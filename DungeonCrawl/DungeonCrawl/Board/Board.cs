using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DungeonCrawl.Board
{
    /*
     * The Board class. It mostly generates the map and then stores the informtion about it.
     * That's all it does.
     */
    class BoardRow
    {
        public List<Cell> Columns = new List<Cell>();
    }
    class CollisionRow
    {
        public List<CollisionCell> Columns = new List<CollisionCell>();
    }
    class Board
    {
        public List<BoardRow> Rows = new List<BoardRow>();
        public List<CollisionRow> ColRows = new List<CollisionRow>();
        public int Height = Game1.graphicHeight / (Tile.tileSize / 2);
        public int Width = (Game1.graphicWidth / (Tile.tileSize / 2))-1;
        public List<Entities.Entity> ents = new List<Entities.Entity>();
        public Entities.PC Player;
        public Board()
        {
            for (int y = 0; y < Height; y++)
            {
                BoardRow curRow = new BoardRow();
                CollisionRow curcolrow = new CollisionRow();
                for (int x = 0; x < Width; x++)
                {
                    curRow.Columns.Add(new Cell(1));
                    CollisionCell temp = new CollisionCell();
                    temp.xIndex = x;
                    temp.yIndex = y;
                    curcolrow.Columns.Add(temp);
                }
                Rows.Add(curRow);
                ColRows.Add(curcolrow);
            }
            //GenerateCave(Rows, Height, Width);
        }
        public Entities.PC getPlayer()
        {
            return this.Player;
        }
        private void GenerateCave(List<BoardRow> Rows, int Height, int Width)//Cave generation method. Creates a board where each cell has a % chance to be a wall, then passes this board to the secondary method.
        {
            Random rand = new Random();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (rand.Next(0, 100) < 40)
                    {
                        Rows[y].Columns[x].TileID = 0;
                    }
                }
            }
            for (int i = 0; i < 20; i++)
            {
                Rows = Cave2(Rows, Height, Width);
            }
            GenerateCollisionMap();
        }
        private void GenerateCollisionMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (getCell(x, y).TileID == 0)
                    {
                        ColRows[y].Columns[x].block(true);
                    }
                }
            }
        }
        private List<BoardRow> Cave2(List<BoardRow> Rows, int Height, int Width) //Secondary Cave Generation method. Generates/smooths the cave structure based on the random board given
        {
            Random rand = new Random();
            //List<BoardRow> Rows = new List<BoardRow>();
            //Rows = Rows.ToList();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                        int id = Rows[y].Columns[x].TileID;
                        int adj1 = 0;
                        int adj2 = 0;
                        for (int xx = -1; xx <= 1; xx++)
                        {
                            for (int yy = -1; yy <= 1; yy++)
                            {
                                if (x + xx >= 0 && y + yy >= 0 &&
                                    x + xx < Width && y + yy < Height)
                                {
                                    if (Rows[y + yy].Columns[x + xx].TileID == 0)
                                    {
                                        adj1++;
                                    }
                                }
                                else { adj1++; }
                            }
                        }
                        if (id == 0)
                        {
                            if (adj1 < 4)
                            {
                                Rows[y].Columns[x].TileID = 1;
                            }
                            else
                            {
                                Rows[y].Columns[x].TileID = 0;
                            }
                        }
                        else
                        {
                            if (adj1 > 4)
                            {
                                Rows[y].Columns[x].TileID = 0;
                            }
                            else
                            {
                                Rows[y].Columns[x].TileID = 1;
                            }
                        }
                        adj1 = 0;
                }
            }
            return Rows;
        }

        private void Generate(List<BoardRow> Rows, int Height, int Width)//Unfinished method to randomly generate dungeons with "normal rooms" (I.E. square/circular, manmade rooms)
        {
            int halfx = Width / 2;
            int halfy = Height / 2;
            Random rand = new Random();
            int rmheight = rand.Next(3, 10);
            int rmwidth = rand.Next(3, 10);
            halfx -= rmwidth;
            halfy -= rmheight;
            
            for (int x = 0; x < rmwidth; x++)
            {
                for (int y = 0; y < rmheight; y++)
                {
                    Rows[halfy + y].Columns[halfx + x].TileID = 1;
                }
            }
            for (int i = 0; i < 100; i++)
            {

            }
        }
        private Cell getCell(int x, int y)
        {
            return Rows[y].Columns[x];
        }
        private bool hasWall(int x, int y)
        {
            bool wall = false;
            if (Rows[y].Columns[x - 1].TileID == 0)
            {
                wall = true;
            }
            else if (Rows[y].Columns[x + 1].TileID == 0)
            {
                wall = true;
            }
            else if (Rows[y - 1].Columns[x].TileID == 0)
            {
                wall = true;
            }
            else if (Rows[y + 1].Columns[x].TileID == 0)
            {
                wall = true;
            }
            return wall;
        }
    }
}
