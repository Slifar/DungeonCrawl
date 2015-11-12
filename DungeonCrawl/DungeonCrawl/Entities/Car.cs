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
        public PC player;
        public Board.Board board;
        MobileSprite attack;
        public Texture2D tex { get; set; }
        public Texture2D deadTex { get; set; }
        public override void Init()
        {
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
            if (this.state != dead)
            {
                int xDist = (int)((player.getLoc().X / Board.Tile.tileSize) - (this.loc.X / Board.Tile.tileSize));
                int yDist = (int)((player.getLoc().Y / Board.Tile.tileSize) - (this.loc.Y / Board.Tile.tileSize));
                if (this.state != attacking)
                {
                    if ((Math.Abs(xDist) < 1) && Math.Abs(yDist) < 1)
                    {
                        this.Attack();
                    }
                    else if ((Math.Abs(xDist) < 5) && Math.Abs(yDist) < 5)
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
                AnimCheck.changeAnim(this);
                self.Position = this.loc;
                self.Update(time);
                this.moved = false;
                this.moveCheckReset();
            }
            else
            {
                Vector2 pos = self.DrawPosition;
                pos.Y -= 16;
                deadSprite.DrawPosition = pos;
                deadSprite.Update(time);
                if (deadSprite.Sprite.CurrentAnimation == "dead")
                {
                    self.Position = Vector2.Zero;
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
        public override void Draw(SpriteBatch sb)
        {
            if (state == neutral) { self.Draw(sb); }
            else if (state == dead) { deadSprite.Draw(sb); }
        }
        public override void die()
        {
            this.state = dead;
        }
    }
}

