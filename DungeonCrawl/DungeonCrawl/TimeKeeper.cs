using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonCrawl
{
    public class TimeKeeper : DrawableGameComponent
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public TimeKeeper(Game game)
            : base(game)
        {
            content = Game.Content;//new ContentManager(game.Services);
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            //spriteFont = _content.Load<SpriteFont>(@"Fonts\gamefont");

            //spriteFont = content.Load<SpriteFont>(@"\Fonts\gamefont");

            spriteFont = content.Load<SpriteFont>("font");



        }

        protected override void UnloadContent()
        {

            content.Unload();

        }


        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("Time: {0}", elapsedTime);

            spriteBatch.Begin();

            spriteBatch.DrawString(spriteFont, fps, new Vector2(33, 65), Color.Black);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(32, 64), Color.White);

            spriteBatch.End();
        }
    }
}
