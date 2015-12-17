using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Animation;

namespace DungeonCrawl.Entities
{
    public abstract class Entity//The base Entity class. Mostly sets a lot of reused variables.
    {
        Board.Board board;
        public string Name;
        public int score;
        private int HP;
        private int AP;
        private int SP;
        private int STR;
        private int DEX;
        private int CON;
        private int INT;
        private int WIS;
        private int CHA;
        public Vector2 loc;
        public Rectangle colbox;
        public int ID;
        public Texture2D Texture;
        private MobileSprite OwnSelf;
        public MobileSprite deadSprite { get; set; }
        public bool moved;
        public bool wentRight = false;
        public bool wentLeft = false;
        public bool wentUp = false;
        public bool wentDown = false;
        internal int attacking = 1;
        internal int neutral = 0;
        internal int dead = -1;
        internal int state = 0;
        public string orientation = "down";
        

        public abstract void update(GameTime gameTime);
        public abstract void Init();
        public abstract void syncAnims();
        public abstract void die();
        public abstract void Draw(SpriteBatch sb);
        public void takeDmg(int dmg)
        {
            this.HP -= dmg;
            if (this.HP <= 0) this.die();
        }
        public void moveCheckReset()
        {
            wentRight = false;
            wentLeft = false;
            wentUp = false;
            wentDown = false;
        }
        public void setHP(int num)
        {
            this.HP = num;
        }
        public void setAP(int num)
        {
            this.AP = num;
        }

        public MobileSprite self
        {
            get { return this.OwnSelf; }
            set { this.OwnSelf = value; }
        }

        public void setSP(int num)
        {
            this.SP = num;
        }
        public void setSTR(int num)
        {
            this.STR = num;
        }
        public void setDEX(int num)
        {
            this.DEX = num;
        }

        internal abstract void bumped(Entity ent);

        public void setCON(int num)
        {
            this.CON = num;
        }
        public void setINT(int num)
        {
            this.INT = num;
        }
        public void setWIS(int num)
        {
            this.WIS = num;
        }
        public void setCHA(int num)
        {
            this.CHA = num;
        }
        public int getHP()
        {
            return HP;
        }
        public int getAP()
        {
            return this.AP;
        }
        public int getSP()
        {
            return this.SP;
        }
        public int getSTR()
        {
            return this.STR;
        }
        public int getDEX()
        {
            return this.DEX;
        }
        public int getCON()
        {
            return this.CON;
        }
        public int getINT()
        {
            return this.INT;
        }
        public int getWIS()
        {
            return this.WIS;
        }
        public int getCHA()
        {
            return this.CHA;
        }
    }
}
