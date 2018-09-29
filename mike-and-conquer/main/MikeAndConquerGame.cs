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
using SandbagView = mike_and_conquer.gameview.SandbagView;

using BasicMapSquare = mike_and_conquer.gameview.BasicMapSquare;

using MinigunnerAIController = mike_and_conquer.aicontroller.MinigunnerAIController;

using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;

using Camera2D = mike_and_conquer_6.Camera2D;

using Point = Microsoft.Xna.Framework.Point;

using Serilog;

using Graph = mike_and_conquer.pathfinding.Graph;
using AStar = mike_and_conquer.pathfinding.AStar;

namespace mike_and_conquer
{

    public class MikeAndConquerGame : Game
    {

        private float testRotation = 0;
        public Camera2D camera2D;

        public static MikeAndConquerGame instance;

        public List<Minigunner> gdiMinigunnerList { get; }
        public List<Minigunner> nodMinigunnerList { get; }

        public List<MinigunnerAIController> nodMinigunnerAIControllerList { get; }

        private List<MinigunnerView> gdiMinigunnerViewList;
        private List<MinigunnerView> nodMinigunnerViewList;

        private List<BasicMapSquare> basicMapSquareList;

        private List<Sandbag> sandbagList;
        private List<SandbagView> sandbagViewList;


        public Graph navigationGraph;
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

        public List<SandbagView> SandbagViewList
        {
            get { return sandbagViewList; }
        }

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


        KeyboardState oldState;

        Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();


        public MikeAndConquerGame(bool testMode)
        {
            RemoveHostingTraceListenerToEliminateDuplicateLogEntries();

            log.Information("Hello, Serilog!");

            this.testMode = testMode;
            graphics = new GraphicsDeviceManager(this);
            
            bool makeFullscreen = true;
//            bool makeFullscreen = false;
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

            sandbagList = new List<Sandbag>();
            sandbagViewList = new List<SandbagView>();

            currentGameState = new PlayingGameState();

            textureListMap = new TextureListMap();

            gameEvents = new List<AsyncGameEvent>();

            oldState = Keyboard.GetState();

            MikeAndConquerGame.instance = this;
        }


        private void  RemoveHostingTraceListenerToEliminateDuplicateLogEntries()
        {
            System.Diagnostics.Trace.Listeners.Remove("HostingTraceListener");
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


            this.camera2D = new Camera2D(GraphicsDevice.Viewport);
            this.camera2D.Zoom = 3.0f;
//            this.camera2D.Zoom = 1.0f;
            this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(calculateLeftmostScrollX(), calculateTopmostScrollY());


            base.Initialize();

            if (!testMode)
            {
                bool aiIsOn = false;
                AddNodMinigunner(310, 10, aiIsOn);
                AddNodMinigunner(315, 30, aiIsOn);

                AddGdiMinigunner(10, 300);
                AddGdiMinigunner(30, 300);
                int mapX = 1;
                int mapY = 1;
                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12,10);
                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//                AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);

            }

            InitializeMap();

            InitializeNavigationGraph();

        }

        private void InitializeNavigationGraph()
        {
            
            int[,] nodeArray = new int[this.gameMap.numRows, this.gameMap.numColumns];


            int mapX = 10;
            int mapY = 7;
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);
//            AddSandbag(mapX++ * 24 + 12, mapY * 24 + 12, 10);

