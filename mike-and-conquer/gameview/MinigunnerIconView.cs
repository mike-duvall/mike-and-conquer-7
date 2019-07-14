

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class MinigunnerIconView
    {

        private ToolbarBuildIconSprite toolbarBuildIconSprite;

        public const string SPRITE_KEY = "MinigunnerIcon";

        public const string SHP_FILE_NAME = "Content\\e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public MinigunnerIconView()
        {
            toolbarBuildIconSprite =
                new ToolbarBuildIconSprite(
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].Texture,
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].FrameData);

        }


        // This code for updating the buildIcon with percent progress shading
        // Manualy sets and restore GraphicsDevice.renderTarget, so it needs to happen in the "Update" 
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
            toolbarBuildIconSprite.Draw(gameTime, spriteBatch, new Vector2(toolbarBuildIconSprite.Width / 2, toolbarBuildIconSprite.Height / 2));
        }

    }
}
