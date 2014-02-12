using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl.Entities
{
    static class AnimCheck
        /*
         * The class that checks to see if an entity should change it's animation.
         * Effectively, it sees if the entity is playing a movement animation, and if they have moved.
         * If they have not moved but are playing a movement animation, it changes it to the correct idle animation.
         * It will also check if an incorrect animation is playing, E.G. if the left walking animation is playing when we aren't moving left
         * If an inccorect animation is playing, it will change it to the appropriate idle animation, which is then smoothly changed to the correct
         * animation if neccesary due to the structuring of the Movement class.
         */
    {
        public static void changeAnim(Entity e){
            String anim = e.self.Sprite.CurrentAnimation;
            if (!e.moved)
            {
                if (e.self.Sprite.CurrentAnimation == "up")
                {
                    e.self.Sprite.CurrentAnimation = "upstop";
                    e.syncAnims();
                }
                else if (e.self.Sprite.CurrentAnimation == "down")
                {
                    e.self.Sprite.CurrentAnimation = "downstop";
                    e.syncAnims();
                }
                else if (e.self.Sprite.CurrentAnimation == "left")
                {
                    e.self.Sprite.CurrentAnimation = "leftstop";
                    e.syncAnims();
                }
                else if (e.self.Sprite.CurrentAnimation == "right")
                {
                    e.self.Sprite.CurrentAnimation = "rightstop";
                    e.syncAnims();
                }
            }
            else
            {
                if (e.wentLeft == false && anim == "left") { e.self.Sprite.CurrentAnimation = "leftstop"; e.syncAnims(); }
                else if (e.wentRight == false && anim == "right") { e.self.Sprite.CurrentAnimation = "rightstop"; e.syncAnims(); }
                else if (e.wentUp == false && anim == "up") { e.self.Sprite.CurrentAnimation = "upstop"; e.syncAnims(); }
                else if (e.wentDown == false && anim == "down") { e.self.Sprite.CurrentAnimation = "downstop"; e.syncAnims(); }
            }
        }
    }
}
