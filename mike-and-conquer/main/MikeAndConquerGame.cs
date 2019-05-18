
using System;
using System.Collections.Generic;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gameview;
using Game = Microsoft.Xna.Framework.Game;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;

using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using MinigunnerView = mike_and_conquer.gameview.MinigunnerView;
using GdiMinigunnerView = mike_and_conquer.gameview.GdiMinigunnerView;
using NodMinigunnerView = mike_and_conquer.gameview.NodMinigunnerView;
using SandbagView = mike_and_conquer.gameview.SandbagView;

using BasicMapSquare = mike_and_conquer.gameview.BasicMapSquare;


using FileStream = System.IO.FileStream;
using FileMode = System.IO.FileMode;

using Camera2D = mike_and_conquer_6.Camera2D;

using Point = Microsoft.Xna.Framework.Point;
using Vector2 = Microsoft.Xna.Framework.Vector2;

using Serilog;




namespace mike_and_conquer
{

    public class MikeAndConquerGame : Game
    {

        private float testRotation = 0;
        public Camera2D camera2D;

        public static MikeAndConquerGame instance;

        public GameWorld gameWorld;

        public ShadowMapper shadowMapper;
        private List<MinigunnerView> gdiMinigunnerViewList;
        private List<MinigunnerView> nodMinigunnerViewList;

        private GDIBarracksView gdiBarracksView;
        private MinigunnerIconView minigunnerIconView;

        private List<BasicMapSquare> basicMapSquareList;

        private List<SandbagView> sandbagViewList;

        private GameStateView currentGameStateView;

        public GameCursor gameCursor;

        public UnitSelectionBox unitSelectionBox;

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
        private TextureListMap textureListMap;

        private bool testMode;

        public GameMap gameMap;

        private int borderSize = 0;

        KeyboardState oldKeyboardState;

        public Serilog.Core.Logger log = new LoggerConfiguration()
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

//            bool makeFullscreen = true;
            bool makeFullscreen = false;
            if (makeFullscreen)
            {
                graphics.IsFullScreen = true;
//                graphics.PreferredBackBufferWidth = 1920;
//                graphics.PreferredBackBufferHeight = 1080;
                graphics.PreferredBackBufferWidth = 2880;
                graphics.PreferredBackBufferHeight = 1800;

            }
            else
            {
                graphics.IsFullScreen = false;
                //                graphics.PreferredBackBufferWidth = 1280;
                //                graphics.PreferredBackBufferHeight = 1024;
                graphics.PreferredBackBufferWidth = 1024;
                graphics.PreferredBackBufferHeight = 768;

            }

            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.IsFixedTimeStep = false;

            gameWorld = new GameWorld();


            basicMapSquareList = new List<BasicMapSquare>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            sandbagViewList = new List<SandbagView>();

            textureListMap = new TextureListMap();

            oldKeyboardState = Keyboard.GetState();
            unitSelectionBox = new UnitSelectionBox();

            MikeAndConquerGame.instance = this;
        }


        private void RemoveHostingTraceListenerToEliminateDuplicateLogEntries()
        {
            System.Diagnostics.Trace.Listeners.Remove("HostingTraceListener");
        }


        public static Vector2 ConvertWorldCoordinatesToScreenCoordinates(Vector2 positionInWorldCoordinates)
        {
            return Vector2.Transform(positionInWorldCoordinates, MikeAndConquerGame.instance.camera2D.TransformMatrix);
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


            this.camera2D = new Camera2D(GraphicsDevice.Viewport);
            this.camera2D.Zoom = 3.4f;
//            this.camera2D.Zoom = 5.0f;
            this.camera2D.Location =
                new Vector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());

            base.Initialize();

            gameWorld.Initialize(this.gameMap.numColumns, this.gameMap.numRows);

