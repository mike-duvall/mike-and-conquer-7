﻿

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gameworld;
using mike_and_conquer.main;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;

using BlendState = Microsoft.Xna.Framework.Graphics.BlendState;
using DepthStencilState = Microsoft.Xna.Framework.Graphics.DepthStencilState;
using RasterizerState = Microsoft.Xna.Framework.Graphics.RasterizerState;
using Effect = Microsoft.Xna.Framework.Graphics.Effect;
using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;


using Camera2D = mike_and_conquer.main.Camera2D;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Color = Microsoft.Xna.Framework.Color;

using Viewport = Microsoft.Xna.Framework.Graphics.Viewport;
using BarracksSidebarIconView = mike_and_conquer.gameview.sidebar.BarracksSidebarIconView;
using MinigunnerSidebarIconView = mike_and_conquer.gameview.sidebar.MinigunnerSidebarIconView;

using RenderTarget2D = Microsoft.Xna.Framework.Graphics.RenderTarget2D;

using ShadowMapper = mike_and_conquer.gamesprite.ShadowMapper;

using Vector2 = Microsoft.Xna.Framework.Vector2;

using ImmutablePalette = mike_and_conquer.openra.ImmutablePalette;

using Matrix = Microsoft.Xna.Framework.Matrix;

namespace mike_and_conquer.gameview
{
    public class GameWorldView
    {

        public GameCursor gameCursor;

        public ShadowMapper shadowMapper;
        public MinigunnerSidebarIconView minigunnerSidebarIconView;
        public BarracksSidebarIconView barracksSidebarIconView;

        public float MapZoom
        {
            get { return mapViewportCamera.Zoom; }
        }

        public int ScreenHeight
        {
            get { return defaultViewport.Height; }
        }

        public int ScreenWidth
        {
            get { return defaultViewport.Width; }
        }

        private Viewport defaultViewport;
        private Camera2D mapViewportCamera;
        private Viewport sidebarViewport;  // TODO: Make this private again
        private Viewport mapViewport;
        private Camera2D renderTargetCamera;
        private Camera2D sidebarViewportCamera;

        private Texture2D sidebarBackgroundRectangle;
        private SpriteBatch spriteBatch;

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

        private Effect mapTilePaletteMapperEffect;
        private Effect mapTileShadowMapperEffect;

        private Texture2D paletteTexture;
        private Texture2D tunitsMrfTexture;

        private Texture2D mapBackgroundRectangle;

        private int borderSize = 0;

        private KeyboardState oldKeyboardState;

        private List<MinigunnerView> gdiMinigunnerViewList;
        private List<MinigunnerView> nodMinigunnerViewList;

        private GDIBarracksView gdiBarracksView;

        private GDIConstructionYardView gdiConstructionYardView;
        //        private MinigunnerSidebarIconView minigunnerSidebarIconView;

        private List<MapTileInstanceView> mapTileInstanceViewList;

        private List<SandbagView> sandbagViewList;

        private UnitSelectionBoxView unitSelectionBoxView;

