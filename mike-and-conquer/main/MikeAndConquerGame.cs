
using System.Collections.Generic;
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

using Serilog;




namespace mike_and_conquer
{

    public class MikeAndConquerGame : Game
    {

        private float testRotation = 0;
        public Camera2D camera2D;

        public static MikeAndConquerGame instance;

        public GameWorld gameWorld;

        private List<MinigunnerView> gdiMinigunnerViewList;
        private List<MinigunnerView> nodMinigunnerViewList;

        private List<BasicMapSquare> basicMapSquareList;

        private List<SandbagView> sandbagViewList;

        private GameStateView currentGameStateView;

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

        private GameMap gameMap;


        KeyboardState oldKeyboardState;

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
                //                graphics.PreferredBackBufferWidth = 1920;
                //                graphics.PreferredBackBufferHeight = 1080;
                graphics.PreferredBackBufferWidth = 2880;
                graphics.PreferredBackBufferHeight = 1800;

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

            gameWorld = new GameWorld();


            basicMapSquareList = new List<BasicMapSquare>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            sandbagViewList = new List<SandbagView>();

            textureListMap = new TextureListMap();

            oldKeyboardState = Keyboard.GetState();

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
//            this.camera2D.Zoom = 30.8f;
            this.camera2D.Zoom = 4.8f;
            //            this.camera2D.Zoom = 2.0f;
            this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(calculateLeftmostScrollX(), calculateTopmostScrollY());

            // base.Initialize() calls MikeAndConquerGame.LoadContent()
            base.Initialize();

            gameWorld.Initialize(this.gameMap.numColumns, this.gameMap.numRows);

            if (!testMode)
            {
                bool aiIsOn = false;
//                AddNodMinigunner(310, 10, aiIsOn);
//                AddNodMinigunner(315, 30, aiIsOn);

//                AddGdiMinigunner(10, 300);
//                AddGdiMinigunner(30, 300);

//                Point minigunnerStartPosition = new Point(60,12);
                Point minigunnerStartPosition = new Point(24, 24);
                AddGdiMinigunner(minigunnerStartPosition);


                AddGdiMinigunner(new Point(64, 64));
                AddGdiMinigunner(new Point(132, 64));
                AddGdiMinigunner(new Point(64, 132));


                int mapX = 1;
                int mapY = 1;

                AddSandbag(1, 1, 10);
                AddSandbag(2, 1, 10);
                AddSandbag(3, 1, 10);
                AddSandbag(4, 1, 10);

                AddSandbag(1, 2, 5);
                AddSandbag(1, 3, 5);
                AddSandbag(4, 2, 5);
                AddSandbag(4, 3, 5);

            }

            InitializeMap();



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

            int startX = 36;
            int startY = 39;
            int endX = 61;
            int endY = 61;

//            int numColumns = endX - startX + 1;
//            int numRows = endY - startY + 1;

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

            this.gameWorld.Update(gameTime);

            this.camera2D.Rotation = testRotation;
            //            testRotation += 0.01f;


            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

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

            else if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
            {
                int newX = (int)(this.camera2D.Location.X + scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
            {
                int newX = (int)(this.camera2D.Location.X - scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
            {

                int newY = (int)(this.camera2D.Location.Y + scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
            {
                int newY = (int)(this.camera2D.Location.Y - scrollAmount);
                this.camera2D.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemPlus) && newKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                float newZoom = this.camera2D.Zoom + 0.2f;
                this.camera2D.Zoom = newZoom;
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemMinus) && newKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                float newZoom = this.camera2D.Zoom - 0.2f;
                this.camera2D.Zoom = newZoom;
            }

            resetCamera();
            oldKeyboardState = newKeyboardState;

            SwitchToNewGameStateViewIfNeeded();

            base.Update(gameTime);
        }

        private void SwitchToNewGameStateViewIfNeeded()
        {
            GameState currentGameState = this.gameWorld.GetCurrentGameState();
            if (currentGameState.GetType().Equals(typeof(PlayingGameState)))
            {
//                currentGameStateView = new PlayingGameStateView();
                HandleSwitchToPlayingGameStateView();
            }
            else if (currentGameState.GetType().Equals(typeof(MissionAccomplishedGameState)))
            {
                //                currentGameStateView = new MissionAccomplishedGameStateView();
                HandleSwitchToMissionAccomplishedGameStateView();
            }
            else if (currentGameState.GetType().Equals(typeof(MissionFailedGameState)))
            {
                //                currentGameStateView = new MissionFailedGameStateView();
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


            this.currentGameStateView.Draw(gameTime, spriteBatch);

            spriteBatch.End();
           
            base.Draw(gameTime);
        }

        internal Minigunner AddGdiMinigunner(Point worldCoordinates)
        {
            Minigunner newMinigunner =  GameWorld.instance.AddGdiMinigunner(worldCoordinates);

            // TODO:  In future, decouple always adding a view when adding a minigunner
            // to enable running headless with no UI
            MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
            GdiMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
        }

        internal Sandbag AddSandbag(int x, int y, int sandbagType)
        {
            gameWorld.navigationGraph.UpdateNode(x, y, 1);
            x = x * 24 + 12;
            y = y * 24 + 12;


            Sandbag newSandbag = new Sandbag(x,y, sandbagType);
            GameWorld.instance.sandbagList.Add(newSandbag);

            SandbagView newSandbagView = new SandbagView(newSandbag);
            sandbagViewList.Add(newSandbagView);
            return newSandbag;
        }


        internal Minigunner AddNodMinigunner(int x, int y, bool aiIsOn)
        {
            Minigunner newMinigunner = gameWorld.AddNodMinigunner(x, y, aiIsOn);

            MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
            NodMinigunnerViewList.Add(newMinigunnerView);
            return newMinigunner;
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


        public GameState HandleReset()
        {

            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            sandbagViewList.Clear();

            return gameWorld.HandleReset();
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
