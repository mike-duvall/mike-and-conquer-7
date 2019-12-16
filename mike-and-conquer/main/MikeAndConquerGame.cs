
using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gamesprite;
using mike_and_conquer.gameview;
using mike_and_conquer.openralocal;
using OpenRA.Graphics;
using Game = Microsoft.Xna.Framework.Game;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;

using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using KeyboardState = Microsoft.Xna.Framework.Input.KeyboardState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using GdiMinigunnerView = mike_and_conquer.gameview.GdiMinigunnerView;
using NodMinigunnerView = mike_and_conquer.gameview.NodMinigunnerView;
using SandbagView = mike_and_conquer.gameview.SandbagView;
using Camera2D = mike_and_conquer_6.Camera2D;

using Point = Microsoft.Xna.Framework.Point;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Serilog;

using Matrix = Microsoft.Xna.Framework.Matrix;




namespace mike_and_conquer
{

    public class MikeAndConquerGame : Game
    {


        public Viewport defaultViewport;
        private Viewport mapViewport;
        private Viewport toolbarViewport;
        private float testRotation = 0;
        public Camera2D mapViewportCamera;
        public Camera2D renderTargetCamera;
        private Camera2D toolbarViewportCamera;

        public static MikeAndConquerGame instance;

        public GameWorld gameWorld;
        private GameWorldView gameWorldView;

        public ShadowMapper shadowMapper;
        private MinigunnerIconView minigunnerIconView;

        private GameStateView currentGameStateView;

        public GameCursor gameCursor;

        public UnitSelectionBox unitSelectionBox;

        private GameState currentGameState;

        public SpriteSheet SpriteSheet
        {
            get { return spriteSheet; }
        }

        private RAISpriteFrameManager raiSpriteFrameManager;
        private SpriteSheet spriteSheet;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private bool testMode;

        private int borderSize = 0;

        private int mouseCounter = 0;

        KeyboardState oldKeyboardState;

        private Texture2D mapBackgroundRectangle;
        private Texture2D toolbarBackgroundRectangle;

        public Serilog.Core.Logger log = new LoggerConfiguration()
            //.WriteTo.Console()
            //.WriteTo.File("log.txt")
            .WriteTo.Debug()
            .CreateLogger();

        public const string CONTENT_DIRECTORY_PREFIX = "Content\\";

        private Effect mapTilePaletteMapperEffect;
        private Effect mapTileShadowMapperEffect;

        private Texture2D paletteTexture;
        private Texture2D tunitsMrfTexture;

        private Texture2D tshadow13MrfTexture;
        private Texture2D tshadow14MrfTexture;
        private Texture2D tshadow15MrfTexture;
        private Texture2D tshadow16MrfTexture;


        private RenderTarget2D mapTileRenderTarget;
        private RenderTarget2D shadowOnlyRenderTarget;
        private RenderTarget2D mapTileAndShadowsRenderTarget;
        private RenderTarget2D mapTileShadowsAndTreesRenderTarget;
        private RenderTarget2D mapTileVisibilityRenderTarget;
        private RenderTarget2D unitsAndTerrainRenderTarget;

        private bool redrawBaseMapTiles;

        public MikeAndConquerGame(bool testMode)
        {
            RemoveHostingTraceListenerToEliminateDuplicateLogEntries();

            log.Information("Hello, Serilog!");

            this.testMode = testMode;
            graphics = new GraphicsDeviceManager(this);

            if (GameOptions.IS_FULL_SCREEN)
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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
            gameWorldView = new GameWorldView();

            oldKeyboardState = Keyboard.GetState();
            unitSelectionBox = new UnitSelectionBox();

            shadowMapper = new ShadowMapper();
            currentGameState = new PlayingGameState();

            raiSpriteFrameManager = new RAISpriteFrameManager();
            spriteSheet = new SpriteSheet();

            MikeAndConquerGame.instance = this;
            redrawBaseMapTiles = true;
        }