            if (!testMode)
            {
                bool aiIsOn = false;

                AddGdiMinigunnerAtMapSquareCoordinates(new Point(6, 1));

                AddGdiMinigunnerAtMapSquareCoordinates(new Point(8, 3));

                AddNodMinigunnerAtMapSquareCoordinates(new Point(10, 3), aiIsOn);


                AddGdiMinigunnerAtMapSquareCoordinates(new Point(15, 16));
                AddGdiMinigunnerAtMapSquareCoordinates(new Point(16, 16));
                AddGdiMinigunnerAtMapSquareCoordinates(new Point(17, 16));
                AddGdiMinigunnerAtMapSquareCoordinates(new Point(17, 15));
                AddGdiMinigunnerAtMapSquareCoordinates(new Point(18, 16));


                AddSandbag(10, 6, 5);
                AddSandbag(10, 7, 5);
                AddSandbag(10, 8, 5);
                AddSandbag(10, 9, 5);
                AddSandbag(10, 10, 5);

                AddSandbag(8, 4, 10);
                AddSandbag(9, 4, 10);

                //                AddSandbag(12, 16, 10);

                AddSandbag(11, 16, 2);
                AddSandbag(12, 16, 8);


                AddSandbag(14, 5, 0);
                AddSandbag(14, 6, 2);
                AddSandbag(14, 7, 8);

                gdiBarracksView = new GDIBarracksView();
                minigunnerIconView = new MinigunnerIconView();
                

            }

