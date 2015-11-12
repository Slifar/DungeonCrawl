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
    class PC : Entity
    {
        //public MobileSprite self;
        private int closestTile;
        MobileSprite attack;
        MobileSprite cast;
        MobileSprite deadSprite;
        bool canMove { get; set; }
        bool canAtk { get; set; }
        public List<Objects.Equipment> clothes = new List<Objects.Equipment>();
        public List<Objects.Equipment> Equipment = new List<Objects.Equipment>();
        //Vector2 self;
        public void setTex(Texture2D Walk) //Initialize all of the player's animations and his collision box
        {
            self = new MobileSprite(Walk);
            self.Sprite.AddAnimation("upstop", 0, 0, 64, 64, 1, 0.1f);
            self.Sprite.CurrentAnimation = "upstop";
            self.Sprite.AddAnimation("up", 64, 0, 64, 64, 8, 0.1f);
            self.Sprite.AddAnimation("downstop", 0, 128, 64, 64, 1, 0.1f);
            self.Sprite.AddAnimation("down", 64, 128, 64, 64, 8, 0.1f);
            self.Sprite.AddAnimation("leftstop", 0, 64, 64, 64, 1, 0.1f);
            self.Sprite.AddAnimation("left", 64, 64, 64, 64, 8, 0.1f);
            self.Sprite.AddAnimation("rightstop", 0, 192, 64, 64, 1, 0.1f);
            self.Sprite.AddAnimation("right", 64, 192, 64, 64, 8, 0.1f);
            self.HorizontalCollisionBuffer = 4 *Tile.tileSize / 8;
            self.HorizontalCollisionOffset = 0;
            self.VerticalCollisionBuffer = 3 * Tile.tileSize / 6;
            self.VerticalCollisionOffset = 0;
        }
        public void equipClothes(Objects.Equipment toEquip)
        {
            clothes.Add(toEquip);
        }
        public void equipEquipment(Objects.Equipment toEquip)
        {
            Equipment.Add(toEquip);
        }
        public void setAtkTex(Texture2D AtkTex) //Set the player's attack animations
        {
            attack = new MobileSprite(AtkTex);
            attack.Sprite.AddAnimation("upstop", 0, 0, 32, 32, 1, 0.05f);
            attack.Sprite.CurrentAnimation = "upstop";
            attack.Sprite.AddAnimation("up", 64, 0, 64, 64, 5, 0.05f, "upstop");
            attack.Sprite.AddAnimation("downstop", 0, 128, 65, 65, 1, 0.05f);
            attack.Sprite.AddAnimation("down", 64, 128, 64, 64, 5, 0.05f, "downstop");
            attack.Sprite.AddAnimation("leftstop", 0, 64, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("left", 64, 64, 64, 64, 5, 0.05f, "leftstop");
            attack.Sprite.AddAnimation("rightstop", 0, 192, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("right", 64, 192, 64, 64, 5, 0.05f, "rightstop");
        }
        public void setCastTex(Texture2D CastTex)
        {
            cast = new MobileSprite(CastTex);
            cast.Sprite.AddAnimation("upstop", 0, 0, 64, 64, 1, 0.1f);
            cast.Sprite.CurrentAnimation = "upstop";
            cast.Sprite.AddAnimation("up", 64, 0, 64, 64, 5, 0.05f, "upstop");
            cast.Sprite.AddAnimation("downstop", 0, 128, 64, 64, 1, 0.1f);
            cast.Sprite.AddAnimation("down", 64, 128, 64, 64, 5, 0.05f, "downstop");
            cast.Sprite.AddAnimation("leftstop", 0, 64, 64, 64, 1, 0.1f);
            cast.Sprite.AddAnimation("left", 64, 64, 64, 64, 5, 0.05f, "leftstop");
            cast.Sprite.AddAnimation("rightstop", 0, 192, 64, 64, 1, 0.1f);
            cast.Sprite.AddAnimation("right", 64, 192, 64, 64, 5, 0.05f, "rightstop");
        }
        public override void Init()
        {
            this.setHP(100);
            this.setAP(100);
            this.setSP(100);
            this.loc = new Vector2(0,0);
            state = neutral;
        }
        public void setDieTex(Texture2D tex)//Initialize dying animation
        {
            deadSprite = new MobileSprite(tex);
            deadSprite.Sprite.AddAnimation("dying", 0, 0, 64, 64, 6, 0.1f, "dead");
            deadSprite.Sprite.CurrentAnimation = "dying";
            deadSprite.Sprite.AddAnimation("dead", 0, 320, 64, 64, 1, 0.1f);
        }
        /*
         * The player's update method. Checks to see if the player is attacking, and sets the correct draw position and animation for the attack sprite if we are
         * Resets any movement flags we have, so we can tell if we have moved after the next cycle
         * Finally, it will call update on any equipped items, allowing them to update animations
         */
        public override void update(GameTime time)
        {
            AnimCheck.changeAnim(this);
            self.Update(time);
            if (this.getHP() <= 0) { this.die(); }
            if (state == attacking)
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
            else if (state == dead)
            {
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
            this.moved = false;
            this.moveCheckReset();
            foreach (Objects.Equipment e in clothes)
            {
                e.update(time);
            }
            foreach (Objects.Equipment e in Equipment)
            {
                e.update(time);
            }
        }
        public void spawn(int x, int y)//Spawns the player at the given coords X,Y
        {
            this.loc = new Vector2(x,y);
            canMove = true;
        }

        public Vector2 getLoc()
        {
            return this.loc;
        }

        internal void Update(KeyboardState prev, KeyboardState ks)
        {
            
        }

        internal void moveY(float p)
        {
            this.loc.Y = p;
        }

        internal void moveX(float p)
        {
            this.loc.X = p;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (state == neutral)
            {
                self.Draw(spriteBatch);
                foreach (Objects.Equipment e in clothes)
                {
                    e.self.DrawPosition = self.DrawPosition;
                    e.Draw(spriteBatch);
                }
                foreach (Objects.Equipment e in Equipment)
                {
                    e.self.DrawPosition = self.DrawPosition;
                    e.Draw(spriteBatch);
                }
            }
            else if (state == attacking)
            {
                attack.Draw(spriteBatch);
                foreach (Objects.Equipment e in clothes)
                {
                    e.attack.DrawPosition = attack.DrawPosition;
                    e.Draw(spriteBatch);
                }
                foreach (Objects.Equipment e in Equipment)
                {
                    e.attack.DrawPosition = attack.DrawPosition;
                    e.Draw(spriteBatch);
                }
            }
            else if (state == dead) { deadSprite.Draw(spriteBatch); }
            
        }
        
        /*
         * The following synchronizes the animation of the player with that of his equipment
         * This allows us to dynamically layer sprites so the player's equipment can change
         */

        public override void syncAnims()
        {
            if (state == neutral)
            {
                foreach (Objects.Equipment e in clothes)
                {
                    e.state = this.state;
                    e.self.Sprite.CurrentAnimation = this.self.Sprite.CurrentAnimation;
                }
                foreach (Objects.Equipment e in Equipment)
                {
                    e.state = this.state;
                    e.self.Sprite.CurrentAnimation = this.self.Sprite.CurrentAnimation;
                }
            }
            else if (state == attacking)
            {
                foreach (Objects.Equipment e in clothes)
                {
                    e.state = this.state;
                    e.attack.Sprite.CurrentAnimation = this.attack.Sprite.CurrentAnimation;
                }
                foreach (Objects.Equipment e in Equipment)
                {
                    e.state = this.state;
                    e.attack.Sprite.CurrentAnimation = this.attack.Sprite.CurrentAnimation;
                }
            }
        }
        /*
         * Attack. We make a new rectangle object and set it in the correct position based on where the player is facing.
         * If there is an entity in that position, we deal damage to them.
         * If the entities HP reaches 0, it dies and is removed from the board after its death animation
         */
        internal void Attack() //Method to cause the player to attack
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
                Collisions.checkAtk(atkBox, 20); //Check for collisions with this new collision box
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
                Collisions.checkAtk(atkBox, 20);
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
                Collisions.checkAtk(atkBox, 20);
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
                Collisions.checkAtk(atkBox, 20);
                syncAnims();
            }
        }
        public override void die()
        {
            this.state = dead;
        }
    }
}