        private void RemoveHostingTraceListenerToEliminateDuplicateLogEntries()
        {
            System.Diagnostics.Trace.Listeners.Remove("HostingTraceListener");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.InitializeDefaultMap will enumerate through any components
        /// and initialize them as well.
        /// </summary>
//        protected override void Initialize()
//        {
//            // TODO: Add your initialization logic here
//            base.InitializeDefaultMap();
//        }



        private void AddTestModeObjects()
        {
            bool aiIsOn = false;

//            AddGdiMinigunnerAtMapSquareCoordinates(new Point(21, 14));

            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 11), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(20, 11), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(21, 11), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(22, 11), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 11), MapTileInstance.MapTileVisibility.PartiallyVisible);

            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(18, 12), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 12), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(20, 12), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(21, 12), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(22, 12), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 12), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(24, 12), MapTileInstance.MapTileVisibility.PartiallyVisible);


            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(18, 13), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 13), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(20, 13), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(21, 13), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(22, 13), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 13), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(24, 13), MapTileInstance.MapTileVisibility.PartiallyVisible);

            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(18, 14), MapTileInstance.MapTileVisibility.PartiallyVisible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 14), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(20, 14), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(21, 14), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(22, 14), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 14), MapTileInstance.MapTileVisibility.Visible);
            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(24, 14), MapTileInstance.MapTileVisibility.PartiallyVisible);


            //
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 15), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 15), MapTileInstance.MapTileVisibility.Visible);
            //
            //
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 16), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 16), MapTileInstance.MapTileVisibility.Visible);
            //
            //
            //
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 17), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(20, 17), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(21, 17), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(22, 17), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 17), MapTileInstance.MapTileVisibility.Visible);
            //
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 18), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(20, 18), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(21, 18), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(22, 18), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 18), MapTileInstance.MapTileVisibility.Visible);
            //
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(19, 19), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(20, 19), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(21, 19), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(22, 19), MapTileInstance.MapTileVisibility.Visible);
            //            MakeMapSquareVisibleAtMapSquareCoorindates(new Point(23, 19), MapTileInstance.MapTileVisibility.Visible);




            //            AddGdiMinigunnerAtMapSquareCoordinates(new Point(6, 1));
            //            AddGdiMinigunnerAtMapSquareCoordinates(new Point(8, 4));


            //            AddNodMinigunnerAtMapSquareCoordinates(new Point(10, 3), aiIsOn);
            //
            //            AddSandbag(10, 6, 5);
            //            AddSandbag(10, 7, 5);
            //            AddSandbag(10, 8, 5);
            //            AddSandbag(10, 9, 5);
            //            AddSandbag(10, 10, 5);
            //
            //            AddSandbag(8, 4, 10);
            //            AddSandbag(9, 4, 10);

            //                AddSandbag(12, 16, 10);

            //                AddSandbag(11, 16, 2);
            //                AddSandbag(12, 16, 8);
            //
            //
            //                AddSandbag(14, 5, 0);
            //                AddSandbag(14, 6, 2);
            //                AddSandbag(14, 7, 8);
            minigunnerIconView = new MinigunnerIconView();
            AddGDIBarracksAtMapSquareCoordinates(new Point(20, 15));

        }



        private void SetupToolbarViewportAndCamera()
        {
            toolbarViewport = new Viewport();
            toolbarViewport.X = mapViewport.Width + 2;
            toolbarViewport.Y = 0;
            toolbarViewport.Width = defaultViewport.Width - mapViewport.Width - 5;
            toolbarViewport.Height = defaultViewport.Height;
            toolbarViewport.MinDepth = 0;
            toolbarViewport.MaxDepth = 1;

            toolbarViewportCamera = new Camera2D(toolbarViewport);
            toolbarViewportCamera.Zoom = 3.0f;
//            toolbarViewportCamera.Zoom = 1.5f;

            float scaledHalfViewportWidth = CalculateLeftmostScrollX(toolbarViewport, toolbarViewportCamera.Zoom, 0);
            float scaledHalfViewportHeight = CalculateTopmostScrollY(toolbarViewport, toolbarViewportCamera.Zoom, 0);

            toolbarViewportCamera.Location = new Vector2(scaledHalfViewportWidth, scaledHalfViewportHeight);
        }

        private void SetupMapViewportAndCamera()
        {
            mapViewport = new Viewport();
            mapViewport.X = 0;
            mapViewport.Y = 0;
            mapViewport.Width = (int)(defaultViewport.Width * 0.8f);
            mapViewport.Height = defaultViewport.Height;
            mapViewport.MinDepth = 0;
            mapViewport.MaxDepth = 1;

            this.mapViewportCamera = new Camera2D(mapViewport);

            this.mapViewportCamera.Zoom = GameOptions.INITIAL_MAP_ZOOM;
            this.mapViewportCamera.Location =
                new Vector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());

            this.renderTargetCamera = new Camera2D(mapViewport);
            this.renderTargetCamera.Zoom = 1.0f;
            this.renderTargetCamera.Location =
                new Vector2(CalculateLeftmostScrollX(mapViewport, renderTargetCamera.Zoom, borderSize), CalculateTopmostScrollY(mapViewport, renderTargetCamera.Zoom, borderSize));

        }


        private void CreateBasicMapSquareViews()
        {

            foreach(MapTileInstance mapTileInstance in this.gameWorld.gameMap.MapTileInstanceList)
            {
                gameWorldView.AddMapTileInstanceView(mapTileInstance);
            }

        }

        private void CreateTerrainItemViews()
        {

            

            foreach (TerrainItem terrainItem in gameWorld.terrainItemList)
            {
                gameWorldView.AddTerrainItemView(terrainItem);
            }

        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            gameWorld.InitializeDefaultMap();

            spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();
            CreateBasicMapSquareViews();
            CreateTerrainItemViews();

            if (!testMode)
            {
                AddTestModeObjects();
            }

            gameWorld.InitializeNavigationGraph();
            gameCursor = new GameCursor(1, 1);

            this.defaultViewport = GraphicsDevice.Viewport;
            SetupMapViewportAndCamera();
            SetupToolbarViewportAndCamera();

            toolbarBackgroundRectangle = new Texture2D(GraphicsDevice, 1, 1);
            toolbarBackgroundRectangle.SetData(new[] { Color.LightSkyBlue });

            mapBackgroundRectangle = new Texture2D(GraphicsDevice, 1, 1);
            mapBackgroundRectangle.SetData(new[] { Color.MediumSeaGreen });

            this.mapTilePaletteMapperEffect = Content.Load<Effect>("Effects\\MapTilePaletteMapperEffect");
            this.mapTileShadowMapperEffect = Content.Load<Effect>("Effects\\MapTileShadowMapperEffect");

            this.paletteTexture = new Texture2D(GraphicsDevice, 256,1);
            int[] remap = { };
            ImmutablePalette palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);
            int numPixels = 256;
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                uint mappedColor = palette[i];
                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                byte alpha = 255;
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, alpha);
                texturePixelData[i] = xnaColor;
            }
            paletteTexture.SetData(texturePixelData);


            LoadTUnitsMrfTexture();
            LoadTShadow13MrfTexture();
            LoadTShadow14MrfTexture();
            LoadTShadow15MrfTexture();
            LoadTShadow16MrfTexture();




        }

        private void LoadTUnitsMrfTexture()
        {

            int numPixels = 256;
            tunitsMrfTexture = new Texture2D(GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapUnitsShadowPaletteIndex(i);
                byte byteMrfValue = (byte) mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte) 0, (byte) 0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tunitsMrfTexture.SetData(texturePixelData);
        }

        private void LoadTShadow13MrfTexture()
        {
            int numPixels = 256;
            tshadow13MrfTexture = new Texture2D(GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile13PaletteIndex(i);
                byte byteMrfValue = (byte)mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte)0, (byte)0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow13MrfTexture.SetData(texturePixelData);
        }

        private void LoadTShadow14MrfTexture()
        {
            int numPixels = 256;
            tshadow14MrfTexture = new Texture2D(GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile14PaletteIndex(i);
                byte byteMrfValue = (byte)mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte)0, (byte)0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow14MrfTexture.SetData(texturePixelData);
        }

        private void LoadTShadow15MrfTexture()
        {
            int numPixels = 256;
            tshadow15MrfTexture = new Texture2D(GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile15PaletteIndex(i);
                byte byteMrfValue = (byte)mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte)0, (byte)0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow15MrfTexture.SetData(texturePixelData);
        }


        private void LoadTShadow16MrfTexture()
        {
            int numPixels = 256;
            tshadow16MrfTexture = new Texture2D(GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapMapTile16PaletteIndex(i);
                byte byteMrfValue = (byte)mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte)0, (byte)0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tshadow16MrfTexture.SetData(texturePixelData);
        }



        private void LoadTextures()
        {
            LoadMapTextures();
            LoadSingleTextures();
            LoadShpFileTextures();
            LoadTemFiles();
        }

        private void LoadShpFileTextures()
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GdiMinigunnerView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GdiMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GdiMinigunnerView.SHP_FILE_NAME),
                GdiMinigunnerView.SHP_FILE_COLOR_MAPPER);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                NodMinigunnerView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(NodMinigunnerView.SHP_FILE_NAME),
                NodMinigunnerView.SHP_FILE_COLOR_MAPPER);


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(SandbagView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                SandbagView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(SandbagView.SHP_FILE_NAME),
                SandbagView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(MinigunnerIconView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                MinigunnerIconView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(MinigunnerIconView.SHP_FILE_NAME),
                MinigunnerIconView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(GDIBarracksView.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                GDIBarracksView.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(GDIBarracksView.SHP_FILE_NAME),
                GDIBarracksView.SHP_FILE_COLOR_MAPPER);


            raiSpriteFrameManager.LoadAllTexturesFromShpFile(PartiallyVisibileMapTileMask.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(PartiallyVisibileMapTileMask.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(PartiallyVisibileMapTileMask.SHP_FILE_NAME),
                GDIBarracksView.SHP_FILE_COLOR_MAPPER);

            raiSpriteFrameManager.LoadAllTexturesFromShpFile(UnitSelectionCursor.SHP_FILE_NAME);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                UnitSelectionCursor.SPRITE_KEY,
                raiSpriteFrameManager.GetSpriteFramesForUnit(UnitSelectionCursor.SHP_FILE_NAME),
                UnitSelectionCursor.SHP_FILE_COLOR_MAPPER);

        }


        private void LoadTemFiles()
        {
            LoadTerrainTexture("T01.tem");
            LoadTerrainTexture("T02.tem");
            LoadTerrainTexture("T05.tem");
            LoadTerrainTexture("T06.tem");
            LoadTerrainTexture("T07.tem");
            LoadTerrainTexture("T16.tem");
            LoadTerrainTexture("T17.tem");
            LoadTerrainTexture("TC01.tem");
            LoadTerrainTexture("TC02.tem");
            LoadTerrainTexture("TC04.tem");
            LoadTerrainTexture("TC05.tem");

        }


        private void LoadTerrainTexture(String filename)
        {
            raiSpriteFrameManager.LoadAllTexturesFromShpFile(filename);
            spriteSheet.LoadUnitFramesFromSpriteFrames(
                filename,
                raiSpriteFrameManager.GetSpriteFramesForUnit(filename),
                TerrainView.SHP_FILE_COLOR_MAPPER);

        }

        private void LoadSingleTextures()
        {
            spriteSheet.LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            spriteSheet.LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY,
                "Accomplished");
            spriteSheet.LoadSingleTextureFromFile(gameobjects.MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");
            spriteSheet.LoadSingleTextureFromFile(gameobjects.DestinationSquare.SPRITE_KEY,
                gameobjects.DestinationSquare.SPRITE_KEY);
        }



        private void LoadTmpFile(string tmpFileName)
        {
            raiSpriteFrameManager.LoadAllTexturesFromTmpFile(tmpFileName);
            spriteSheet.LoadMapTileFramesFromSpriteFrames(
                tmpFileName,
                raiSpriteFrameManager.GetSpriteFramesForMapTile(tmpFileName));

        }

        private void LoadMapTextures()
        {
            LoadTmpFile(GameMap.CLEAR1_SHP);
            LoadTmpFile(GameMap.D04_TEM);
            LoadTmpFile(GameMap.D09_TEM);
            LoadTmpFile(GameMap.D13_TEM);
            LoadTmpFile(GameMap.D15_TEM);
            LoadTmpFile(GameMap.D20_TEM);
            LoadTmpFile(GameMap.D21_TEM);
            LoadTmpFile(GameMap.D23_TEM);

            LoadTmpFile(GameMap.P07_TEM);
            LoadTmpFile(GameMap.P08_TEM);

            LoadTmpFile(GameMap.S09_TEM);
            LoadTmpFile(GameMap.S10_TEM);
            LoadTmpFile(GameMap.S11_TEM);
            LoadTmpFile(GameMap.S12_TEM);
            LoadTmpFile(GameMap.S14_TEM);
            LoadTmpFile(GameMap.S22_TEM);
            LoadTmpFile(GameMap.S29_TEM);
            LoadTmpFile(GameMap.S32_TEM);
            LoadTmpFile(GameMap.S34_TEM);
            LoadTmpFile(GameMap.S35_TEM);

            LoadTmpFile(GameMap.SH1_TEM);
            LoadTmpFile(GameMap.SH2_TEM);
            LoadTmpFile(GameMap.SH3_TEM);
            LoadTmpFile(GameMap.SH4_TEM);
            LoadTmpFile(GameMap.SH5_TEM);
            LoadTmpFile(GameMap.SH6_TEM);
            LoadTmpFile(GameMap.SH9_TEM);
            LoadTmpFile(GameMap.SH10_TEM);
            LoadTmpFile(GameMap.SH17_TEM);
            LoadTmpFile(GameMap.SH18_TEM);

            LoadTmpFile(GameMap.W1_TEM);
            LoadTmpFile(GameMap.W2_TEM);


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
            int viewportWidth = mapViewport.Width;
            int halfViewportWidth = viewportWidth / 2;
            float scaledHalfViewportWidth = halfViewportWidth / mapViewportCamera.Zoom;
            return scaledHalfViewportWidth - borderSize;
        }

        // TODO Unduplicate this code?
        public float CalculateLeftmostScrollX(Viewport viewport, float zoom, int borderSize)
        {
            int viewportWidth = viewport.Width;
            int halfViewportWidth = viewportWidth / 2;
            float scaledHalfViewportWidth = halfViewportWidth / zoom;
            return scaledHalfViewportWidth - borderSize;
        }


        private float CalculateRightmostScrollX()
        {
            int widthOfMapInWorldSpace = gameWorld.gameMap.numColumns * GameWorld.MAP_TILE_WIDTH;

            int viewportWidth = mapViewport.Width;
            int halfViewportWidth = viewportWidth / 2;

            float scaledHalfViewportWidth = halfViewportWidth / mapViewportCamera.Zoom;
            float amountToScrollHorizontally = widthOfMapInWorldSpace - scaledHalfViewportWidth;
            return amountToScrollHorizontally + borderSize;
        }

        public float CalculateTopmostScrollY()
        {
            int viewportHeight = mapViewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / mapViewportCamera.Zoom;
            return scaledHalfViewportHeight - borderSize;
        }

        // TODO Unduplicate this code?
        public float CalculateTopmostScrollY(Viewport viewport, float zoom, int borderSize)
        {
            int viewportHeight = viewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / zoom;
            return scaledHalfViewportHeight - borderSize;
        }


        private float CalculateBottommostScrollY()
        {
            int heightOfMapInWorldSpace = gameWorld.gameMap.numRows * GameWorld.MAP_TILE_HEIGHT;
            int viewportHeight = mapViewport.Height;
            int halfViewportHeight = viewportHeight / 2;
            float scaledHalfViewportHeight = halfViewportHeight / mapViewportCamera.Zoom;
            float amountToScrollVertically = heightOfMapInWorldSpace - scaledHalfViewportHeight;
            return amountToScrollVertically + borderSize;
        }


        private void SnapMapCameraToBounds()
        {
            float newX = this.mapViewportCamera.Location.X;
            float newY = this.mapViewportCamera.Location.Y;

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

            this.mapViewportCamera.Location = new Vector2(newX, newY);

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
                this.mapViewportCamera.Location = new Vector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());
            }

            if (state.IsKeyDown(Keys.P))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateRightmostScrollX(), CalculateTopmostScrollY());
            }
            if (state.IsKeyDown(Keys.M))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateLeftmostScrollX(), CalculateBottommostScrollY());
            }
            if (state.IsKeyDown(Keys.OemPeriod))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateRightmostScrollX(), CalculateBottommostScrollY());
            }

            if (!oldKeyboardState.IsKeyDown(Keys.Y) && state.IsKeyDown(Keys.Y))
            {
                GameOptions.ToggleDrawTerrainBorder();
                redrawBaseMapTiles = true;
            }

            if (!oldKeyboardState.IsKeyDown(Keys.H) && state.IsKeyDown(Keys.H))
            {
                GameOptions.ToggleDrawBlockingTerrainBorder();
                redrawBaseMapTiles = true;
            }


            if (!oldKeyboardState.IsKeyDown(Keys.S) && state.IsKeyDown(Keys.S))
            {
                GameOptions.ToggleDrawShroud();
            }


            currentGameState = this.currentGameState.Update(gameTime);
            this.mapViewportCamera.Rotation = testRotation;
