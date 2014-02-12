using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.Entities
{
    class Goblin : Entity
    {
        public PC player;
        public Board.Board board;
        public void Init()
        {
            this.setHP(100);
            this.setAP(100);
            this.setSP(100);
            this.loc = new Vector2(0, 0);
        }
        public override void update()
        {
            int xDist = (int)((player.getLoc().X / Board.Tile.tileSize) - (this.loc.X / Board.Tile.tileSize));
            int yDist = (int)((player.getLoc().Y / Board.Tile.tileSize) - (this.loc.Y / Board.Tile.tileSize));
            Console.WriteLine(xDist);
            if ((Math.Abs(xDist) < 3) && Math.Abs(yDist) < 3)
            {
                if (xDist > 0)
                {
                    System.Movement.move(this, 3, 0, board, Board.Tile.tileSize);
                }
                if (xDist < 0)
                {
                    System.Movement.move(this, -3, 0, board, Board.Tile.tileSize);
                }
                if (yDist > 0)
                {
                    System.Movement.move(this, 0, 3, board, Board.Tile.tileSize);
                }
                if (yDist < 0)
                {
                    System.Movement.move(this, 0, -3, board, Board.Tile.tileSize);
                }
            }

        }
        public void spawn(int x, int y)
        {
            this.loc = new Vector2(x, y);
        }

        public Vector2 getLoc()
        {
            return this.loc;
        }
        public void setBoard(Board.Board b)
        {
            this.board = b;
        }
    }
}
