using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.Entities
{
    class PC : Entity
    {
        public void Init()
        {
            this.setHP(100);
            this.setAP(100);
            this.setSP(100);
        }
        public void Update()
        {
        }
    }
}
