using System.Collections.Generic;

using Game = Microsoft.Xna.Framework.Game;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;

using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using AsyncGameEvent = mike_and_conquer.gameevent.AsyncGameEvent;
using CreateGDIMinigunnerGameEvent = mike_and_conquer.gameevent.CreateGDIMinigunnerGameEvent;
using GetGDIMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetGDIMinigunnerByIdGameEvent;
using CreateNodMinigunnerGameEvent = mike_and_conquer.gameevent.CreateNodMinigunnerGameEvent;
using GetNodMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetNodMinigunnerByIdGameEvent;
using ResetGameGameEvent = mike_and_conquer.gameevent.ResetGameGameEvent;
using GetCurrentGameStateGameEvent = mike_and_conquer.gameevent.GetCurrentGameStateGameEvent;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using MinigunnerView = mike_and_conquer.gameview.MinigunnerView;
using GdiMinigunnerView = mike_and_conquer.gameview.GdiMinigunnerView;
using NodMinigunnerView = mike_and_conquer.gameview.NodMinigunnerView;

using BasicMapSquare = mike_and_conquer.gameview.BasicMapSquare;

using MinigunnerAIController = mike_and_conquer.aicontroller.MinigunnerAIController;





namespace mike_and_conquer
{

    public class MikeAndConqueryGame : Game
    {


        public static MikeAndConqueryGame instance;

        public List<Minigunner> gdiMinigunnerList { get; }
        public List<Minigunner> nodMinigunnerList { get; }

        public List<MinigunnerAIController> nodMinigunnerAIControllerList { get; }

        private List<MinigunnerView> gdiMinigunnerViewList;
        private List<MinigunnerView> nodMinigunnerViewList;

        private List<BasicMapSquare> basicMapSquareList;

        public List<BasicMapSquare> BasicMapSquareList
        {
            get { return basicMapSquareList; }
        }

        public List<MinigunnerView> GdiMinigunnerViewList
        {
            get { return gdiMinigunnerViewList; }
        }


        public List<MinigunnerView> NodMinigunnerViewList
        {
            get { return nodMinigunnerViewList; }
        }

        public float scale { get; }

        public TextureListMap TextureListMap
        {
            get { return textureListMap; }
        }


        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private GameState currentGameState;
        private TextureListMap textureListMap;
        private List<AsyncGameEvent> gameEvents;

        private bool testMode;


        public MikeAndConqueryGame(bool testMode)
        {
            this.testMode = testMode;
            graphics = new GraphicsDeviceManager(this);
            scale = 5f;

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


            basicMapSquareList = new List<BasicMapSquare>();

            nodMinigunnerAIControllerList = new List<MinigunnerAIController>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            currentGameState = new PlayingGameState();

            textureListMap = new TextureListMap();

            gameEvents = new List<AsyncGameEvent>();

            MikeAndConqueryGame.instance = this;
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
            this.IsMouseVisible = true;
            base.Initialize();
            if (!testMode)
            {
                //AddNodMinigunner(1100, 100);
                //AddNodMinigunner(1150, 200);

                //AddGdiMinigunner(100, 1000);
                //AddGdiMinigunner(150, 1000);



                string[,] mapTable = new string[,] {
                        {TextureListMap.CLEAR1_SHP_FILE_NAME,"15"},
                        {TextureListMap.CLEAR1_SHP_FILE_NAME,"12"},
                        {TextureListMap.CLEAR1_SHP_FILE_NAME,"13"},
                        {TextureListMap.S12_TEM,"2"},
                        {TextureListMap.S12_TEM,"3"},
                        {TextureListMap.CLEAR1_SHP_FILE_NAME,"15"},
                        {TextureListMap.CLEAR1_SHP_FILE_NAME,"12"},
                        {TextureListMap.D21_TEM,"0"},
                        {TextureListMap.D21_TEM,"1"},
                        {TextureListMap.D21_TEM,"2"},
                        //{TextureListMap.D15_TEM,"2"},
                };

                int x = 60;
                int y = 60;

                int numSquares = mapTable.GetLength(0);

                for(int i = 0; i < numSquares; i++)
                {
                    BasicMapSquareList.Add(new BasicMapSquare(x, y, mapTable[i, 0], System.Int32.Parse(mapTable[i, 1])));
                    x = x + 120;
                }

            }

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.CLEAR1_SHP_FILE_NAME);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S12_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D21_TEM);

