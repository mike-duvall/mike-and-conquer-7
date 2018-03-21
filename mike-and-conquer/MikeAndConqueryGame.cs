using System.Collections.Generic;


using Game = Microsoft.Xna.Framework.Game;

using MouseState = Microsoft.Xna.Framework.Input.MouseState;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;

using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;


namespace mike_and_conquer
{

    public class MikeAndConqueryGame : Game
    {
        public static MikeAndConqueryGame instance;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public List<Minigunner> gdiMinigunnerList { get; }
        public List<Minigunner> nodMinigunnerList { get; }


        string Mike { get; set; }

        private MouseState oldState;
        private GameState currentGameState;


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

            gdiMinigunnerList = new List<Minigunner>();
            nodMinigunnerList = new List<Minigunner>();

            currentGameState = new PlayingGameState();

            MikeAndConqueryGame.instance = this;

        }

        internal Minigunner GetGdiMinigunner()
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                foundMinigunner = nextMinigunner;
            }

            return foundMinigunner;
        }

        internal Minigunner GetNodMinigunner()
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                foundMinigunner = nextMinigunner;
            }

            return foundMinigunner;
        }



        internal Minigunner AddGdiMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new Minigunner(x, y);
            gdiMinigunnerList.Add(newMinigunner);
            return newMinigunner;
        }


        internal Minigunner AddNodMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new NodMinigunner(x, y);
            nodMinigunnerList.Add(newMinigunner);
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

            //loadMinigunnerTexture();
            //loadSelectionCursorTexture();

            this.IsMouseVisible = true;
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
            currentGameState = currentGameState.Update(gameTime);
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


            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                nextMinigunner.Draw(gameTime, spriteBatch);
            }

            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                nextMinigunner.Draw(gameTime, spriteBatch);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }


        internal GameState GetCurrentGameState()
        {
            return currentGameState;
        }


    }
}
