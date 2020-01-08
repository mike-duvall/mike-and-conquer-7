using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.util;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class MapTileInstanceView
    {
        public SingleTextureSprite singleTextureSprite;
        public MapTileInstance myMapTileInstance;

        // TODO Refactor handling of map shroud masks.  Consider pulling out everything into separate class(es)
        private static Texture2D visibleMask = null;
        private PartiallyVisibileMapTileMask partiallyVisibileMapTileMask;
        private static Vector2 middleOfSpriteInSpriteCoordinates;

        private int imageIndex;
        private string textureKey;

        private Texture2D mapTileBorder;
        private Texture2D mapTileBlockingTerrainBorder;

        private List<MapTileShroudMapping> mapTileShroudMappingList;


        public MapTileInstanceView(MapTileInstance aMapTileInstance)
        {
            this.myMapTileInstance = aMapTileInstance;
            imageIndex = myMapTileInstance.ImageIndex;
            textureKey = myMapTileInstance.TextureKey;
            List<MapTileFrame> mapTileFrameList =
                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
            this.singleTextureSprite = new SingleTextureSprite(mapTileFrameList[imageIndex].Texture);


            InitializeVisibleMask(mapTileFrameList);

            partiallyVisibileMapTileMask = new PartiallyVisibileMapTileMask();

            mapTileBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                Color.White,
                this.singleTextureSprite.Width,
                this.singleTextureSprite.Height);

            mapTileBlockingTerrainBorder = TextureUtil.CreateSpriteBorderRectangleTexture(
                new Color(127, 255, 255, 255),
                this.singleTextureSprite.Width,
                this.singleTextureSprite.Height);


            InitializeMapTileShroudMappingList();

        }

        private void InitializeVisibleMask(List<MapTileFrame> mapTileFrameList)
        {
            if (MapTileInstanceView.visibleMask == null)
            {
                visibleMask = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice,
                    mapTileFrameList[imageIndex].Texture.Width, mapTileFrameList[imageIndex].Texture.Height);

                int numPixels = mapTileFrameList[imageIndex].Texture.Width *
                                mapTileFrameList[imageIndex].Texture.Height;

                Color[] textureData = new Color[numPixels];
                visibleMask.GetData(textureData);

                for (int i = 0; i < numPixels; i++)
                {
                    Color xnaColor = Color.Transparent;
                    textureData[i] = xnaColor;
                }

                visibleMask.SetData(textureData);
                middleOfSpriteInSpriteCoordinates = new Vector2();

                middleOfSpriteInSpriteCoordinates.X = visibleMask.Width / 2;
                middleOfSpriteInSpriteCoordinates.Y = visibleMask.Height / 2;
            }
        }


