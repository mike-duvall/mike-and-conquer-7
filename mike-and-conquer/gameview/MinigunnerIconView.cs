
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

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


        public void Update(GameTime gameTime)
        {

            GDIBarracks barracks = MikeAndConquerGame.instance.gameWorld.GDIBarracks;
            if (barracks.IsBuildingMinigunner)
            {
//                int percentComplete = barracks.PercentMinigunnerBuildComplete;
//                int angle = 360 * percentComplete / 100;
//                angle += 270;
//                if (angle > 360)
//                {
//                    angle -= 360;
//                }
//
                toolbarBuildIconSprite.isBuilding = true;
//                toolbarBuildIconSprite.RemapAllPixels(angle);

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