        public List<MapTileInstanceView> MapTileInstanceViewList
        {
            get { return mapTileInstanceViewList; }
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

        public GDIBarracksView GDIBarracksView
        {
            get { return gdiBarracksView; }
        }

        public GDIConstructionYardView GdiConstructionYardView
        {
            get { return gdiConstructionYardView; }
        }



        public List<TerrainView> terrainViewList;

        public MCVView mcvView;

        public static GameWorldView instance;

        public BarracksPlacementIndicatorView barracksPlacementIndicatorView;


        public GameWorldView()
        {
            mapTileInstanceViewList = new List<MapTileInstanceView>();

            gdiMinigunnerViewList = new List<MinigunnerView>();
            nodMinigunnerViewList = new List<MinigunnerView>();

            sandbagViewList = new List<SandbagView>();
            terrainViewList = new List<TerrainView>();

            unitSelectionBoxView =
                new UnitSelectionBoxView(GameWorld.instance.unitSelectionBox);

            shadowMapper = new ShadowMapper();
            redrawBaseMapTiles = true;
            instance = this;

        }


        internal void Draw(GameTime gameTime)
        {

            DrawMap(gameTime);
            DrawSidebar(gameTime);
            DrawGameCursor(gameTime);


        }


        private void SetupSidebarViewportAndCamera()
        {
            sidebarViewport = new Viewport();
            sidebarViewport.X = mapViewport.Width + 2;
            sidebarViewport.Y = 0;
            sidebarViewport.Width = defaultViewport.Width - mapViewport.Width - 5;
            sidebarViewport.Height = defaultViewport.Height;
            sidebarViewport.MinDepth = 0;
            sidebarViewport.MaxDepth = 1;

            sidebarViewportCamera = new Camera2D(sidebarViewport);
            sidebarViewportCamera.Zoom = 3.0f;
            //            sidebarViewportCamera.Zoom = 1.5f;

            float scaledHalfViewportWidth = CalculateLeftmostScrollX(sidebarViewport, sidebarViewportCamera.Zoom, 0);
            float scaledHalfViewportHeight = CalculateTopmostScrollY(sidebarViewport, sidebarViewportCamera.Zoom, 0);

            sidebarViewportCamera.Location = new Vector2(scaledHalfViewportWidth, scaledHalfViewportHeight);
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
            int widthOfMapInWorldSpace = GameWorld.instance.gameMap.numColumns * GameWorld.MAP_TILE_WIDTH;

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

        private void DrawMap(GameTime gameTime)
        {

            MikeAndConquerGame.instance.GraphicsDevice.Viewport = mapViewport;

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


        private void DrawGameCursor(GameTime gameTime)
        {
            MikeAndConquerGame.instance.GraphicsDevice.Viewport = defaultViewport;
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


        private void DrawSidebar(GameTime gameTime)
        {
            MikeAndConquerGame.instance.GraphicsDevice.Viewport = sidebarViewport;

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
                sidebarViewportCamera.TransformMatrix);

            spriteBatch.Draw(sidebarBackgroundRectangle,
                new Rectangle(0, 0, sidebarViewport.Width / 2, sidebarViewport.Height / 2), Color.White);

            if (minigunnerSidebarIconView != null)
            {
                minigunnerSidebarIconView.Draw(gameTime, spriteBatch);
            }

            if (barracksSidebarIconView != null)
            {
                barracksSidebarIconView.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
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
                MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(mapTileRenderTarget);
                MikeAndConquerGame.instance.GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    nullBlendState,
                    SamplerState.PointClamp,
                    nullDepthStencilState,
                    nullRasterizerState,
                    nullEffect,
                    renderTargetCamera.TransformMatrix);

                foreach (MapTileInstanceView mapTileInstanceView in GameWorldView.instance.MapTileInstanceViewList)
                {
                    mapTileInstanceView.Draw(gameTime, spriteBatch);
                }

//                if (barracksPlacementIndicatorView != null)
//                {
//                    barracksPlacementIndicatorView.Draw(gameTime, spriteBatch);
//                }


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

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(shadowOnlyRenderTarget);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);

            if (GameWorldView.instance.GDIBarracksView != null)
            {
                GameWorldView.instance.GDIBarracksView.DrawShadowOnly(gameTime, spriteBatch);
            }

            if (GameWorldView.instance.GdiConstructionYardView != null)
            {
                GameWorldView.instance.GdiConstructionYardView.DrawShadowOnly(gameTime, spriteBatch);
            }



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

            if (GameWorldView.instance.mcvView != null)
            {
                GameWorldView.instance.mcvView.DrawShadowOnly(gameTime, spriteBatch);
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

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(mapTileAndShadowsRenderTarget);

            MikeAndConquerGame.instance.GraphicsDevice.Clear(Color.Black);

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


            // Setting blendstate to Opaque because we want the 
            // transparent pixels (alpha = 0) to be preserved in
            // mapTileVisibilityRenderTarget, because the shader
            // uses alpha to determine whether to render the underlying tile
            // Without setting it to Opaque, alpha was getting set to 0 for 
            // the pertinent pixels
            BlendState blendState = BlendState.Opaque;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            if (mapTileVisibilityRenderTarget == null)
            {
                mapTileVisibilityRenderTarget = new RenderTarget2D(
                    MikeAndConquerGame.instance.GraphicsDevice,
                    mapViewport.Width, mapViewport.Height);
            }

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(mapTileVisibilityRenderTarget);

            //            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                blendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                renderTargetCamera.TransformMatrix);

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

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(unitsAndTerrainRenderTarget);
            MikeAndConquerGame.instance.GraphicsDevice.Clear(Color.Black);
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


            if (GameWorldView.instance.GDIBarracksView != null)
            {
                GameWorldView.instance.GDIBarracksView.DrawNoShadow(gameTime, spriteBatch);
            }

            if (GameWorldView.instance.GdiConstructionYardView != null)
            {
                GameWorldView.instance.GdiConstructionYardView.DrawNoShadow(gameTime, spriteBatch);
            }




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

            if (GameWorldView.instance.mcvView != null)
            {
                GameWorldView.instance.mcvView.DrawNoShadow(gameTime, spriteBatch);
            }

            if (barracksPlacementIndicatorView != null)
            {
                barracksPlacementIndicatorView.Draw(gameTime, spriteBatch);
            }



            spriteBatch.End();
        }

        private void DrawAndApplyPaletteAndMapTileVisbility()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

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


            unitSelectionBoxView.Draw(spriteBatch);

            spriteBatch.End();

        }





        public void HandleReset()
        {
            gdiMinigunnerViewList.Clear();
            nodMinigunnerViewList.Clear();
            sandbagViewList.Clear();
            mcvView = null;
            gdiConstructionYardView = null;
            gdiBarracksView = null;
            barracksSidebarIconView = null;
            minigunnerSidebarIconView = null;
        }


        public void AddMapTileInstanceView(MapTileInstance mapTileInstance)
        {
            MapTileInstanceView mapTileInstanceView = new MapTileInstanceView(mapTileInstance);
            this.mapTileInstanceViewList.Add(mapTileInstanceView);
        }

        public void AddTerrainItemView(TerrainItem terrainItem)
        {
            TerrainView terrainView = new TerrainView(terrainItem);
            this.terrainViewList.Add(terrainView);
        }


        public void AddGDIMinigunnerView(Minigunner newMinigunner)
        {
            MinigunnerView newMinigunnerView = new GdiMinigunnerView(newMinigunner);
            gdiMinigunnerViewList.Add(newMinigunnerView);

        }

        public void AddSandbagView(Sandbag newSandbag)
        {
            SandbagView newSandbagView = new SandbagView(newSandbag);
            sandbagViewList.Add(newSandbagView);
        }

        public void AddGDIBarracksView(GDIBarracks gdiBarracks)
        {
            gdiBarracksView = new GDIBarracksView(gdiBarracks);
            minigunnerSidebarIconView = new MinigunnerSidebarIconView(new Point(112, 24));
        }

        public void AddGDIConstructionYardView(GDIConstructionYard gdiConstructionYard)
        {
            gdiConstructionYardView = new GDIConstructionYardView(gdiConstructionYard);
            barracksSidebarIconView = new BarracksSidebarIconView(new Point(32, 24));
        }



        public void AddMCVView(MCV mcv)
        {
            mcvView = new MCVView(mcv);
        }


        public void AddNodMinigunnerView(Minigunner newMinigunner)
        {
            MinigunnerView newMinigunnerView = new NodMinigunnerView(newMinigunner);
            nodMinigunnerViewList.Add(newMinigunnerView);

        }

        public MapTileInstanceView FindMapSquareView(int xWorldCoordinate, int yWorldCoordinate)
        {

            foreach (MapTileInstanceView nextBasicMapSquareView in this.mapTileInstanceViewList)
            {
                MapTileInstance mapTileInstance = nextBasicMapSquareView.myMapTileInstance;
                if (mapTileInstance.ContainsPoint(new Point(xWorldCoordinate, yWorldCoordinate)))
                {
                    return nextBasicMapSquareView;
                }
            }
            throw new Exception("Unable to find MapTileInstance at coordinates, x:" + xWorldCoordinate + ", y:" + yWorldCoordinate);

        }


        private void CreateBasicMapSquareViews()
        {
            foreach (MapTileInstance mapTileInstance in GameWorld.instance.gameMap.MapTileInstanceList)
            {
                AddMapTileInstanceView(mapTileInstance);
            }
        }

        private void CreateTerrainItemViews()
        {
            foreach (TerrainItem terrainItem in GameWorld.instance.terrainItemList)
            {
                AddTerrainItemView(terrainItem);
            }
        }


        public void LoadContent()
        {

            CreateBasicMapSquareViews();
            CreateTerrainItemViews();

            spriteBatch = new SpriteBatch(MikeAndConquerGame.instance.GraphicsDevice);
            gameCursor = new GameCursor(1, 1);

            this.defaultViewport = MikeAndConquerGame.instance.GraphicsDevice.Viewport;
            SetupMapViewportAndCamera();
            SetupSidebarViewportAndCamera();

            sidebarBackgroundRectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 1, 1);
            sidebarBackgroundRectangle.SetData(new[] { Color.LightSkyBlue });

            mapBackgroundRectangle = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 1, 1);
            mapBackgroundRectangle.SetData(new[] { Color.MediumSeaGreen });

            this.mapTilePaletteMapperEffect = MikeAndConquerGame.instance.Content.Load<Effect>("Effects\\MapTilePaletteMapperEffect");
            this.mapTileShadowMapperEffect = MikeAndConquerGame.instance.Content.Load<Effect>("Effects\\MapTileShadowMapperEffect");

            this.paletteTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
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

//            LoadTmpFile(BarracksPlacementIndicatorView.FILE_NAME);
//            barracksPlacementIndicatorView = new BarracksPlacementIndicatorView();
        }

