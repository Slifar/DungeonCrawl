using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.Entities
{
    static class Collisions
    {
        static Board.Board board;
        static Entities.PC player;
        public static void Init(Board.Board b, Entities.PC p)
        {
            board = b;
            player = p;
        }
        public static void checkAtk(Rectangle entBox, int damage)
        {

            foreach (Entity e in board.ents)//Since we likely only have a few entities on any given board, we can just check all of them
            //Since we only check when we attack, which happens far less than movement
            {
                if (entBox.Intersects(e.self.CollisionBox))
                {
                    e.takeDmg(damage);//If an entity intersects with the collision box for the attack, it takes damage
                }
            }
        }
        public static void checkMobAtk(Rectangle entBox, int damage)
        {
            
            if (entBox.Intersects(player.self.CollisionBox))
                {
                    player.takeDmg(damage);//If an entity intersects with the collision box for the attack, it takes damage
                }

        }

        /*
         * Check if a given entity will collide with anything if it were to ad X and Y to it's X and Y coords, respectively
         * If it will collide, return true, otherwise, return false
         */
        internal static bool check(Entity ent, Board.CollisionCell collisionCell, float x, float y)
        {
            bool blocked = false;
            Rectangle entBox = new Rectangle();
            entBox.Height = ent.self.CollisionBox.Height;
            entBox.Width = ent.self.CollisionBox.Width;
            entBox.X = ent.self.CollisionBox.X + (int)x;
            entBox.Y = ent.self.CollisionBox.Y + (int)y;
            if (collisionCell.isBlocked() && entBox.Intersects(collisionCell.getColBox()))
            {
                blocked = true;
            }
            foreach (Entity e in collisionCell.getEnts())
            {
                if (e != ent && entBox.Intersects(e.self.CollisionBox))
                {
                    e.bumped(ent);
                    blocked = true;
                    break;
                }
            }

            return blocked;
        }
    }
}