//                                    testRotation += 0.01f;

            // This is a hack fix to fix an issue where if you change this.IsMouseVisible to false
            // while the Windows pointer is showing the mouse pointer arrow with the blue sworl "busy" icon on the side
            // it will continue to show a frozen(non moving) copy of the blue sworl "busy" icon, even after it 
            // stops showing and updating the normal Winodws mouse pointer (in favor of my manually handled one)
            // TODO:  Investigate replacing countdown timer with direct call to (possibly to native Windows API) to determine
            // native mouse pointer "busy" status, and wait until ti goes "not busy"
            if (mouseCounter < 20)
            {
                this.IsMouseVisible = true;
                mouseCounter++;
            }
            else
            {
                this.IsMouseVisible = false;
            }


            KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            int originalX = (int)this.mapViewportCamera.Location.X;
            int originalY = (int)this.mapViewportCamera.Location.Y;

            HandleMapScrolling(originalY, originalX, newKeyboardState);
            oldKeyboardState = newKeyboardState;

            SwitchToNewGameStateViewIfNeeded();
            gameCursor.Update(gameTime);

            minigunnerIconView.Update(gameTime);
            base.Update(gameTime);
        }

        public GameState GetCurrentGameState()
        {
            return currentGameState;
        }


        private void HandleMapScrolling(int originalY, int originalX, KeyboardState newKeyboardState)
        {
            int scrollAmount = 10;
            int mouseScrollThreshold = 30;

            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            float zoomChangeAmount = 0.2f;
            if (mouseState.Position.X > defaultViewport.Width - mouseScrollThreshold)
            {
                int newX = (int) (this.mapViewportCamera.Location.X + 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.X < mouseScrollThreshold)
            {
                int newX = (int) (this.mapViewportCamera.Location.X - 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.Y > defaultViewport.Height - mouseScrollThreshold)
            {
                int newY = (int) (this.mapViewportCamera.Location.Y + 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (mouseState.Position.Y < mouseScrollThreshold)
            {
                int newY = (int) (this.mapViewportCamera.Location.Y - 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }

            else if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
            {
                int newX = (int) (this.mapViewportCamera.Location.X + scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
            {
                int newX = (int) (this.mapViewportCamera.Location.X - scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
            {
                int newY = (int) (this.mapViewportCamera.Location.Y + scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
            {
                int newY = (int) (this.mapViewportCamera.Location.Y - scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemPlus) && newKeyboardState.IsKeyDown(Keys.OemPlus))
            {
                float newZoom = this.mapViewportCamera.Zoom + zoomChangeAmount;
                this.mapViewportCamera.Zoom = newZoom;
            }
            else if (oldKeyboardState.IsKeyUp(Keys.OemMinus) && newKeyboardState.IsKeyDown(Keys.OemMinus))
            {
                float newZoom = this.mapViewportCamera.Zoom - zoomChangeAmount;
                this.mapViewportCamera.Zoom = newZoom;
            }

            SnapMapCameraToBounds();
        }

        private void SwitchToNewGameStateViewIfNeeded()
        {
            GameState currentGameState = this.GetCurrentGameState();
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

            DrawMap(gameTime);
            DrawToolbar(gameTime);
            DrawGameCursor(gameTime);

            GraphicsDevice.Viewport = defaultViewport;
            base.Draw(gameTime);


        }



        private void DrawMap(GameTime gameTime)
        {

            GraphicsDevice.Viewport = mapViewport;

            UpdateMapTileRenderTarget(gameTime);  // mapTileRenderTarget:  Just map tiles, as palette values
            UpdateShadowOnlyRenderTarget(gameTime);  // shadowOnlyRenderTarget:  shadows of units and trees, as palette values
            UpdateMapTileAndShadowsRenderTarget();  // mapTileAndShadowsRenderTarget:  Drawing mapTileRenderTarget with shadowOnlyRenderTarget shadows mapped to it, as palette values
            UpdateMapTileVisibilityRenderTarget(gameTime); // mapTileVisibilityRenderTarget
            UpdateUnitsAndTerrainRenderTarget(gameTime); //    unitsAndTerrainRenderTarget:    draw mapTileAndShadowsRenderTarget, then units and terrain
            DrawAndApplyPaletteAndMapTileVisbility();

            //            DrawMrf16Texture();
            //            DrawVisibilityMaskAsTest();
            //            DrawShadowShapeAsTest();
        }


        private void UpdateMapTileRenderTarget(GameTime gameTime)
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (mapTileRenderTarget == null)
            {
                mapTileRenderTarget = new RenderTarget2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);

            }

            if (redrawBaseMapTiles)
            {
                redrawBaseMapTiles = false;
                GraphicsDevice.SetRenderTarget(mapTileRenderTarget);
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    nullBlendState,
                    SamplerState.PointClamp,
                    nullDepthStencilState,
                    nullRasterizerState,
                    nullEffect,
                    renderTargetCamera.TransformMatrix);

                foreach (MapTileInstanceView basicMapSquareView in GameWorldView.instance.MapTileInstanceViewList)
                {
                    basicMapSquareView.Draw(gameTime, spriteBatch);
                }

                spriteBatch.End();
            }
        }

        private void UpdateShadowOnlyRenderTarget(GameTime gameTime)
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            if (shadowOnlyRenderTarget == null)
            {
                shadowOnlyRenderTarget = new RenderTarget2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            GraphicsDevice.SetRenderTarget(shadowOnlyRenderTarget);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);


            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.DrawShadowOnly(gameTime, spriteBatch);
            }

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.DrawShadowOnly(gameTime, spriteBatch);
            }

            foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            {
                nextTerrainView.DrawShadowOnly(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void UpdateMapTileAndShadowsRenderTarget()
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (mapTileAndShadowsRenderTarget == null)
            {
                mapTileAndShadowsRenderTarget = new RenderTarget2D(
                    MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            GraphicsDevice.SetRenderTarget(mapTileAndShadowsRenderTarget);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);


            mapTileShadowMapperEffect.Parameters["ShadowTexture"].SetValue(shadowOnlyRenderTarget);
            mapTileShadowMapperEffect.Parameters["UnitMrfTexture"].SetValue(tunitsMrfTexture);
            mapTileShadowMapperEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(mapTileRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);
            spriteBatch.End();
        }

        private void UpdateMapTileVisibilityRenderTarget(GameTime gameTime)
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (mapTileVisibilityRenderTarget == null)
            {
                mapTileVisibilityRenderTarget = new RenderTarget2D(
                    MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            GraphicsDevice.SetRenderTarget(mapTileVisibilityRenderTarget);

            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);


            // TODO: Consider removing this if once shroud is fully working
            if (GameOptions.DRAW_SHROUD)
            {
                foreach (MapTileInstanceView basicMapSquareView in GameWorldView.instance.MapTileInstanceViewList)
                {
                    basicMapSquareView.DrawVisbilityMask(gameTime, spriteBatch);
                }

            }


            spriteBatch.End();
        }

        private void UpdateUnitsAndTerrainRenderTarget(GameTime gameTime)
        {

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (unitsAndTerrainRenderTarget == null)
            {
                unitsAndTerrainRenderTarget = new RenderTarget2D(
                    MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            GraphicsDevice.SetRenderTarget(unitsAndTerrainRenderTarget);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);

            spriteBatch.Draw(mapTileAndShadowsRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.DrawNoShadow(gameTime, spriteBatch);
            }

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.DrawNoShadow(gameTime, spriteBatch);
            }


            foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            {
                nextTerrainView.DrawNoShadow(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void DrawAndApplyPaletteAndMapTileVisbility()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);


            mapTilePaletteMapperEffect.Parameters["PaletteTexture"].SetValue(paletteTexture);
            mapTilePaletteMapperEffect.Parameters["MapTileVisibilityTexture"].SetValue(mapTileVisibilityRenderTarget);
            mapTilePaletteMapperEffect.Parameters["DrawShroud"].SetValue(GameOptions.DRAW_SHROUD);
            mapTilePaletteMapperEffect.Parameters["Value13MrfTexture"].SetValue(tshadow13MrfTexture);
            mapTilePaletteMapperEffect.Parameters["Value14MrfTexture"].SetValue(tshadow14MrfTexture);
            mapTilePaletteMapperEffect.Parameters["Value15MrfTexture"].SetValue(tshadow15MrfTexture);
            mapTilePaletteMapperEffect.Parameters["Value16MrfTexture"].SetValue(tshadow16MrfTexture);

            mapTilePaletteMapperEffect.CurrentTechnique.Passes[0].Apply();

            spriteBatch.Draw(unitsAndTerrainRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);


            spriteBatch.End();

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);


            MikeAndConquerGame.instance.unitSelectionBox.Draw(spriteBatch);

            spriteBatch.End();

        }

        private void DrawShadowShapeAsTest()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);

            
//            spriteBatch.Draw(PartiallyVisibileMapTileMask.PartiallyVisibleMask, new Rectangle(0, 0, 24,24),
//                Color.White);
            spriteBatch.End();
        }


        private void DrawMrf16Texture()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);

            spriteBatch.Draw(tshadow16MrfTexture, new Vector2(0,0), Color.White);
            spriteBatch.End();
        }


        private void DrawVisibilityMaskAsTest()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);


            spriteBatch.Draw(mapTileVisibilityRenderTarget, new Rectangle(0, 0, mapViewport.Width, mapViewport.Height),
                Color.White);
            spriteBatch.End();
        }






        private void DrawToolbar(GameTime gameTime)
        {
            GraphicsDevice.Viewport = toolbarViewport;

            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                toolbarViewportCamera.TransformMatrix);

            spriteBatch.Draw(toolbarBackgroundRectangle,
                new Rectangle(0, 0, toolbarViewport.Width / 2, toolbarViewport.Height / 2), Color.White);

            minigunnerIconView.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void DrawGameCursor(GameTime gameTime)
        {
            GraphicsDevice.Viewport = defaultViewport;
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect);
            gameCursor.Draw(gameTime, spriteBatch);
            spriteBatch.End();


        }

        internal Minigunner AddGdiMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {

            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);
            return AddGdiMinigunner(positionInWorldCoordinates);
        }

        internal void MakeMapSquareVisibleAtMapSquareCoorindates(Point positionInMapSquareCoordinates, MapTileInstance.MapTileVisibility visibility)
        {

            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            gameWorld.MakeMapSquareVisible(positionInWorldCoordinates, visibility);

        }

        internal Minigunner AddGdiMinigunner(Point positionInWorldCoordinates)
        {

            Minigunner newMinigunner =  gameWorld.AddGdiMinigunner(positionInWorldCoordinates);

            // TODO:  In future, decouple always adding a view when adding a minigunner
            // to enable running headless with no UI
            gameWorldView.AddGDIMinigunnerView(newMinigunner);

            return newMinigunner;
        }

        internal Sandbag AddSandbag(int xInMapSquareCoordinates, int yInMapSquareCoordinates, int sandbagType)
        {

            int xInWorldCoordinates =
                (xInMapSquareCoordinates * GameWorld.MAP_TILE_WIDTH) + (GameWorld.MAP_TILE_WIDTH / 2);

            int yInWorldCoordinates =
                (yInMapSquareCoordinates * GameWorld.MAP_TILE_HEIGHT) + (GameWorld.MAP_TILE_HEIGHT / 2);

            Sandbag newSandbag = new Sandbag(xInWorldCoordinates, yInWorldCoordinates, sandbagType);
            gameWorld.sandbagList.Add(newSandbag);

            gameWorldView.AddSandbagView(newSandbag);

            return newSandbag;
        }


        public void AddGDIBarracksAtMapSquareCoordinates(Point positionInMapSquareCoordinates)
        {
            int xInWorldCoordinates = positionInMapSquareCoordinates.X * GameWorld.MAP_TILE_WIDTH;
            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * GameWorld.MAP_TILE_HEIGHT;

            Point positionInWorldCoordinates = new Point(xInWorldCoordinates, yInWorldCoordinates);

            GDIBarracks gdiBarracks = gameWorld.AddGDIBarracks(positionInWorldCoordinates);
            gameWorldView.AddGDIBarracksView(gdiBarracks);
        }



        internal Minigunner AddNodMinigunnerAtMapSquareCoordinates(Point positionInMapSquareCoordinates, bool aiIsOn)
        {

            Point positionInWorldCoordinates =
                gameWorld.ConvertWorldMapTileCoordinatesToWorldCoordinates(positionInMapSquareCoordinates);

            return AddNodMinigunner(positionInWorldCoordinates, aiIsOn);
        }


        internal Minigunner AddNodMinigunner(Point positionInWorldCoordinates, bool aiIsOn)
        {
            Minigunner newMinigunner = gameWorld.AddNodMinigunner(positionInWorldCoordinates, aiIsOn);
            gameWorldView.AddNodMinigunnerView(newMinigunner);

            return newMinigunner;
        }

        public GameState HandleReset()
        {
            GameState newGameState = gameWorld.HandleReset();
            gameWorldView.HandleReset();
            return newGameState;
        }

        public Vector2 ConvertWorldCoordinatesToScreenCoordinates(Vector2 positionInWorldCoordinates)
        {
            return Vector2.Transform(positionInWorldCoordinates, MikeAndConquerGame.instance.mapViewportCamera.TransformMatrix);
        }

        public Vector2 ConvertScreenLocationToWorldLocation(Vector2 screenLocation)
        {
            return Vector2.Transform(screenLocation, Matrix.Invert(MikeAndConquerGame.instance.mapViewportCamera.TransformMatrix));
        }

        public Vector2 ConvertScreenLocationToToolbarLocation(Vector2 screenLocation)
        {
            screenLocation.X = screenLocation.X - toolbarViewport.X;
            Vector2 result = Vector2.Transform(screenLocation, Matrix.Invert(MikeAndConquerGame.instance.toolbarViewportCamera.TransformMatrix));
            return result;
        }



        //        // TODO:  Where does this method go?
        //        public Point ConvertMapSquareCoordinatesToWorldCoordinates(Point positionInMapSquareCoordinates)
        //        {
        //
        //            int xInWorldCoordinates = positionInMapSquareCoordinates.X * 24 + 12;
        //            int yInWorldCoordinates = positionInMapSquareCoordinates.Y * 24 + 12;
        //
        //            return new Point(xInWorldCoordinates, yInWorldCoordinates);
        //        }


    }




}