            // Revisit this, doesn't match actual sandbags
            // Have this be done automatically, not manually
            nodeArray[mapY, mapX++] = 1;
            nodeArray[mapY, mapX++] = 1;
            nodeArray[mapY, mapX++] = 1;
            nodeArray[mapY, mapX++] = 1;
            nodeArray[mapY, mapX++] = 1;
            nodeArray[mapY, mapX++] = 1;
            nodeArray[mapY, mapX++] = 1;
            nodeArray[mapY, mapX++] = 1;

//            navigationGraph = new Graph(nodeArray);

//            AStar aStar = new AStar();
//            aStar.FindPath(navigationGraph, )
        }

        private void InitializeMap()
        {
            //  (Starting at 0x13CC in the file)
            //    Trees appear to be SHP vs TMP?
            //    Map file only references TMP ?
            //    What about placement of initial troops?
            //    Sandbags

            int x = 12;
            int y = 12;


//            MapTile nextMapTile = gameMap.MapTiles[0];
//            BasicMapSquareList.Add(new BasicMapSquare(100,100, nextMapTile.textureKey, nextMapTile.imageIndex));
//
//            BasicMapSquare basicMapSquare2 = new BasicMapSquare(100, 130, nextMapTile.textureKey, nextMapTile.imageIndex);
//            basicMapSquare2.gameSprite.drawBoundingRectangle = false;
//            BasicMapSquareList.Add(basicMapSquare2);

            //            BasicMapSquareList.Add(new BasicMapSquare(13, 13, nextMapTile.textureKey, nextMapTile.imageIndex));

            int numSquares = gameMap.MapTiles.Count;
            for (int i = 0; i < numSquares; i++)
            {
            
                MapTile nextMapTile = gameMap.MapTiles[i];
                BasicMapSquareList.Add(new BasicMapSquare(x, y, nextMapTile.textureKey, nextMapTile.imageIndex));
            
                x = x + 24;
            
                bool incrementRow = ((i + 1) % 26) == 0;
                if (incrementRow)
                {
                    x = 12;
                    y = y + 24;
                }
            }

        }


        private void LoadMap()
        {

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

            List<string> textureKeysAlreadyAdded = new List<string>();

            foreach (MapTile nextMapTile in gameMap.MapTiles)
            {
                string textureKey = nextMapTile.textureKey;
                if(!textureKeysAlreadyAdded.Contains(textureKey))
                {
                    // TODO Remove this null check
                    if (textureKey != null)
                    {
                        textureListMap.LoadSpriteListFromTmpFile(textureKey);
                        textureKeysAlreadyAdded.Add(textureKey);
                    }
                }
            }

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textureListMap.LoadSpriteListFromShpFile(GdiMinigunnerView.SPRITE_KEY, GdiMinigunnerView.SHP_FILE_NAME, GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            textureListMap.LoadSpriteListFromShpFile(NodMinigunnerView.SPRITE_KEY, GdiMinigunnerView.SHP_FILE_NAME, NodMinigunnerView.SHP_FILE_COLOR_MAPPER);
            textureListMap.LoadSpriteListFromShpFile(SandbagView.SPRITE_KEY, SandbagView.SHP_FILE_NAME, SandbagView.SHP_FILE_COLOR_MAPPER);

            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY, "Accomplished");
            LoadSingleTextureFromFile(gameobjects.MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");
            LoadSingleTextureFromFile(gameobjects.DestinationSquare.SPRITE_KEY, gameobjects.DestinationSquare.SPRITE_KEY);


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


        private int calculateLeftmostScrollX()
        {
            return (int)((graphics.PreferredBackBufferWidth / 2 / camera2D.Zoom) - 1);
           
        }

        private int calculateRightmostScrollX()
        {
            int numSquaresWidth = 26;
            int widthOfMapSquare = 24;
            int widthOfMapInWorldSpace = numSquaresWidth * widthOfMapSquare;
            int scaledWidthOfMap = (int)(widthOfMapInWorldSpace * camera2D.Zoom);
            int amountToScrollover = scaledWidthOfMap - GraphicsDevice.Viewport.Width;

            int rightmostScrollX = calculateLeftmostScrollX() + (int)((amountToScrollover / camera2D.Zoom) + 1);

            return rightmostScrollX;
        }

        private int calculateTopmostScrollY()
        {
            return (int)((graphics.PreferredBackBufferHeight / 2 / camera2D.Zoom) - 1);
           
        }

        private int calculateBottommostScrollY()
        {
            int numSquaresWidth = 23;
            int heightOfMapSquare = 24;
            int heightOfMapInWorldSpace = numSquaresWidth * heightOfMapSquare;
            int scaledHeightOfMap = (int)(heightOfMapInWorldSpace * camera2D.Zoom);
            int amountToScrollover = scaledHeightOfMap - GraphicsDevice.Viewport.Height;

            int bottommostScrollY = calculateTopmostScrollY() + (int)((amountToScrollover / camera2D.Zoom) + 1);

            return bottommostScrollY;
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {


//            log.Information("gameTime.ElapsedGameTime.TotalMilliseconds:" + gameTime.ElapsedGameTime.TotalMilliseconds);

            KeyboardState state = Keyboard.GetState();

            // If they hit esc, exit
            if (state.IsKeyDown(Keys.Escape))
            {
                Program.restServer.Dispose();
                Exit();
            }
            currentGameState = currentGameState.Update(gameTime);

            this.camera2D.Rotation = testRotation;
            //            testRotation += 0.01f;


            KeyboardState newState = Keyboard.GetState();  // get the newest state

            int originalX = (int)this.camera2D.Location.X;
            int originalY = (int)this.camera2D.Location.Y;


            int scrollAmount = 10;

            int mouseScrollThreshold = 30;

            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            if(mouseState.Position.X > GraphicsDevice.Viewport.Width - mouseScrollThreshold)
            {
                int newX = (int)(this.camera2D.Location.X + 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.X < mouseScrollThreshold)
            {
                int newX = (int)(this.camera2D.Location.X - 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.Y  > GraphicsDevice.Viewport.Height - mouseScrollThreshold)
            {
                int newY = (int)(this.camera2D.Location.Y + 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (mouseState.Position.Y < mouseScrollThreshold)
            {
                int newY = (int)(this.camera2D.Location.Y - 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }

            else if (oldState.IsKeyUp(Keys.Right) && newState.IsKeyDown(Keys.Right))
            {
                int newX = (int)(this.camera2D.Location.X + scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldState.IsKeyUp(Keys.Left) && newState.IsKeyDown(Keys.Left))
            {
                int newX = (int)(this.camera2D.Location.X - scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down))
            {

                int newY = (int)(this.camera2D.Location.Y + scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up))
            {
                int newY = (int)(this.camera2D.Location.Y - scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldState.IsKeyUp(Keys.OemPlus) && newState.IsKeyDown(Keys.OemPlus))
            {
                float newZoom = this.camera2D.Zoom + 0.2f;
                this.camera2D.Zoom = newZoom;
            }
            else if (oldState.IsKeyUp(Keys.OemMinus) && newState.IsKeyDown(Keys.OemMinus))
            {
                float newZoom = this.camera2D.Zoom - 0.2f;
                this.camera2D.Zoom = newZoom;
            }

            resetCamera();
            oldState = newState;   
            base.Update(gameTime);
        }


        private void resetCamera()
        {
            int newX = (int)(this.camera2D.Location.X);
            int newY = (int)(this.camera2D.Location.Y);

            if (newX > calculateRightmostScrollX())
            {
                newX = calculateRightmostScrollX();
            }
            if (newX < calculateLeftmostScrollX())
            {
                newX = calculateLeftmostScrollX();
            }
            if (newY < calculateTopmostScrollY())
            {
                newY = calculateTopmostScrollY();
            }
            if (newY > calculateBottommostScrollY())
            {
                newY = calculateBottommostScrollY();
            }

            this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, newY);

        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Microsoft.Xna.Framework.Graphics.BlendState nullBlendState = null;
            Microsoft.Xna.Framework.Graphics.DepthStencilState nullDepthStencilState = null;
            Microsoft.Xna.Framework.Graphics.RasterizerState nullRasterizerState = null;
            Microsoft.Xna.Framework.Graphics.Effect nullEffect = null;
            spriteBatch.Begin(
                   SpriteSortMode.Deferred,
                   nullBlendState,
                   SamplerState.PointClamp,
                   nullDepthStencilState,
                   nullRasterizerState,
                   nullEffect,
                   camera2D.TransformMatrix);

//            spriteBatch.Begin(
//                SpriteSortMode.Deferred,
//                nullBlendState,
//                SamplerState.PointClamp,
//                nullDepthStencilState,
//                nullRasterizerState,
//                nullEffect,
//                null);

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


        internal Minigunner GetGdiOrNodMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foundMinigunner = GetGdiMinigunner(id);
            if(foundMinigunner == null)
            {
                foundMinigunner = GetNodMinigunner(id);
            }
            return foundMinigunner;
        }

        internal Minigunner AddGdiMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new Minigunner(x, y);
            gdiMinigunnerList.Add(newMinigunner);

            // TODO:  In future, decouple always adding a view when adding a minigunner
            // to enable running headless with no UI
            MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
            GdiMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
        }

        internal Sandbag AddSandbag(int x, int y, int sandbagType)
        { 
            Sandbag newSandbag = new Sandbag(x,y, sandbagType);
            sandbagList.Add(newSandbag);

            SandbagView newSandbagView = new SandbagView(newSandbag);
            sandbagViewList.Add(newSandbagView);

            return newSandbag;
        }




        internal Minigunner AddNodMinigunner(int x, int y, bool aiIsOn)
        {
            Minigunner newMinigunner = new Minigunner(x, y);
            nodMinigunnerList.Add(newMinigunner);
            MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
            NodMinigunnerViewList.Add(newMinigunnerView);

            // TODO:  In future, don't couple Nod having to be AI controlled enemy
            if (aiIsOn)
            {
                MinigunnerAIController minigunnerAIController = new MinigunnerAIController(newMinigunner);
                nodMinigunnerAIControllerList.Add(minigunnerAIController);
            }

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
            sandbagList.Clear();
            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            sandbagViewList.Clear();
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


        public Minigunner CreateNodMinigunnerViaEvent(int x, int y, bool aiIsOn)
        {
            CreateNodMinigunnerGameEvent gameEvent = new CreateNodMinigunnerGameEvent(x, y, aiIsOn);
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

        public BasicMapSquare FindMapSquare(int mouseX, int mouseY)
        {

            foreach (BasicMapSquare nextBasicMapSquare in basicMapSquareList)
            {
                if (nextBasicMapSquare.ContainsPoint(new Point(mouseX, mouseY)))
                {
                    return nextBasicMapSquare;
                }
            }
            return null;

        }
    }

}
