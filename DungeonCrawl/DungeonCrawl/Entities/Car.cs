using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using DungeonCrawl.Board;
using Animation;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawl.Entities
{
    class Car : Entity
    {
        int scanTimer = 0;
        int clearTimer = 0;
        int tarTimer = 0;
        int compression = 0;
        public List<Entities.Entity> carsBumped = new List<Entities.Entity>();
        public List<Entities.Entity> Targets = new List<Entities.Entity>();
        public List<Entities.Entity> totalCarsBumped = new List<Entities.Entity>();
        int cycles = 0;
        Entity curTar = null;
        static Random rand = new Random();
        int xCheck = 0;//rand.Next(0, 100);
        int yCheck = 0;//rand.Next(0, 100);
        public PC player;
        public Board.Board board;
        MobileSprite attack;
        private int compressionThreshold = 4;
        private int compressionRadius = 7;
        private int scanRange = 100;

        public Texture2D tex { get; set; }
        public Texture2D deadTex { get; set; }
        public override void Init()
        {
            rand = new Random();
            this.setHP(100);
            this.setAP(100);
            this.setSP(100);
            this.loc = new Vector2(0, 0);
            if (this.tex != null) { this.setTex(this.tex); }
            if (this.deadTex != null) { this.setDieTex(this.deadTex); }
        }
        public override void syncAnims()
        {
        }
        public void setTex(Texture2D tex)//Initialize animations and collision box
        {
            self = new MobileSprite(tex);
            self.Sprite.AddAnimation("upstop", 0, 0, 64, 64, 1, 0.1f);
            self.Sprite.CurrentAnimation = "upstop";
            self.Sprite.AddAnimation("up", 64, 0, 64, 64, 8, 0.1f);
            self.Sprite.AddAnimation("downstop", 0, 128, 64, 64, 1, 0.1f);
            self.Sprite.AddAnimation("down", 64, 128, 64, 64, 8, 0.1f);
            self.Sprite.AddAnimation("leftstop", 0, 64, 64, 64, 1, 0.1f);
            self.Sprite.AddAnimation("left", 64, 64, 64, 64, 8, 0.1f);
            self.Sprite.AddAnimation("rightstop", 0, 192, 64, 64, 1, 0.1f);
            self.Sprite.AddAnimation("right", 64, 192, 64, 64, 8, 0.1f);
            self.HorizontalCollisionBuffer = 4 * Tile.tileSize / 8;
            self.HorizontalCollisionOffset = 0;
            self.VerticalCollisionBuffer = 3 * Tile.tileSize / 6;
            self.VerticalCollisionOffset = 0;
        }
        public void setDieTex(Texture2D tex)//Initialize dying animation
        {
            deadSprite = new MobileSprite(tex);
            deadSprite.Sprite.AddAnimation("dying", 0, 0, 64, 64, 6, 0.1f, "dead");
            deadSprite.Sprite.CurrentAnimation = "dying";
            deadSprite.Sprite.AddAnimation("dead", 0, 320, 64, 64, 1, 0.1f);
        }
        public void setAtkTex(Texture2D AtkTex) //Set the attack animations
        {
            attack = new MobileSprite(AtkTex);
            attack.Sprite.AddAnimation("upstop", 0, 0, 64, 64, 1, 0.1f);
            attack.Sprite.CurrentAnimation = "upstop";
            attack.Sprite.AddAnimation("up", 64, 0, 64, 64, 5, 0.1f, "upstop");
            attack.Sprite.AddAnimation("downstop", 0, 128, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("down", 64, 128, 64, 64, 5, 0.1f, "downstop");
            attack.Sprite.AddAnimation("leftstop", 0, 64, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("left", 64, 64, 64, 64, 5, 0.1f, "leftstop");
            attack.Sprite.AddAnimation("rightstop", 0, 192, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("right", 64, 192, 64, 64, 5, 0.1f, "rightstop");
        }
        internal void Attack() //Method to cause the entity to attack
        {
            
        }
        /*
         * Check if we are dead. If we are not, then attempt to move to the player if they are within range, then update animations and positions
         * Otherwise, update our dead animation
         */
        public override void update(GameTime time)
        {
            if (scanTimer <= 0 && (curTar == null || tarTimer < 1))
            {
                scanTimer = 60;
                tarTimer = 150;
                scanForTarget();
            }
            else
            {
                scanTimer--;
                tarTimer--;
            }
            if (clearTimer <= 0)
            {
                clearTimer = 900;
                carsBumped.Clear();
            }
            else clearTimer--;
            if (this.curTar != null)
            {
                if (carsBumped.Count >= board.ents.Count) carsBumped.Clear();
                int xDist = (int)((curTar.loc.X / Board.Tile.tileSize) - (this.loc.X / Board.Tile.tileSize));
                int yDist = (int)((curTar.loc.Y / Board.Tile.tileSize) - (this.loc.Y / Board.Tile.tileSize));
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
                AnimCheck.changeAnim(this);
                self.Position = this.loc;
                self.Update(time);
                this.moved = false;
                this.moveCheckReset();
            }
            else
            {
                
                if(xCheck < 33)
                {
                    System.Movement.move(this, -3, 0, board, Board.Tile.tileSize);
                }
                else if(xCheck < 66)
                {
                    System.Movement.move(this, 3, 0, board, Board.Tile.tileSize);
                }
                if(yCheck < 33)
                {
                    System.Movement.move(this, 0, -3, board, Board.Tile.tileSize);
                }
                else if(yCheck < 66)
                {
                    System.Movement.move(this, 0, 3, board, Board.Tile.tileSize);
                }
                AnimCheck.changeAnim(this);
                self.Position = this.loc;
                self.Update(time);
                this.moved = false;
                this.moveCheckReset();
            }
        }

        private void scanForTarget()
        {
            curTar = null;
            Targets.Clear();
            for(int i = -scanRange; i <= scanRange; i++)
            {
                for (int j = -scanRange; j <= scanRange; j++)
                {
                    int cellx = (int)(loc.X) / (Board.Tile.tileSize);
                    int celly = (int)(loc.Y) / (Board.Tile.tileSize);
                    Board.CollisionCell curCell = board.ColRows[(int)(loc.Y / Board.Tile.tileSize)].Columns[(int)(loc.X / Board.Tile.tileSize)];
                    Board.CollisionCell Cell = board.ColRows[celly].Columns[cellx];
                    if ((celly + j) >= 0 && (cellx + i) >= 0 && (celly + j) < board.ColRows.Count 
                            && (cellx + i) < board.ColRows[celly + j].Columns.Count)
                    {
                        CollisionCell scanCell = board.ColRows[celly+j].Columns[cellx+i];
                        foreach(Entity e in scanCell.getEnts())
                        {
                            if (e != this && !carsBumped.Contains(e))
                            {
                                Targets.Add(e);
                                if (i < compressionRadius && j < compressionRadius) compression++;
                                if (compression > compressionThreshold) break;
                            }
                        }
                    }
                    if (Targets.Count > 0)
                    {
                        curTar = Targets[rand.Next(Targets.Count)];
                        if (compression > compressionThreshold || carsBumped.Contains(curTar))
                        {
                            curTar = null;
                        }
                    }
                }
            }
            xCheck = rand.Next(0, 100);
            yCheck = rand.Next(0, 100);
            compression = 0;
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
        public override void Draw(SpriteBatch sb)
        {
            if (state == neutral) { self.Draw(sb); }
            else if (state == dead) { deadSprite.Draw(sb); }
        }
        public override void die()
        {
            this.state = dead;
        }

        internal override void bumped(Entity ent)
        {
            if(!carsBumped.Contains(ent)) carsBumped.Add(ent);
            if (!totalCarsBumped.Contains(ent))
            {
                totalCarsBumped.Add(ent);
                if(totalCarsBumped.Count >= board.ents.Count - 1)
                {
                    cycles++;
                    totalCarsBumped.Clear();
                }
            }
            scanForTarget();
            if (carsBumped.Count >= (3*board.ents.Count/4)) carsBumped.Clear();
            score = (board.ents.Count * cycles) + totalCarsBumped.Count;
        }
    }
}

