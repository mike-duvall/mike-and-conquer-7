﻿
using System.Collections.Generic;
using mike_and_conquer.gameview;


using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;

using OpenRA.Graphics;


namespace mike_and_conquer
{
    public class ShadowHelper
    {


        public static Color[] UpdateShadowPixels(
            int topLeftXOfSpriteInWorldCoordinates,
            int topLeftYOfSpriteInWorldCoordinates,
            Color[] texturePixelData,
            List<int> shadowIndexList,
            int width,
            ImmutablePalette palette)
        {

            foreach (int shadowIndex in shadowIndexList)
            {
                int shadowXSpriteCoordinate = shadowIndex % width;
                int shadowYSpriteCoordinate = shadowIndex / width;

                int shadowXWorldCoordinates = topLeftXOfSpriteInWorldCoordinates + shadowXSpriteCoordinate;
                int shadowYWorldCoordinate = topLeftYOfSpriteInWorldCoordinates + shadowYSpriteCoordinate;

                MapTileInstanceView underlyingMapTileInstanceView =
                    GameWorldView.instance.FindMapSquareView(shadowXWorldCoordinates,
                        shadowYWorldCoordinate);

                int halfWidth = underlyingMapTileInstanceView.singleTextureSprite.Width / 2;
                int topLeftXOfUnderlyingMapSquareWorldCoordinates = underlyingMapTileInstanceView.myMapTileInstance.GetCenter().X - halfWidth;
                int topLeftYOfUnderlyingMapSquareWorldCoordinates = underlyingMapTileInstanceView.myMapTileInstance.GetCenter().Y - halfWidth;

                int shadowXMapSquareCoordinate = shadowXWorldCoordinates - topLeftXOfUnderlyingMapSquareWorldCoordinates;
                int shadowYMapSquareCoordinate = shadowYWorldCoordinate - topLeftYOfUnderlyingMapSquareWorldCoordinates;

                int nonShadowPaletteIndexAtShadowLocation =
                    underlyingMapTileInstanceView.GetPaletteIndexOfCoordinate(shadowXMapSquareCoordinate, shadowYMapSquareCoordinate);

                //                int shadowPaletteIndex =
                //                    MikeAndConquerGame.instance.shadowMapper.MapUnitsShadowPaletteIndex(nonShadowPaletteIndexAtShadowLocation);
                //
                //                if (shadowPaletteIndex != nonShadowPaletteIndexAtShadowLocation)
                //                {
                //                    // If we found a different color for the shadow pixel (which we should)
                //                    // remap the color in the texture to be the shadow color
                //                    uint mappedColor = palette[shadowPaletteIndex];
                //                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                //                    Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                //                    texturePixelData[shadowIndex] = xnaColor;
                //                }
                //                else
                //                {
                //                    // If we didn't find a different shadow palette color, map it to bright green
                //                    // so we can see it and debug it
                //                    // TODO:  Or, consider throwing and exception and logging it
                //                    texturePixelData[shadowIndex] = new Color(255, 252, 84);
                //                }

                //                uint mappedColor = palette[5];
                //                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                //                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
//                uint mappedColor = palette[5];
//                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                Color xnaColor = new Color(255, 0,0,255);

                texturePixelData[shadowIndex] = xnaColor;

            }

            //            currentTexture.SetData(texturePixelData);
            return texturePixelData;
        }

    }



}
