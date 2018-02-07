using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace mike_and_conquer
{

    public class MikeAndConqueryGame : Game
    {
        public static MikeAndConqueryGame instance;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D minigunnerTexture;
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
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;

            //graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;

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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Texture2D loadedTexture = this.Content.Load<Texture2D>("m3");

            int numPixels = loadedTexture.Width * loadedTexture.Height;
            uint[] loadedTexturePixelData = new uint[numPixels];

            loadedTexture.GetData<uint>(loadedTexturePixelData);


            //minigunnerTexture = new Texture2D(this.GraphicsDevice, 16, 16);
            //uint[] minigunnerTexturePixelData = new uint[numPixels];

            //for (int i = 0; i < numPixels; i++)
            //{
            //    minigunnerTexturePixelData[i] = loadedTexturePixelData[i];
            //}
            //minigunnerTexture.SetData(minigunnerTexturePixelData);



            //if (loader.IsShpTD(stream))
            //{
            //    frames = null;
            //    return false;
            //}
            //loader.TryParseSprite(stream, out frames);


            int[] remap = { };
            OpenRA.Graphics.ImmutablePalette palette = new OpenRA.Graphics.ImmutablePalette("D:\\workspace\\mike-and-conquer\\assets\\temperat.pal", remap);
            uint palette1 = palette[1];
            Color color1 = new Color(palette1);
            byte red1 = color1.R;
            byte green1 = color1.G;
            byte blue1 = color1.B;

            uint palette254 = palette[254];
            Color color254 = new Color(palette254);
            byte red254 = color254.R;
            byte green254 = color254.G;
            byte blue254 = color254.B;

            uint palette255 = palette[255];
            Color color255 = new Color(palette255);
            byte red255 = color255.R;
            byte green255 = color255.G;
            byte blue255 = color255.B;

            uint palette179 = palette[179];
            Color color179 = new Color(palette179);
            byte red179 = color179.R;
            byte green179 = color179.G;
            byte blue179 = color179.B;



            System.IO.FileStream stream = System.IO.File.Open("D:\\workspace\\mike-and-conquer\\assets\\e1.shp", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
//            OpenRA.Graphics.ISpriteFrame[] frames;
            OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader loader = new OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader();
            OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite shpTDSprite = new OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite(stream);

            OpenRA.Graphics.ISpriteFrame frame0 = shpTDSprite.Frames[0];
            byte[] frameData = frame0.Data;

            

            minigunnerTexture = new Texture2D(this.GraphicsDevice, 50, 39);
            numPixels = minigunnerTexture.Width * minigunnerTexture.Height;
            Color[] minigunnerTexturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                uint paletteX = palette[frameData[i]];
                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)palette[frameData[i]]);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                minigunnerTexturePixelData[i] = xnaColor;
            }
            minigunnerTexture.SetData(minigunnerTexturePixelData);
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

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
