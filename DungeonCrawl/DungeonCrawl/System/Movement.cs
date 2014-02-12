using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.System
{
    static class Movement
    {
        internal static void move(Entities.Entity ent, float x, float y, Board.Board board, int tileSize)
        {
            bool blocked = false;
            bool shouldStop = false;
            int cellx = (int)(ent.loc.X + x) / Board.Tile.tileSize;
            int celly = (int)(ent.loc.Y + y) / Board.Tile.tileSize;
            Board.CollisionCell curCell = board.ColRows[(int)(ent.loc.Y / Board.Tile.tileSize)].Columns[(int)(ent.loc.X / Board.Tile.tileSize)];
            Board.CollisionCell Cell = board.ColRows[celly].Columns[cellx];
            for (int i = -1; i <= 1; i++)
            {
                if (shouldStop) break; //If we've already found something we've collided with, we don't need to check more, so break
                else
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if ((celly + j) >= 0 && (cellx + i) >= 0 && (celly+j) < board.ColRows.Count //If we've found we have collided with something
                            && (cellx + i) < board.ColRows[celly+j].Columns.Count && Entities.Collisions.check(ent, board.ColRows[celly + j].Columns[cellx + i], x, y))
                        {
                            //Set the appropriate flags and break from the loop, since we don't need to check more
                            shouldStop = true;
                            blocked = true;
                            break;
                        }
                    }
                }
            }
            //if (!Cell.isBlocked() && ((Cell.getEnts().Contains(ent) && Cell.getEnts().Count < 2) || (!Cell.getEnts().Contains(ent) && Cell.getEnts().Count < 1))) //Replace true with call to colision detection logic later
            if(!blocked)//If we haven't been blocked, we can move
            {
                ent.moved = true; //Set our moved flag to true
                
                curCell.remEnt(ent);//Remove ourselves from our old cell's entity list
                Cell.addEnt(ent);//Add ourselves to our new cell's entity list
                ent.loc.X = MathHelper.Clamp(ent.loc.X + x, 0, (board.Width) * tileSize - tileSize);
                ent.loc.Y = MathHelper.Clamp(ent.loc.Y + y, 0, (board.Height) * tileSize - tileSize);
                /*
                 * If we have animations, we need to set the flags to denote which way we moved so we know when to change animations
                 * We also need to start animations if they are not playing
                 */
                if (ent.self != null)
                {
                    if (ent.self.Sprite.CurrentAnimation != "left" && ent.self.Sprite.CurrentAnimation != "right" && ent.self.Sprite.CurrentAnimation != "up" &&
                        ent.self.Sprite.CurrentAnimation != "down")
                    {
                        if (x > 0) { ent.self.Sprite.CurrentAnimation = "right"; ent.wentRight = true; ent.syncAnims(); }
                        else if (x < 0) { ent.self.Sprite.CurrentAnimation = "left"; ent.wentLeft = true; ent.syncAnims(); }
                        else if (y > 0) { ent.self.Sprite.CurrentAnimation = "down"; ent.wentDown = true; ent.syncAnims(); }
                        else if (y < 0) { ent.self.Sprite.CurrentAnimation = "up"; ent.wentUp = true; ent.syncAnims(); }
                    }
                    if (x > 0) { ent.wentRight = true; }
                    else if (x < 0) { ent.wentLeft = true; }
                    else if (y > 0) { ent.wentDown = true; }
                    else if (y < 0) { ent.wentUp = true; }
                    ent.self.Position = ent.loc;
                }
            }
            else
            {
                if (ent.self.Sprite.CurrentAnimation != "left" && ent.self.Sprite.CurrentAnimation != "right" && ent.self.Sprite.CurrentAnimation != "up" &&
                    ent.self.Sprite.CurrentAnimation != "down")
                {
                    if (x > 0) { ent.self.Sprite.CurrentAnimation = "right"; ent.syncAnims(); }
                    else if (x < 0) { ent.self.Sprite.CurrentAnimation = "left"; ent.syncAnims(); }
                    else if (y > 0) { ent.self.Sprite.CurrentAnimation = "down"; ent.syncAnims(); }
                    else if (y < 0) { ent.self.Sprite.CurrentAnimation = "up"; ent.syncAnims(); }
                }
            }
        }
    }
}
