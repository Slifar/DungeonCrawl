using Animation;
using DungeonCrawl.Board;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.Entities
{
    class Skeleton : Entity
    {
        public PC player;
        MobileSprite attack;
        public Board.Board board;
        public Texture2D tex {get; set;}
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
            self.HorizontalCollisionBuffer = 5 * Tile.tileSize / 8;
            self.VerticalCollisionBuffer = 2 * Tile.tileSize / 3;
            self.VerticalCollisionOffset = -17;
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
            state = attacking;
            if (self.Sprite.CurrentAnimation == "left" || self.Sprite.CurrentAnimation == "leftstop")//If we are facing left, make a collision box to our left
            {
                attack.Sprite.CurrentAnimation = "left";
                orientation = "left";
                Rectangle atkBox = new Rectangle();
                atkBox.Width = this.self.CollisionBox.Width / 2;
                atkBox.Height = this.self.CollisionBox.Height;
                atkBox.X = this.self.CollisionBox.X - atkBox.Width;
                atkBox.Y = this.self.CollisionBox.Y;
                Collisions.checkMobAtk(atkBox, 5); //Check for collisions with this new collision box
                syncAnims();
            }
            else if (self.Sprite.CurrentAnimation == "right" || self.Sprite.CurrentAnimation == "rightstop")
            {
                attack.Sprite.CurrentAnimation = "right";
                orientation = "right";
                Rectangle atkBox = new Rectangle();
                atkBox.Width = this.self.CollisionBox.Width / 2;
                atkBox.Height = this.self.CollisionBox.Height;
                atkBox.X = this.self.CollisionBox.Right + 1;
                atkBox.Y = this.self.CollisionBox.Y;
                Collisions.checkMobAtk(atkBox, 5);
                syncAnims();
            }
            else if (self.Sprite.CurrentAnimation == "down" || self.Sprite.CurrentAnimation == "downstop")
            {
                attack.Sprite.CurrentAnimation = "down";
                orientation = "down";
                Rectangle atkBox = new Rectangle();
                atkBox.Width = this.self.CollisionBox.Width;
                atkBox.Height = this.self.CollisionBox.Height / 2;
                atkBox.X = this.self.CollisionBox.X;
                atkBox.Y = this.self.CollisionBox.Bottom + 1;
                Collisions.checkMobAtk(atkBox, 5);
                syncAnims();
            }
            else if (self.Sprite.CurrentAnimation == "up" || self.Sprite.CurrentAnimation == "upstop")
            {
                attack.Sprite.CurrentAnimation = "up";
                orientation = "up";
                Rectangle atkBox = new Rectangle();
                atkBox.Width = this.self.CollisionBox.Width;
                atkBox.Height = this.self.CollisionBox.Height / 2;
                atkBox.X = this.self.CollisionBox.X;
                atkBox.Y = this.self.CollisionBox.Y - atkBox.Height;
                Collisions.checkMobAtk(atkBox, 5);
                syncAnims();
            }
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
                if (this.state == attacking)
                {
                    attack.Position = self.Position;
                    //attack.DrawPosition = self.DrawPosition;
                    Vector2 pos = self.DrawPosition;
                    pos.Y -= 16;
                    attack.DrawPosition = pos;
                    attack.Update(time);
                    //Check to see if we've stopped attacking, and change state back to normal if we have
                    if (attack.Sprite.CurrentAnimation == "upstop" || attack.Sprite.CurrentAnimation == "leftstop" ||
                        attack.Sprite.CurrentAnimation == "rightstop" || attack.Sprite.CurrentAnimation == "downstop")
                    {
                        state = neutral;
                        syncAnims();
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
            else if (state == attacking) { attack.Draw(sb); }
        }
        public override void die()
        {
            this.state = dead;
        }

        internal override void bumped(Entity ent)
        {
            
        }
    }
}
