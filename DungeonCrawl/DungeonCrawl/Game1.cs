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

namespace DungeonCrawl
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        FrameRateCounter FRC;
        TimeKeeper TK;
        Random rand = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D floor;
        public static Texture2D wall;
        Texture2D playertex;
        Texture2D colDebug;
        Texture2D gobtex;
        Texture2D playerDead;
        Texture2D skellDead;
        Texture2D skellAtk;
        Texture2D playerAttack;
        Texture2D pants;
        Texture2D pantsAttack;
        Texture2D armor;
        Texture2D armorAtk;
        Texture2D swordAtk;
        Texture2D map;
        Texture2D playerCast;
        Texture2D pantsCast;
        Texture2D armorCast;
        public static int graphicHeight = 600;
        public static int graphicWidth = 800;
        KeyboardState ks;
        KeyboardState prev;
        Vector2 mapPos = Vector2.Zero;
        MapRenderer MapRenderer = new MapRenderer();
        Board.Board board;
        int height;
        int width;
        int remaining;
        static Entities.PC Player;
        bool moving;
        bool needMapRender = true;
        Song Music;
        SoundEffect swordClang;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = graphicWidth;//1280;
            graphics.PreferredBackBufferHeight = graphicHeight;//1024;
            this.IsMouseVisible = true;
            graphics.SynchronizeWithVerticalRetrace = false;
            //this.IsFixedTimeStep = false; //Uncommenting this section allows updates, and thus FPS by extension, to occur more than 60 times per second.
            //However, as movement is currently done by a flat amount per update, this will likely cause the character to move too quickly to be controled.
            
            //graphics.IsFullScreen = true;
        }
        
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Components.Add(new FrameRateCounter(this));
            FRC = new FrameRateCounter(this);
            Components.Add(new TimeKeeper(this));
            TK = new TimeKeeper(this);
            Timer timer = new Timer();
            Board.Tile.tileSize = 32;
            board = new Board.Board();
            Player = new Entities.PC();
            base.Initialize();
            MapRenderer.init(GraphicsDevice);
            board.Player = Player;
            Player.Init();
            //height = GraphicsDevice.Viewport.Height / Tile.tileSize + 5;
            //width = GraphicsDevice.Viewport.Width / Tile.tileSize + 5;
            height = GraphicsDevice.Viewport.Height;
            width = GraphicsDevice.Viewport.Width;
            moving = true;
            while (true)//Randomly search the board until a valid spawn point is found, then spawn the player there.
            {
                int ranx = rand.Next(0, board.Width);
                int rany = rand.Next(0, board.Height);
                if (board.Rows[rany].Columns[ranx].TileID == 1)
                {
                    Player.spawn(ranx * Tile.tileSize, rany * Tile.tileSize);
                    //board.ColRows[rany].Columns[ranx].addEnt(Player);
                    break;
                }
            }
            for (int i = 0; i < 20; i++)//Generate 20 skeletons, and randomly spawn them like we did the player
            {
                while (true)
                {
                    int ranx = rand.Next(0, board.Width);
                    int rany = rand.Next(0, board.Height);
                    if (board.Rows[rany].Columns[ranx].TileID == 1)
                    {
                        Entities.Car gob = new Entities.Car();
                        board.ents.Add(gob);//Add the skeleton to the list of entities
                        gob.player = Player;//Let the skeleton know who the player is
                        gob.board = this.board;//Let the skeleton know what the board is
                        gob.setBoard(board);//Perform board initialization
                        gob.tex = System.TexHelper.TexMake(Tile.tileSize, GraphicsDevice, RandomColor());//Set the default texture
                        gob.setAtkTex(skellAtk);
                        gob.deadTex = skellDead;//Set the dead texture
                        gob.Init();//Initialize the skeleton
                        gob.spawn(ranx * Tile.tileSize, rany * Tile.tileSize);//Spawn the skeleton
                        board.ColRows[rany].Columns[ranx].addEnt(gob);//Add the skeleton to the entity list of the cell it spawned on
                        gob.board = board;
                        break;//Stop looping now that the skeleton is spawned
                    }
                } 
            }
            Entities.Collisions.Init(board, Player);//Pass the board to the collision management system
            //MediaPlayer.IsRepeating = true;
            //MediaPlayer.Volume = 2f;
            //MediaPlayer.Play(Music);//Play the background music
            //Generate the equipment the player is currently using
            Objects.pants pant = new Objects.pants();
            pant.setTex(pants);
            pant.setAtkTex(pantsAttack);
            Player.equipClothes(pant);
            Objects.shirt shirt = new Objects.shirt();
            shirt.setTex(armor);
            shirt.setAtkTex(armorAtk);
            Player.equipClothes(shirt);
            Player.setDieTex(playerDead);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            floor = Content.Load<Texture2D>(@"Tiles/dc-dngn/floor/grey_dirt0");//new Texture2D(GraphicsDevice, 1, 1);
            wall = Content.Load<Texture2D>(@"Tiles/dc-dngn/wall/pebble_red0");//new Texture2D(GraphicsDevice, 1, 1);
            colDebug = new Texture2D(GraphicsDevice, 1, 1);
            playertex = Content.Load<Texture2D>(@"png/walkcycle/BODY_male");
            Player.setTex(playertex);
            playertex = System.TexHelper.TexMake(Tile.tileSize, GraphicsDevice, Color.Orange); //new Texture2D(GraphicsDevice, 1, 1);
            Player.setTex(playertex);
            //floor.SetData(new[] { Color.Tan });
            //wall.SetData(new[] { Color.Brown });
            colDebug.SetData(new[] { Color.White });
            gobtex = Content.Load<Texture2D>(@"png/walkcycle/BODY_skeleton");
            Music = Content.Load<Song>(@"Sound/Music/Colossus");
            swordClang = Content.Load<SoundEffect>(@"Sound/Effects/SwordClang1");
            playerAttack = Content.Load<Texture2D>(@"png/slash/BODY_human");
            skellAtk = Content.Load<Texture2D>(@"png/slash/BODY_skeleton");
            Player.setAtkTex(playerAttack);
            pants = Content.Load<Texture2D>(@"png/walkcycle/LEGS_plate_armor_pants");
            pantsAttack = Content.Load<Texture2D>(@"png/slash/LEGS_plate_armor_pants");
            armor = Content.Load<Texture2D>(@"png/walkcycle/TORSO_plate_armor_torso");
            armorAtk = Content.Load<Texture2D>(@"png/slash/TORSO_plate_armor_torso");
            skellDead = Content.Load<Texture2D>(@"png/hurt/BODY_skeleton");
            swordAtk = Content.Load<Texture2D>(@"png/slash/WEAPON_dagger");
            playerCast = Content.Load<Texture2D>(@"png/spellcast/BODY_male");
            pantsCast = Content.Load<Texture2D>(@"png/spellcast/LEGS_plate_armor_pants");
            armorCast = Content.Load<Texture2D>(@"png/spellcast/TORSO_plate_armor_torso");
            playerDead = Content.Load<Texture2D>(@"png/hurt/BODY_male");
            spriteFont = Content.Load<SpriteFont>("font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            prev = ks;
            ks = Keyboard.GetState();
            if (ks.IsKeyDown(Keys.Escape)) {
                spriteBatch.Dispose();
                Components.Clear();
                this.Initialize(); }
            
            /*if (ks.IsKeyDown(Keys.Left))
            {
                if (Player.state == 0) //If the player is currently in a state where arrows move them
                {
                    Movement.move(Player, -3, 0, board, Tile.tileSize);
                    //Player.moveX(MathHelper.Clamp(Player.getLoc().X - 3, 0 + (Tile.tileSize / 2), (board.Width - width) * (Tile.tileSize + (Tile.tileSize / 2))));
                }
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                if (Player.state == 0)
                {
                    Movement.move(Player, 3, 0, board, Tile.tileSize);
                    //Player.moveX(MathHelper.Clamp(Player.getLoc().X + 3, 0 + (Tile.tileSize / 2), (board.Width - width) * (Tile.tileSize + (Tile.tileSize / 2))));
                }
            }
            
            if (ks.IsKeyDown(Keys.Up))
            {
                if (Player.state == 0)
                {
                    Movement.move(Player, 0, -3, board, Tile.tileSize);
                   // Player.moveY(MathHelper.Clamp(Player.getLoc().Y - 3, 0 + (Tile.tileSize / 2), (board.Height - height) * (Tile.tileSize + (Tile.tileSize / 2)))); 
                }
            }

            if (ks.IsKeyDown(Keys.Down))
            {
                if (Player.state == 0)
                {
                    Movement.move(Player, 0, 3, board, Tile.tileSize);
                  //  Player.moveY(MathHelper.Clamp(Player.getLoc().Y + 3, 0 + (Tile.tileSize / 2), (board.Height - height) * (Tile.tileSize + (Tile.tileSize / 2)))); 
                }
            }*/
            Camera.Track(Player, width, height, Tile.tileSize, board.Width, board.Height);
            //Camera.Track(Player, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, Tile.tileSize, board.Width, board.Height);
            
            //Player.Update(prev, ks);
            /*if (ks.IsKeyDown(Keys.Left))
            {
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X - 5, 0, (board.Width - width) * Tile.tileSize);
            }

            if (ks.IsKeyDown(Keys.Right))
            {
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X + 5, 0, (board.Width - width) * Tile.tileSize);
            }

            if (ks.IsKeyDown(Keys.Up))
            {
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - 5, 0, (board.Height - height) * Tile.tileSize);
            }

            if (ks.IsKeyDown(Keys.Down))
            {
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + 5, 0, (board.Height - height) * Tile.tileSize);
            }*/

            foreach (Entities.Entity e in board.ents) //Update all of the entities!
            {
                e.update(gameTime);
            }
            Player.update(gameTime);//Update the player
            remaining = 0;
            foreach (Entities.Entity e in board.ents)
            {
                if (e.state != -1)
                {
                    remaining++;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            Matrix tMatrix = Matrix.CreateScale(.5f, .5f, .5f);
            spriteBatch.Begin( SpriteSortMode.Deferred, null, null, null, null, null, tMatrix); 
            if (needMapRender)
            {
                //map = MapRenderer.Render(board, spriteBatch);
                needMapRender = false;
            }
            //mapPos.X = board.Width * Tile.tileSize - Camera.Location.X;
            //mapPos.Y = board.Height * Tile.tileSize - Camera.Location.Y;
            var source = new Rectangle((int)Camera.Location.X, (int)Camera.Location.Y,
                GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //source.Offset(source.Width, source.Height);
            var Dest = new Rectangle(0,0,
                GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //spriteBatch.Draw(map, Dest, source, Color.White);
            /*Vector2 firstSquare = new Vector2(Camera.Location.X / Tile.tileSize, Camera.Location.Y / Tile.tileSize);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2(Camera.Location.X % Tile.tileSize, Camera.Location.Y % Tile.tileSize);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            for (int y = 0; y < height; y++)//Draw all of the map tiles
            {
                for (int x = 0; x < width; x++)
                {
                    if (y + firstY < board.Rows.Count && x + firstX < board.Rows[y+firstY].Columns.Count)//We are set to render off screen to make scrolling smooth, so make sure there is something to render before trying
                    {
                        if (board.Rows[y + firstY].Columns[x + firstX].TileID == 0)//Currently can only draw a floor or wall. Will need to change to use the cell's texture so a bunch of if statements can be avoided when adding other tile types.
                        {
                            spriteBatch.Draw(
                            wall,
                            new Rectangle((x * Tile.tileSize) - offsetX, (y * Tile.tileSize) - offsetY, Tile.tileSize, Tile.tileSize),
                            Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(
                            floor,
                            new Rectangle((x * Tile.tileSize) - offsetX, (y * Tile.tileSize) - offsetY, Tile.tileSize, Tile.tileSize),
                            Color.White);
                        } 
                    }
                    
                }
            }*/
            foreach (Entities.Entity e in board.ents)//Draw all the entities
            {
                e.self.DrawPosition = new Vector2((e.loc.X), (e.loc.Y));
                e.Draw(spriteBatch);
                /*spriteBatch.Draw(
                gobtex,
                new Vector2((e.loc.X - Tile.tileSize / 2) - Camera.Location.X, (e.loc.Y - Tile.tileSize / 2) - Camera.Location.Y),
                Color.White);*/
            }
            Player.self.DrawPosition = new Vector2((Player.loc.X), (Player.loc.Y));//Set the player's draw position
            //Player.Draw(spriteBatch);//Draw the player
            /*spriteBatch.Draw(
                playertex,
                new Vector2((Player.loc.X - Tile.tileSize/2) - Camera.Location.X,(Player.loc.Y - Tile.tileSize/2) - Camera.Location.Y),
                Color.White);*/
            string rem = string.Format("Remaining Skeletons: {0}", remaining);
            //spriteBatch.DrawString(spriteFont, rem, new Vector2(33, 99), Color.Black);
            //spriteBatch.DrawString(spriteFont, rem, new Vector2(32, 98), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public SpriteFont spriteFont { get; set; }
        private Random rnd = new Random();
        private Color[] Colors = new Color[] { Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.Indigo, Color.Purple };

        public Color RandomColor()
        {
            return Colors[rnd.Next(Colors.Length)];
        }
    }

}
