using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DungeonCrawl.Entities
{
    public abstract class Entity
    {
        private string Name;
        private int HP;
        private int AP;
        private int SP;
        private int STR;
        private int DEX;
        private int CON;
        private int INT;
        private int WIS;
        private int CHA;
        private bool isDead;
        private ArrayList occupiedCells = new ArrayList();
        public void setHP(int num)
        {
            this.HP = num;
        }
        public void setAP(int num)
        {
            this.AP = num;
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
        public void setDead(bool dead)
        {
            this.isDead = dead;
        }
        public bool dead()
        {
            return this.isDead;
        }
    }
}