        private void LoadTUnitsMrfTexture()
        {

            int numPixels = 256;
            tunitsMrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < 256; i++)
            {
                byte alpha = 255;
                int mrfValue = this.shadowMapper.MapUnitsShadowPaletteIndex(i);
                byte byteMrfValue = (byte)mrfValue;
                Color xnaColor = new Color(byteMrfValue, (byte)0, (byte)0, alpha);
                texturePixelData[i] = xnaColor;
            }

            tunitsMrfTexture.SetData(texturePixelData);
        }

        private void LoadTShadow13MrfTexture()
        {
            int numPixels = 256;
            tshadow13MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
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
            tshadow14MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
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
            tshadow15MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
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
            tshadow16MrfTexture = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, 256, 1);
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

        private float CalculateBottommostScrollY()
        {
            int heightOfMapInWorldSpace = GameWorld.instance.gameMap.numRows * GameWorld.MAP_TILE_HEIGHT;
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


        public void Update(GameTime gameTime, KeyboardState newKeyboardState)
        {
            if (newKeyboardState.IsKeyDown(Keys.B))
            {
                borderSize = 1;
            }
            if (newKeyboardState.IsKeyDown(Keys.N))
            {
                borderSize = 0;
            }


            if (newKeyboardState.IsKeyDown(Keys.I))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateLeftmostScrollX(), CalculateTopmostScrollY());
            }