            InitializeMap();
            InitializeNavigationGraph();
            gameCursor = new GameCursor(1,1);
            this.IsMouseVisible = false;

        }


        private void InitializeNavigationGraph()
        {
            // TODO:  Fix this.  This code should be in GameWorld, not MikeAndConquerGame
            // but has to be here for now, since BasicMapSquareList is in MikeAndConquerGame
            // Need to separate out view of BasicMapSquare into a BasicMapSquareView
            // And let GameWorld hold BasicMapSquareList(with no view data, just
            // the terrain type and whether it's blocking or not, etc

            gameWorld.navigationGraph.Reset();

            foreach (Sandbag nextSandbag in gameWorld.sandbagList)
            {
                gameWorld.navigationGraph.AddNode(nextSandbag.GetMapSquareX(), nextSandbag.GetMapSquareY(), 1);
            }


            foreach (BasicMapSquare nextBasicMapSquare in this.BasicMapSquareList)
            {
                if (nextBasicMapSquare.IsBlockingTerrain())
                {
//                    nextBasicMapSquare.gameSprite.drawBoundingRectangle = true;
                    gameWorld.navigationGraph.AddNode(nextBasicMapSquare.GetMapSquareX(), nextBasicMapSquare.GetMapSquareY(), 1);
                }
            }

            gameWorld.navigationGraph.RebuildAdajencyGraph();

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

            int numSquares = gameMap.MapTiles.Count;
            for (int i = 0; i < numSquares; i++)
            {
            
                MapTile nextMapTile = gameMap.MapTiles[i];
                BasicMapSquare basicMapSquare =
                    new BasicMapSquare(x, y, nextMapTile.textureKey, nextMapTile.imageIndex);

                BasicMapSquareList.Add(basicMapSquare);

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

            int startX = 36;
            int startY = 39;
            int endX = 61;
            int endY = 61;

            gameMap = new GameMap(inputStream, startX, startY, endX, endY);
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            LoadMap();

            shadowMapper = new ShadowMapper();

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
            textureListMap.LoadSpriteListFromShpFile(MinigunnerIconView.SPRITE_KEY, MinigunnerIconView.SHP_FILE_NAME,
                MinigunnerIconView.SHP_FILE_COLOR_MAPPER);

            textureListMap.LoadSpriteListFromShpFile(GDIBarracksView.SPRITE_KEY, GDIBarracksView.SHP_FILE_NAME, GDIBarracksView.SHP_FILE_COLOR_MAPPER);

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


        public float CalculateLeftmostScrollX()
        {
            int displayWidth = GraphicsDevice.Viewport.Width;
            int halfDisplayWidth = displayWidth / 2;
            float scaledHalfDisplayWidth = halfDisplayWidth / camera2D.Zoom;
            return scaledHalfDisplayWidth - borderSize;
        }

        private float CalculateRightmostScrollX()
        {
            int widthOfMapSquare = 24;
            int widthOfMapInWorldSpace = MikeAndConquerGame.instance.gameMap.numColumns * widthOfMapSquare;

            int displayWidth = GraphicsDevice.Viewport.Width;
            int halfDisplayWidth = displayWidth / 2;
            float scaledHalfDisplayWidth = halfDisplayWidth / camera2D.Zoom;
            float amountToScrollHorizontally = widthOfMapInWorldSpace - scaledHalfDisplayWidth;
            return amountToScrollHorizontally + borderSize;
        }

        public float CalculateTopmostScrollY()
        {
            int viewportHeight = GraphicsDevice.Viewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / camera2D.Zoom;
            return scaledHalfViewportHeight - borderSize;
        }

        private float CalculateBottommostScrollY()
        {
            int heightOfMapSquare = 24;
            int heightOfMapInWorldSpace = MikeAndConquerGame.instance.gameMap.numRows * heightOfMapSquare;
            int viewportHeight = GraphicsDevice.Viewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / camera2D.Zoom;
            float amountToScrollVertically = heightOfMapInWorldSpace - scaledHalfViewportHeight;
            return amountToScrollVertically + borderSize;
        }


        private void ResetCamera()
        {
            float newX = this.camera2D.Location.X;
            float newY = this.camera2D.Location.Y;

            // TODO:  Consider if we store these as class variables
            // and only recalculate when the zoom changes
            float rightMostScrollX = CalculateRightmostScrollX();
            float leftMostScrollX = CalculateLeftmostScrollX();
            float topmostScrollY = CalculateTopmostScrollY();
            float bottommostScrollY = CalculateBottommostScrollY();
            if (newX > rightMostScrollX)
            {
                newX = rightMostScrollX;
            }
            if (newY > bottommostScrollY)
            {
                newY = bottommostScrollY;
            }

            // Check for leftmost and topmost last, which makes it snap to top left corner
            // if zoom is such that entire map fits on current screen
            if (newX < leftMostScrollX)
            {
                newX = leftMostScrollX;
            }

            if (newY < topmostScrollY)
            {
                newY = topmostScrollY;
            }

            this.camera2D.Location = new Vector2(newX, newY);

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

            if (state.IsKeyDown(Keys.B))
            {
                borderSize = 1;
            }
            if (state.IsKeyDown(Keys.N))
            {
                borderSize = 0;
            }


            if (state.IsKeyDown(Keys.I))
            {
                this.camera2D.Location = new Vector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());
            }
            if (state.IsKeyDown(Keys.P))
            {
                this.camera2D.Location = new Vector2(CalculateRightmostScrollX(), CalculateTopmostScrollY());
            }
            if (state.IsKeyDown(Keys.M))
            {
                this.camera2D.Location = new Vector2(CalculateLeftmostScrollX(), CalculateBottommostScrollY());
            }
            if (state.IsKeyDown(Keys.OemPeriod))
            {
                this.camera2D.Location = new Vector2(CalculateRightmostScrollX(), CalculateBottommostScrollY());
            }

            this.gameWorld.Update(gameTime);
            this.camera2D.Rotation = testRotation;
//                        testRotation += 0.05f;

            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            int originalX = (int)this.camera2D.Location.X;
            int originalY = (int)this.camera2D.Location.Y;


            HandleMapScrolling(originalY, originalX, newKeyboardState);
            oldKeyboardState = newKeyboardState;

            SwitchToNewGameStateViewIfNeeded();
            gameCursor.Update(gameTime);
            base.Update(gameTime);
        }

        private void HandleMapScrolling(int originalY, int originalX, KeyboardState newKeyboardState)
        {
            int scrollAmount = 10;
            int mouseScrollThreshold = 30;

            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            float zoomChangeAmount = 0.2f;
            if (mouseState.Position.X > GraphicsDevice.Viewport.Width - mouseScrollThreshold)
            {
                int newX = (int) (this.camera2D.Location.X + 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.X < mouseScrollThreshold)
            {
                int newX = (int) (this.camera2D.Location.X - 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.Y > GraphicsDevice.Viewport.Height - mouseScrollThreshold)
            {
                int newY = (int) (this.camera2D.Location.Y + 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (mouseState.Position.Y < mouseScrollThreshold)
            {
                int newY = (int) (this.camera2D.Location.Y - 2);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }

            else if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
            {
                int newX = (int) (this.camera2D.Location.X + scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
            {
                int newX = (int) (this.camera2D.Location.X - scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
            {
                int newY = (int) (this.camera2D.Location.Y + scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
            {
                int newY = (int) (this.camera2D.Location.Y - scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemPlus) && newKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                float newZoom = this.camera2D.Zoom + zoomChangeAmount;
                this.camera2D.Zoom = newZoom;
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemMinus) && newKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                float newZoom = this.camera2D.Zoom - zoomChangeAmount;
                this.camera2D.Zoom = newZoom;
            }

            ResetCamera();
        }

        private void SwitchToNewGameStateViewIfNeeded()
        {
            GameState currentGameState = this.gameWorld.GetCurrentGameState();
            if (currentGameState.GetType().Equals(typeof(PlayingGameState)))
            {
                HandleSwitchToPlayingGameStateView();
            }
            else if (currentGameState.GetType().Equals(typeof(MissionAccomplishedGameState)))
            {
                HandleSwitchToMissionAccomplishedGameStateView();
            }
            else if (currentGameState.GetType().Equals(typeof(MissionFailedGameState)))
            {
                HandleSwitchToMissionFailedGameStateView();
            }
        }

        private void HandleSwitchToPlayingGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(PlayingGameStateView)))
            {
                currentGameStateView = new PlayingGameStateView();
            }
        }

        private void HandleSwitchToMissionAccomplishedGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionAccomplishedGameStateView)))
            {
                currentGameStateView = new MissionAccomplishedGameStateView();
            }
        }

        private void HandleSwitchToMissionFailedGameStateView()
        {
            if (currentGameStateView == null || !currentGameStateView.GetType().Equals(typeof(MissionFailedGameStateView)))
            {
                currentGameStateView = new MissionFailedGameStateView();
            }
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Crimson);

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


            this.currentGameStateView.Draw(gameTime, spriteBatch);
            gdiBarracksView.Draw(gameTime,spriteBatch);
            minigunnerIconView.Draw(gameTime, spriteBatch );
//            gameCursor.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect);

            gameCursor.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public Point ConvertMapSquareCoordinatesToWorldCoordinates(Point positionInMapSquareCoordinates)
        {

            int xInWorldCoordinates = positionInMapSquareCoordinates.X * 24 + 12;
            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * 24 + 12;

            return new Point(xInWorldCoordinates,yInWorldCoordinates);

        }


        internal Minigunner AddGdiMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            Point positionInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            return AddGdiMinigunner(positionInWorldCoordinates);
        }


        internal Minigunner AddGdiMinigunner(Point positionInWorldCoordinates)
        {

            Minigunner newMinigunner =  GameWorld.instance.AddGdiMinigunner(positionInWorldCoordinates);

            // TODO:  In future, decouple always adding a view when adding a minigunner
            // to enable running headless with no UI
            MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
            GdiMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
        }

        internal Sandbag AddSandbag(int xInMapSquareCoordinates, int yInMapSquareCoordinates, int sandbagType)
        {

            int xInWorldCoordinates = xInMapSquareCoordinates * 24 + 12;
            int yInWorldCoordinates = yInMapSquareCoordinates * 24 + 12;

            Sandbag newSandbag = new Sandbag(xInWorldCoordinates, yInWorldCoordinates, sandbagType);
            GameWorld.instance.sandbagList.Add(newSandbag);

            SandbagView newSandbagView = new SandbagView(newSandbag);
            sandbagViewList.Add(newSandbagView);
            return newSandbag;
        }

        internal Minigunner AddNodMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates, bool aiIsOn)
        {
            Point positionInWorldCoordinates =
                ConvertMapSquareCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            return AddNodMinigunner(positionInWorldCoordinates, aiIsOn);
        }



        internal Minigunner AddNodMinigunner(Point positionInWorldCoordinates, bool aiIsOn)
        {
            Minigunner newMinigunner = gameWorld.AddNodMinigunner(positionInWorldCoordinates, aiIsOn);

            MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
            NodMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
        }


        private void LoadSingleTextureFromFile(string key, string fileName)
        {
            SpriteTextureList spriteTextureList = new SpriteTextureList();
            Texture2D texture2D = Content.Load<Texture2D>(fileName);


            ShpFileImage shpFileImage = new ShpFileImage(texture2D,null,null);
            spriteTextureList.shpFileImageList.Add(shpFileImage);
            spriteTextureList.textureWidth = texture2D.Width;
            spriteTextureList.textureHeight = texture2D.Height;

            TextureListMap.AddTextureList(key, spriteTextureList);
        }


        public GameState HandleReset()
        {
            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            sandbagViewList.Clear();
            // TODO:  Bogus stuff here
            // Have to reset world first, before then resetting navigation graph
            // because navigation graph depends on what's in the game world
            // and sandbags were not getting cleared before navigation graph was updated
            GameState newGameState = gameWorld.HandleReset();
            InitializeNavigationGraph();
            return newGameState;
        }


        public BasicMapSquare FindMapSquare(int xWorldCoordinate, int yWorldCoordinate)
        {

            foreach (BasicMapSquare nextBasicMapSquare in basicMapSquareList)
            {
                if (nextBasicMapSquare.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
                {
                    return nextBasicMapSquare;
                }
            }
            throw new Exception("Unable to find BasicMapSquare at coordinates, x:" + xWorldCoordinate + ", y:" + yWorldCoordinate);

        }
    }

}
