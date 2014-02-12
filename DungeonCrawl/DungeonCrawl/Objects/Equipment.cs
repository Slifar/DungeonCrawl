using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Animation;
using DungeonCrawl.Board;

namespace DungeonCrawl.Objects
{
    public abstract class Equipment //This is mostly a copy of the Player and Skeleton's animation initialization and management methods
        //It's in an abstract class so all equipments can inherit them
        //The Player and Skeleton's methods will eventually be inherited in the same way
    {
        public MobileSprite self { get; set; }//The default sprite
        public MobileSprite attack { get; set; }//The attacking sprite
        internal int attacking = 1;
        internal int neutral = 0;
        internal int state = 0;
        internal bool atkOnly = false;
        public string orientation = "down";
        public void setTex(Texture2D Walk) //Initialize all of the animations
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
            self.HorizontalCollisionBuffer = 5 * Tile.tileSize / 8;
            self.VerticalCollisionBuffer = 2 * Tile.tileSize / 3;
            self.VerticalCollisionOffset = -17;
        }
        public void setAtkTex(Texture2D AtkTex) //Set the attack animations
        {
            attack = new MobileSprite(AtkTex);
            attack.Sprite.AddAnimation("upstop", 0, 0, 64, 64, 1, 0.1f);
            attack.Sprite.CurrentAnimation = "upstop";
            attack.Sprite.AddAnimation("up", 64, 0, 64, 64, 5, 0.05f, "upstop");
            attack.Sprite.AddAnimation("downstop", 0, 128, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("down", 64, 128, 64, 64, 5, 0.05f, "downstop");
            attack.Sprite.AddAnimation("leftstop", 0, 64, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("left", 64, 64, 64, 64, 5, 0.05f, "leftstop");
            attack.Sprite.AddAnimation("rightstop", 0, 192, 64, 64, 1, 0.1f);
            attack.Sprite.AddAnimation("right", 64, 192, 64, 64, 5, 0.05f, "rightstop");
        }
        public void update(GameTime time)
        {
            if (self != null)
            {
                self.Update(time);
                if (state == attacking)
                {
                    attack.Position = self.Position;
                    Vector2 pos = self.DrawPosition;
                    pos.Y -= 16;
                    attack.DrawPosition = pos;
                    attack.Update(time);
                } 
            }
            else if (attack != null)//If we have no neutral animation, but we have an attack animation, then we must only appear when attacking.
                //To help keep things from breaking, copy "attack" to self, but set a flag that prevents us from drawing it when we are not attacking.
            {
                this.self = attack;
                atkOnly = true;
            }
        }
        internal void Draw(SpriteBatch spriteBatch)
        {
            if (!atkOnly)//If this object does not only appear when attacking
            {
                if (state == neutral && self != null) { self.Draw(spriteBatch); }
                else if (state == attacking && attack != null) { attack.Draw(spriteBatch); }
            }
            else
            {
                if (state == attacking && attack != null) { attack.Draw(spriteBatch); }
            }
        }
    }
}
