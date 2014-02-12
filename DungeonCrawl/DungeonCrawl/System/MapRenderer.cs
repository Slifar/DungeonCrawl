using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DungeonCrawl.System;
using DungeonCrawl.Board;

namespace DungeonCrawl.System
{
    class MapRenderer
    {
        GraphicsDevice gd;
        RenderTarget2D renderTarget;
        public void init(GraphicsDevice passed)
        {
            gd = passed;
        }
        public Texture2D Render(Board.Board board, SpriteBatch spriteBatch)
        {
            Texture2D tex;
            renderTarget = new RenderTarget2D(
                gd,
                board.Width * Tile.tileSize,
                board.Height * Tile.tileSize,
                false,
                gd.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24);
            gd.SetRenderTarget(renderTarget);
           // gd.Clear();
            Vector2 firstSquare = Vector2.Zero; //new Vector2(Camera.Location.X / Tile.tileSize, Camera.Location.Y / Tile.tileSize);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = Vector2.Zero;//new Vector2(Camera.Location.X % Tile.tileSize, Camera.Location.Y % Tile.tileSize);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            int height = gd.Viewport.Height / Tile.tileSize + 5;
            int width = gd.Viewport.Width / Tile.tileSize + 5;
            //spriteBatch.Begin();
            for (int y = 0; y < board.Rows.Count; y++)//Draw all of the map tiles
            {
                for (int x = 0; x < board.Rows[y].Columns.Count; x++)
                {
                    if (y + firstY < board.Rows.Count && x + firstX < board.Rows[y + firstY].Columns.Count)//We are set to render off screen to make scrolling smooth, so make sure there is something to render before trying
                    {
                        if (board.Rows[y + firstY].Columns[x + firstX].TileID == 0)//Currently can only draw a floor or wall. Will need to change to use the cell's texture so a bunch of if statements can be avoided when adding other tile types.
                        {
                            spriteBatch.Draw(
                            Game1.wall,
                            new Rectangle((x * Tile.tileSize) - offsetX, (y * Tile.tileSize) - offsetY, Tile.tileSize, Tile.tileSize),
                            Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(
                            Game1.floor,
                            new Rectangle((x * Tile.tileSize) - offsetX, (y * Tile.tileSize) - offsetY, Tile.tileSize, Tile.tileSize),
                            Color.White);
                        }
                    }

                }
            }
            spriteBatch.End();
            gd.SetRenderTarget(null);
            tex = (Texture2D)renderTarget;
            spriteBatch.Begin();
            return tex;

        }
    }
}
