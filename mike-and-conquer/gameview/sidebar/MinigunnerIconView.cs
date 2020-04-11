

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gamesprite;
using OpenRA.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class MinigunnerIconView
    {

        private ToolbarBuildIconSprite toolbarBuildIconSprite;

        public const string SPRITE_KEY = "MinigunnerIcon";

        public const string SHP_FILE_NAME = "SideBar/e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        private Point position;



        public MinigunnerIconView(Point position)
        {
            // TODO:  At some point, instead of rendering this as real color texture
            // and handling the mapping of the unit build countdown timer in the code
            // Consider creating separate shader for the toolbar to handle that
            this.position = position;
            Texture2D textureInPaletteValues =
                MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].Texture;
            Texture2D textureInRealColorValues = RenderPaletteTextureAsRealColors(textureInPaletteValues);
            toolbarBuildIconSprite =
                new ToolbarBuildIconSprite(
                    textureInRealColorValues,
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].FrameData);

        }

        private Texture2D RenderPaletteTextureAsRealColors(Texture2D textureInPaletteValues)
        {
            int[] remap = { };

            ImmutablePalette palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);

            Texture2D textureInRealColorValues = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, textureInPaletteValues.Width, textureInPaletteValues.Height);
            int numPixels = textureInRealColorValues.Width * textureInRealColorValues.Height;


            Color[] paletteTexturePixelData = new Color[numPixels];
            textureInPaletteValues.GetData(paletteTexturePixelData);

            Color[] realColorTexturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {
                float paletteIndexAsFloat = paletteTexturePixelData[i].R;
                int paletteIndex = (int) paletteIndexAsFloat;
                uint mappedColor = palette[paletteIndex];
                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);

//                    Color xnaColor = new Color(paletteIndex, 0, 0, 255);
                realColorTexturePixelData[i] = xnaColor;
            }

            textureInRealColorValues.SetData(realColorTexturePixelData);
            return textureInRealColorValues;


        }


        // This code for updating the buildIcon with percent progress shading
        // Manually sets and restore GraphicsDevice.renderTarget, so it needs to happen in the "Update" 
        // part of the loop, rather than the "Draw" part.  I initial put it in Draw and it didn't work
        public void Update(GameTime gameTime)
        {

            GDIBarracks barracks = MikeAndConquerGame.instance.gameWorld.GDIBarracks;
            if (barracks.IsBuildingMinigunner)
            {
                toolbarBuildIconSprite.isBuilding = true;
                toolbarBuildIconSprite.SetPercentBuildComplete(barracks.PercentMinigunnerBuildComplete);
            }
            else
            {
                toolbarBuildIconSprite.isBuilding = false;
            }
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // toolbarBuildIconSprite.Draw(gameTime, spriteBatch, new Vector2((toolbarBuildIconSprite.Width / 2) + 80, toolbarBuildIconSprite.Height / 2));
            toolbarBuildIconSprite.Draw(gameTime, spriteBatch, new Vector2(position.X, position.Y));
        }

        public Point GetPosition()
        {
            Point position = new Point(toolbarBuildIconSprite.Width / 2, toolbarBuildIconSprite.Height / 2);
            return position;

        }


    }
}
