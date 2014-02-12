using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.Board
{
    class CollisionCell
    {
        bool allBlocked = false;
        public int xIndex = 0;
        public int yIndex = 0;
        List<Entities.Entity> ents = new List<Entities.Entity>();

        public Rectangle getColBox()
        {
            Rectangle c = new Rectangle(xIndex * Tile.tileSize + (Tile.tileSize / 2), yIndex * Tile.tileSize + (Tile.tileSize / 2), Tile.tileSize, Tile.tileSize);
            return c;
        }
        public void block(bool isBlocked)
        {
            this.allBlocked = isBlocked;
        }
        public bool isBlocked()
        {
            return this.allBlocked;
        }
        public void addEnt(Entities.Entity ent)
        {
            if (!this.ents.Contains(ent))
            {
                this.ents.Add(ent); 
            }
        }
        public void remEnt(Entities.Entity ent)
        {
            if (this.ents.Contains(ent))
            {
                this.ents.Remove(ent); 
            }
        }
        public List<Entities.Entity> getEnts()
        {
            return this.ents;
        }
    }
}
