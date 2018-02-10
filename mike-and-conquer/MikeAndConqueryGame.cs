using System.Collections.Generic;


using PlayerIndex = Microsoft.Xna.Framework.PlayerIndex;
using Game = Microsoft.Xna.Framework.Game;
using GamePad = Microsoft.Xna.Framework.Input.GamePad;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using Math = System.Math;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

namespace mike_and_conquer
{

    public class MikeAndConqueryGame : Game
    {
        public static MikeAndConqueryGame instance;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D minigunnerTexture;
        Texture2D selectionCursorTexture;
        List<Minigunner> minigunnerList;

        internal Minigunner GetGdiMinigunner()
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in minigunnerList)
            {
                foundMinigunner = nextMinigunner;
            }

            return foundMinigunner;
        }



        public MikeAndConqueryGame()
        {
            graphics = new GraphicsDeviceManager(this);


            bool makeFullscreen = true;
            //bool makeFullscreen = false;
            if (makeFullscreen)
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = 1920;
                graphics.PreferredBackBufferHeight = 1080;
            }
            else
            {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 1024;

            }


            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.IsFixedTimeStep = false;

            minigunnerList = new List<Minigunner>();

            MikeAndConqueryGame.instance = this;
        }

        internal Minigunner AddGdiMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new Minigunner(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, minigunnerTexture, x, y);
            minigunnerList.Add(newMinigunner);
            return newMinigunner;
        }


        internal Texture2D loadTextureFromShpFile(string shpFileName,int indexOfFrameToLoad)
        {
            //if (loader.IsShpTD(stream))
            //{
            //    frames = null;
            //    return false;
            //}
            //loader.TryParseSprite(stream, out frames);

            int[] remap = { };

            OpenRA.Graphics.ImmutablePalette palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);

            System.IO.FileStream shpStream = System.IO.File.Open(shpFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader loader = new OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader();
            OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite shpTDSprite = new OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite(shpStream);

            OpenRA.Graphics.ISpriteFrame frame = shpTDSprite.Frames[indexOfFrameToLoad];
            byte[] frameData = frame.Data;

            Texture2D texture2D = new Texture2D(this.GraphicsDevice, shpTDSprite.Size.Width, shpTDSprite.Size.Height);
            int numPixels = texture2D.Width * texture2D.Height;
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                uint paletteX = palette[frameData[i]];
                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)palette[frameData[i]]);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                texturePixelData[i] = xnaColor;
            }
            texture2D.SetData(texturePixelData);
            return texture2D;

        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //loadMinigunnerTexture();
            //loadSelectionCursorTexture();

            minigunnerTexture = loadTextureFromShpFile("Content\\e1.shp", 0);
            selectionCursorTexture = loadTextureFromShpFile("Content\\select.shp", 0);

            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            foreach (Minigunner nextMinigunner in minigunnerList)
            {
                nextMinigunner.Update(gameTime);
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);

            foreach (Minigunner nextMinigunner in minigunnerList)
            {
                nextMinigunner.Draw(gameTime, spriteBatch);
            }

            Vector2 plottedPosition = new Vector2();
            plottedPosition.X = (float)Math.Round(200f);
            plottedPosition.Y = (float)Math.Round(200f);
            //plottedPosition.X = (float)Math.Round(position.X);
            //plottedPosition.Y = (float)Math.Round(position.Y);


            //            Debug.WriteLine("x,y=" + plottedPosition.X + "," + plottedPosition.Y);

            float scale = 5f;
            spriteBatch.Draw(selectionCursorTexture, plottedPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