            if (newKeyboardState.IsKeyDown(Keys.P))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateRightmostScrollX(), CalculateTopmostScrollY());
            }
            if (newKeyboardState.IsKeyDown(Keys.M))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateLeftmostScrollX(), CalculateBottommostScrollY());
            }
            if (newKeyboardState.IsKeyDown(Keys.OemPeriod))
            {
                this.mapViewportCamera.Location = new Vector2(CalculateRightmostScrollX(), CalculateBottommostScrollY());
            }

            if (!oldKeyboardState.IsKeyDown(Keys.Y) && newKeyboardState.IsKeyDown(Keys.Y))
            {
                GameOptions.ToggleDrawTerrainBorder();
                redrawBaseMapTiles = true;
            }

            if (!oldKeyboardState.IsKeyDown(Keys.H) && newKeyboardState.IsKeyDown(Keys.H))
            {
                GameOptions.ToggleDrawBlockingTerrainBorder();
                redrawBaseMapTiles = true;
            }

            if (!oldKeyboardState.IsKeyDown(Keys.S) && newKeyboardState.IsKeyDown(Keys.S))
            {
                GameOptions.ToggleDrawShroud();
            }


            // this.mapViewportCamera.Rotation = testRotation;
            //                                    testRotation += 0.01f;

            // KeyboardState newKeyboardState = Keyboard.GetState();  // get the newest state

            int originalX = (int)this.mapViewportCamera.Location.X;
            int originalY = (int)this.mapViewportCamera.Location.Y;

            HandleMapScrolling(originalY, originalX, newKeyboardState);
            oldKeyboardState = newKeyboardState;

            MikeAndConquerGame.instance.SwitchToNewGameStateViewIfNeeded();
            gameCursor.Update(gameTime);

            if (GameWorld.instance.GDIBarracks != null)
            {
                minigunnerSidebarIconView.Update(gameTime);
            }

            if (GameWorld.instance.GDIConstructionYard != null)
            {
                barracksSidebarIconView.Update(gameTime);
            }



        }


        private void HandleMapScrolling(int originalY, int originalX, KeyboardState newKeyboardState)
        {
            int scrollAmount = 10;
            int mouseScrollThreshold = 30;

            Microsoft.Xna.Framework.Input.MouseState mouseState = Microsoft.Xna.Framework.Input.Mouse.GetState();

            float zoomChangeAmount = 0.2f;
            if (mouseState.Position.X > defaultViewport.Width - mouseScrollThreshold)
            {
                int newX = (int)(this.mapViewportCamera.Location.X + 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.X < mouseScrollThreshold)
            {
                int newX = (int)(this.mapViewportCamera.Location.X - 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (mouseState.Position.Y > defaultViewport.Height - mouseScrollThreshold)
            {
                int newY = (int)(this.mapViewportCamera.Location.Y + 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (mouseState.Position.Y < mouseScrollThreshold)
            {
                int newY = (int)(this.mapViewportCamera.Location.Y - 2);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }

            else if (oldKeyboardState.IsKeyUp(Keys.Right) && newKeyboardState.IsKeyDown(Keys.Right))
            {
                int newX = (int)(this.mapViewportCamera.Location.X + scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Left) && newKeyboardState.IsKeyDown(Keys.Left))
            {
                int newX = (int)(this.mapViewportCamera.Location.X - scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(newX, originalY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Down) && newKeyboardState.IsKeyDown(Keys.Down))
            {
                int newY = (int)(this.mapViewportCamera.Location.Y + scrollAmount);
                this.mapViewportCamera.Location = new Microsoft.Xna.Framework.Vector2(originalX, newY);
            }
            else if (oldKeyboardState.IsKeyUp(Keys.Up) && newKeyboardState.IsKeyDown(Keys.Up))
            {
                int newY = (int)(this.mapViewportCamera.Location.Y - scrollAmount);
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

        private void DrawShadowShapeAsTest()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

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

            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                nullBlendState,
                SamplerState.PointClamp,
                nullDepthStencilState,
                nullRasterizerState,
                nullEffect,
                mapViewportCamera.TransformMatrix);

            spriteBatch.Draw(tshadow16MrfTexture, new Vector2(0, 0), Color.White);
            spriteBatch.End();
        }


        private void DrawVisibilityMaskAsTest()
        {
            const BlendState nullBlendState = null;
            const DepthStencilState nullDepthStencilState = null;
            const RasterizerState nullRasterizerState = null;
            const Effect nullEffect = null;


            MikeAndConquerGame.instance.GraphicsDevice.SetRenderTarget(null);

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


        public Vector2 ConvertWorldCoordinatesToScreenCoordinates(Vector2 positionInWorldCoordinates)
        {
            return Vector2.Transform(positionInWorldCoordinates, mapViewportCamera.TransformMatrix);
        }

        public Vector2 ConvertWorldCoordinatesToScreenCoordinatesForSidebar(Vector2 positionInWorldCoordinates)
        {
            // TODO:  Consider if above code could better be done with call to Viewport.Project()
            // OR, should this be done by the Camera class?
            Vector2 positionInCameraViewportCoordinates = Vector2.Transform(positionInWorldCoordinates,
                sidebarViewportCamera.TransformMatrix);
            positionInCameraViewportCoordinates.X += sidebarViewport.X;
            return positionInCameraViewportCoordinates;
        }


        public Vector2 ConvertScreenLocationToWorldLocation(Vector2 screenLocation)
        {
            return Vector2.Transform(screenLocation, Matrix.Invert(mapViewportCamera.TransformMatrix));
        }

        public Vector2 ConvertScreenLocationToSidebarLocation(Vector2 screenLocation)
        {
            screenLocation.X = screenLocation.X - sidebarViewport.X;
            Vector2 result = Vector2.Transform(screenLocation, Matrix.Invert(sidebarViewportCamera.TransformMatrix));
            return result;
        }





    }
}