//        internal int GetPaletteIndexOfCoordinate(int x, int y)
//        {
//            List<MapTileFrame> mapTileFrameList =
//                MikeAndConquerGame.instance.SpriteSheet.GetMapTileFrameForTmpFile(textureKey);
//            MapTileFrame mapTileFrame = mapTileFrameList[imageIndex];
//            byte[] frameData = mapTileFrame.FrameData;
//
//            int frameDataIndex = y * singleTextureSprite.Width + x;
//            return frameData[frameDataIndex];
//        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            singleTextureSprite.Draw(
                gameTime,
                spriteBatch,
                this.myMapTileInstance.PositionInWorldCoordinates,
                SpriteSortLayers.MAP_SQUARE_DEPTH,
                false,
                Color.White);

            if (GameOptions.DRAW_TERRAIN_BORDER)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBorder, this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White,
                    0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }

            if (GameOptions.DRAW_BLOCKING_TERRAIN_BORDER && myMapTileInstance.IsBlockingTerrain)
            {
                float defaultScale = 1;
                float layerDepth = 0;
                spriteBatch.Draw(mapTileBlockingTerrainBorder, this.myMapTileInstance.PositionInWorldCoordinates, null,
                    Color.White, 0f, middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, layerDepth);
            }
        }



        // TODO:  Consider pulling map shroud code into seprate class(es)
        private void InitializeMapTileShroudMappingList()
        {
            mapTileShroudMappingList = new List<MapTileShroudMapping>();

            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                0));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                1));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                2));

            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                3));

            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                4));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                5));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                6));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                7));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                8));

            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                8));



            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.Visible,
                9));


            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                9));

            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                10));

            mapTileShroudMappingList.Add(new MapTileShroudMapping(
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.NotVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                MapTileInstance.MapTileVisibility.PartiallyVisible,
                11));


        }

        //        private int FindMapTileShroudMapping(MapTileInstance.MapTileVisibility east,
        //            MapTileInstance.MapTileVisibility south,
        //            MapTileInstance.MapTileVisibility west,
        //            MapTileInstance.MapTileVisibility north)
        //        {
        //            foreach (MapTileShroudMapping mapping in mapTileShroudMappingList)
        //            {
        //                if (mapping.east == east &&
        //                    mapping.south == south &&
        //                    mapping.west == west &&
        //                    mapping.north == north)
        //                {
        //                    return mapping.shroudTileIndex;
        //                }
        //            }
        //
        //
        //            //            throw new Exception("Didn't find match");
        //            MikeAndConquerGame.instance.log.Information("Didn't find MapTileShroudMapping for: east:" + east + ", south:" + south + ", west:" + west + ", north:" + north );
        //
        //            return 1;
        //
        //
        //        }
        //


        private bool VisibilityMatches(Nullable<MapTileInstance.MapTileVisibility> expectedVisibility,
            Nullable<MapTileInstance.MapTileVisibility> actualVisibility)
        {
            if (!expectedVisibility.HasValue)
            {
                return true;
            }

            return expectedVisibility == actualVisibility;
        }


        private int FindMapTileShroudMapping(MapTileInstance.MapTileVisibility east,
            MapTileInstance.MapTileVisibility south,
            MapTileInstance.MapTileVisibility west,
            MapTileInstance.MapTileVisibility north,
            MapTileInstance.MapTileVisibility northEast,
            MapTileInstance.MapTileVisibility southEast)
        {
            foreach (MapTileShroudMapping mapping in mapTileShroudMappingList)
            {
                if (mapping.east == east &&
                    mapping.south == south &&
                    mapping.west == west &&
                    mapping.north == north &&
                    VisibilityMatches(mapping.northEast,northEast ) &&
                    VisibilityMatches(mapping.southEast, southEast))
                {
                    return mapping.shroudTileIndex;
                }
            }


            //            throw new Exception("Didn't find match");
            String message = "Didn't find MapTileShroudMapping for: east:" + east
                            + ", south:" + south
                            + ", west:" + west
                            + ", north:" + north
                            + ", northEast:" + northEast
                            + ", southEast:" + southEast;
            MikeAndConquerGame.instance.log.Information(message);

            return 1;


        }



        private int DeterminePartiallyVisibleMaskTile()
        {
            int verticalOffset = GameWorld.MAP_TILE_HEIGHT / 2 + 2;
            int horizontalOffset = GameWorld.MAP_TILE_WIDTH / 2 + 2;
            MapTileInstance south = GameWorld.instance.FindMapSquareAllowNull(
                (int) myMapTileInstance.PositionInWorldCoordinates.X,
                (int) (myMapTileInstance.PositionInWorldCoordinates.Y + verticalOffset));
            MapTileInstance.MapTileVisibility belowVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (south != null)
            {
                belowVisibility = south.Visibility;
            }


            MapTileInstance north = GameWorld.instance.FindMapSquare(
                (int) myMapTileInstance.PositionInWorldCoordinates.X,
                (int) (myMapTileInstance.PositionInWorldCoordinates.Y - verticalOffset));


            MapTileInstance west = GameWorld.instance.FindMapSquare(
                (int) myMapTileInstance.PositionInWorldCoordinates.X - horizontalOffset,
                (int) (myMapTileInstance.PositionInWorldCoordinates.Y));


            MapTileInstance east = GameWorld.instance.FindMapSquareAllowNull(
                (int) myMapTileInstance.PositionInWorldCoordinates.X + horizontalOffset,
                (int) (myMapTileInstance.PositionInWorldCoordinates.Y));
            MapTileInstance.MapTileVisibility rightVisibility = MapTileInstance.MapTileVisibility.NotVisible;
            if (east != null)
            {
                rightVisibility = east.Visibility;
            }

            MapTileInstance northEast = GameWorld.instance.FindMapSquareAllowNull(
                (int)myMapTileInstance.PositionInWorldCoordinates.X + horizontalOffset,
                (int)(myMapTileInstance.PositionInWorldCoordinates.Y - verticalOffset));

            MapTileInstance southEast = GameWorld.instance.FindMapSquareAllowNull(
                (int)myMapTileInstance.PositionInWorldCoordinates.X + horizontalOffset,
                (int)(myMapTileInstance.PositionInWorldCoordinates.Y + verticalOffset));


            return FindMapTileShroudMapping(
                rightVisibility, 
                belowVisibility, 
                west.Visibility, 
                north.Visibility,
                northEast.Visibility,
                southEast.Visibility);

        }

        internal void DrawVisbilityMask(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float defaultScale = 1;

            if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.Visible)
            {
                spriteBatch.Draw(visibleMask, this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
            else if (this.myMapTileInstance.Visibility == MapTileInstance.MapTileVisibility.PartiallyVisible)
            {
                int index = DeterminePartiallyVisibleMaskTile();
                spriteBatch.Draw(partiallyVisibileMapTileMask.GetMask(index),
                    this.myMapTileInstance.PositionInWorldCoordinates, null, Color.White, 0f,
                    middleOfSpriteInSpriteCoordinates, defaultScale, SpriteEffects.None, 1.0f);
            }
        }
    }
}