            Line below is not working, I think because the number of tiles the file has is less than the actual number
                so may need extra parameter when loading that says how many actual tiles to expect to load

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D15_TEM);



            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureListMap.LoadSpriteListFromShpFile(GdiMinigunnerView.SPRITE_KEY, GdiMinigunnerView.SHP_FILE_NAME, GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            textureListMap.LoadSpriteListFromShpFile(NodMinigunnerView.SPRITE_KEY, GdiMinigunnerView.SHP_FILE_NAME, NodMinigunnerView.SHP_FILE_COLOR_MAPPER);

            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY, "Accomplished");
            LoadSingleTextureFromFile(gameobjects.MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");


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
            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
            {
                Program.restServer.Dispose();
                Exit();
            }
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

            currentGameState.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }





        internal Minigunner GetGdiMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.id == id)
                {
                    foundMinigunner = nextMinigunner;
                }
            }

            return foundMinigunner;
        }

        internal Minigunner GetNodMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                if (nextMinigunner.id == id)
                {
                    foundMinigunner = nextMinigunner;
                }

            }

            return foundMinigunner;
        }



        internal Minigunner AddGdiMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new Minigunner(x, y,false, this.scale);
            gdiMinigunnerList.Add(newMinigunner);

            // TODO:  In future, decouple always adding a view when adding a minigunner
            // to enable running headless with no UI
            MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
            GdiMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
        }


        internal Minigunner AddNodMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new Minigunner(x, y, true, this.scale);
            nodMinigunnerList.Add(newMinigunner);
            MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
            NodMinigunnerViewList.Add(newMinigunnerView);

            // TODO:  In future, don't couple Nod having to be AI controlled enemy
            MinigunnerAIController minigunnerAIController = new MinigunnerAIController(newMinigunner);
            nodMinigunnerAIControllerList.Add(minigunnerAIController);

            return newMinigunner;
        }



        internal void SelectSingleGDIUnit(Minigunner minigunner)
        {
            minigunner.selected = true;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
               if (nextMinigunner.id != minigunner.id)
               {
                    nextMinigunner.selected = false;
               }
           }

        }

        private void LoadSingleTextureFromFile(string key, string fileName)
        {
            SpriteTextureList spriteTextureList = new SpriteTextureList();
            Texture2D texture2D = Content.Load<Texture2D>(fileName);
            spriteTextureList.textureList.Add(texture2D);
            spriteTextureList.textureWidth = texture2D.Width;
            spriteTextureList.textureHeight = texture2D.Height;

            TextureListMap.AddTextureList(key, spriteTextureList);
        }

        internal GameState GetCurrentGameState()
        {
            return currentGameState;
        }


        public GameState HandleReset()
        {
            gdiMinigunnerList.Clear();
            nodMinigunnerList.Clear();
            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            return new PlayingGameState();
        }


        public GameState ProcessGameEvents()
        {
            GameState newGameState = null;

            lock(gameEvents)
            {
                foreach(AsyncGameEvent nextGameEvent in gameEvents)
                {
                    GameState returnedGameState = nextGameEvent.Process();
                    if (returnedGameState != null && newGameState == null)
                    {
                        newGameState = returnedGameState;
                    }
                }
                gameEvents.Clear();
            }

            return newGameState;

        }


        public Minigunner CreateGDIMinigunnerViaEvent(int x, int y)
        {
            CreateGDIMinigunnerGameEvent gameEvent = new CreateGDIMinigunnerGameEvent(x, y);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;

        }


        public Minigunner GetGDIMinigunnerByIdViaEvent(int id)
        {
            GetGDIMinigunnerByIdGameEvent gameEvent = new GetGDIMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }


        public Minigunner CreateNodMinigunnerViaEvent(int x, int y)
        {
            CreateNodMinigunnerGameEvent gameEvent = new CreateNodMinigunnerGameEvent(x, y);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner minigunner = gameEvent.GetMinigunner();
            return minigunner;

        }


        public Minigunner GetNodMinigunnerByIdViaEvent(int id)
        {
            GetNodMinigunnerByIdGameEvent gameEvent = new GetNodMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }


        public void  ResetGameViaEvent()
        {
            ResetGameGameEvent gameEvent = new ResetGameGameEvent();

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

        }

        public GameState GetCurrentGameStateViaEvent()
        {
            GetCurrentGameStateGameEvent gameEvent = new GetCurrentGameStateGameEvent();

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            return gameEvent.GetGameState();
        }

    }

}
