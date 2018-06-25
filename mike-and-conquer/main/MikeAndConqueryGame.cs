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
using BasicMapSquare2 = mike_and_conquer.gameview.BasicMapSquare2;

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
        private List<BasicMapSquare2> basicMapSquare2List;

        public List<BasicMapSquare> BasicMapSquareList
        {
            get { return basicMapSquareList; }
        }

        public List<BasicMapSquare2> BasicMapSquare2List
        {
            get { return basicMapSquare2List; }
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
            basicMapSquare2List = new List<BasicMapSquare2>();

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


                // One line showing above and below is x = 61, y = 61



                //15, 12, 13
                int x = 60;
                int y = 60;

                BasicMapSquareList.Add(new BasicMapSquare(x, y,15));

                int x2 = x + 120;
                int y2 = y;
                BasicMapSquareList.Add(new BasicMapSquare(x2, y2, 12));

                int x3 = x + (120 * 2);
                int y3 = y;
                BasicMapSquareList.Add(new BasicMapSquare(x3, y3, 13));


                int x4 = x + (120 * 3);
                int y4 = y;
                BasicMapSquare2List.Add(new BasicMapSquare2(x4, y4, 2));

                int x5 = x + (120 * 4);
                int y5 = y;
                BasicMapSquare2List.Add(new BasicMapSquare2(x5, y5, 3));


                //Figure out how to make generic Map square where you tell key and index
                //    will need to load all shp and templates into a generic place to hold on deman
                //    Maybe one big sprite sheet?

                int currentX = x;
                y += 120 * 3;
                for(int i = 0; i < 16; i++)
                {

                    BasicMapSquareList.Add(new BasicMapSquare(x, y, i));
                    x += 121;
                    if(x > 500)
                    {
                        x = 60;
                        y += 122;
                    }
                }


                //int x3 = x;
                //int y3 = y + 120;
                //BasicMapSquareList.Add(new BasicMapSquare(x3, y3, 1));

                //for (int outer = 0; outer < 10; outer++)
                //{
                //    for (int i = 0; i < 18; i++)
                //    {
                //        BasicMapSquareList.Add(new BasicMapSquare(x, y));
                //        x += 110;
                //    }
                //    y += 110;
                //    x = 60;
                //}

                //BasicMapSquareList.Add(new BasicMapSquare(60, 60));
                //BasicMapSquareList.Add(new BasicMapSquare(180, 60));

                //                BasicMapSquareList.Add(new BasicMapSquare(100, 200));
                //                BasicMapSquareList.Add(new BasicMapSquare(220, 200));


            }

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            textureListMap.LoadSpriteListFromTmpFile(BasicMapSquare.SPRITE_KEY, BasicMapSquare.SHP_FILE_NAME, BasicMapSquare.SHP_FILE_COLOR_MAPPER);
            textureListMap.LoadSpriteListFromTmpFile(BasicMapSquare2.SPRITE_KEY, BasicMapSquare2.SHP_FILE_NAME, BasicMapSquare2.SHP_FILE_COLOR_MAPPER);

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
