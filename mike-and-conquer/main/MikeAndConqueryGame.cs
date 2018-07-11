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

using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;


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

        private GameMap gameMap;


        public MikeAndConqueryGame(bool testMode)
        {
            this.testMode = testMode;
            graphics = new GraphicsDeviceManager(this);
            scale = 2f;

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
                AddNodMinigunner(1100, 100);
                AddNodMinigunner(1150, 200);

                AddGdiMinigunner(100, 1000);
                AddGdiMinigunner(150, 1000);



                string[,] mapTable = new string[,] {
                        // 1st row
                        {TextureListMap.CLEAR1_SHP,"12"}, // 0
                        {TextureListMap.CLEAR1_SHP,"13"}, // 1
                        {TextureListMap.S12_TEM,"2"}, // 2
                        {TextureListMap.S12_TEM,"3"}, // 3
                        {TextureListMap.CLEAR1_SHP,"12"}, // 4
                        {TextureListMap.CLEAR1_SHP,"13"}, // 5
                        {TextureListMap.D21_TEM,"0"}, // 6
                        {TextureListMap.D21_TEM,"1"}, // 7
                        {TextureListMap.D21_TEM,"2"}, // 8
                        {TextureListMap.D23_TEM,"6"}, // 9
                        {TextureListMap.D23_TEM,"7"}, // 10
                        {TextureListMap.CLEAR1_SHP,"15"}, // 11
                        {TextureListMap.CLEAR1_SHP,"12"}, // 12
                        {TextureListMap.CLEAR1_SHP,"13"}, // 13
                        {TextureListMap.P07_TEM,"00"}, // 14
                        {TextureListMap.P07_TEM,"01"}, // 15
                        {TextureListMap.P07_TEM,"02"}, // 16
                        {TextureListMap.P07_TEM,"03"}, // 17
                        {TextureListMap.CLEAR1_SHP,"14"}, // 18
                        {TextureListMap.CLEAR1_SHP,"15"}, // 19
                        {TextureListMap.CLEAR1_SHP,"12"}, // 20
                        {TextureListMap.CLEAR1_SHP,"13"}, // 21
                        {TextureListMap.CLEAR1_SHP,"14"}, // 22
                        {TextureListMap.CLEAR1_SHP,"15"}, // 23
                        {TextureListMap.CLEAR1_SHP,"12"}, // 24
                        {TextureListMap.CLEAR1_SHP,"13"}, // 25

                        // 2nd row
                        {TextureListMap.CLEAR1_SHP,"00"}, // 26
                        {TextureListMap.CLEAR1_SHP,"01"}, // 27
                        {TextureListMap.S09_TEM,"00"}, // 28
                        {TextureListMap.S09_TEM,"01"}, // 29
                        {TextureListMap.S09_TEM,"02"}, // 30
                        {TextureListMap.CLEAR1_SHP,"01"}, // 31
                        {TextureListMap.D21_TEM,"03"}, // 32
                        {TextureListMap.D21_TEM,"04"}, // 33
                        {TextureListMap.D21_TEM,"05"}, // 34
                        {TextureListMap.CLEAR1_SHP,"01"}, // 35
                        {TextureListMap.CLEAR1_SHP,"02"}, // 36
                        {TextureListMap.CLEAR1_SHP,"03"}, // 37
                        {TextureListMap.CLEAR1_SHP,"00"}, // 38
                        {TextureListMap.CLEAR1_SHP,"01"}, // 39
                        {TextureListMap.P07_TEM,"04"}, // 40
                        {TextureListMap.P07_TEM,"05"}, // 41
                        {TextureListMap.P07_TEM,"06"}, // 42
                        {TextureListMap.P07_TEM,"07"}, // 43

                };

                //  (Starting at 0x13CC in the file)

                //Step 1:  Fill in whole map with repeating 4x4 default clear template
                //Step 2:  Where not FF 00 in map file, overwrite clear template with specified tiles
                //Step 3:  Figure out if this includes trees, etc, if not figure out where trees are documented
                //Step 4:  Profit

                //    Trees appear to be SHP vs TMP?
                //    Map file only references TMP ?
                //    What about placement of initial troops?
                //    Sandbags

                //    Is forumla for finding default templace image number somthing like:

                //    (row - 1) mod 4 = the row of the template from  0 to 3
                //    (column - 1) mod 4 = the column of the template from 0 to 3

                //    So the image number = (row * 4) + column 

                //    = (((row - 1) mod 4) * 4) + (column -1) mod 4

                int x = (int)(12 * this.scale);
                int y = (int)(12 * this.scale);

                //int numSquares = mapTable.GetLength(0);

                //for(int i = 0; i < numSquares; i++)
                //{
                //    BasicMapSquareList.Add(new BasicMapSquare(x, y, mapTable[i, 0], System.Int32.Parse(mapTable[i, 1])));
                //    //                    x = x + 120;
                //    x = x + (int)(24 * this.scale);

                //    if( i == 25)
                //    {
                //        x = (int)(12 * this.scale);
                //        y = y + (int)(24 * this.scale);
                //    }
                //}

                MapTile mapTile1 = gameMap.MapTiles[0];
                BasicMapSquareList.Add(new BasicMapSquare(x, y, mapTile1.textureKey, mapTile1.imageIndex));

                //MapTile needs to be changed to not have byte1 and byte2, but to instead have: textureKey, and imageIndex
                //    Write unit test to that effect

//                BasicMapSquare(int x, int y, string textureKey, int imageIndex)




            }

        }


        private void LoadMap()
        {
//           gameMap = new GameMap();

            System.IO.Stream inputStream = new FileStream("Content\\scg01ea.bin", FileMode.Open);

            // when
            int startX = 36;
            int startY = 39;
            int endX = 61;
            int endY = 61;

            int numColumns = endX - startX + 1;
            int numRows = endY - startY + 1;

            gameMap = new GameMap(inputStream, startX, startY, endX, endY);

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            LoadMap();

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.CLEAR1_SHP);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D09_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D15_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D21_TEM);
            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.D23_TEM);

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S12_TEM);

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.P07_TEM);

            textureListMap.LoadSpriteListFromTmpFile(TextureListMap.S09_TEM);


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
