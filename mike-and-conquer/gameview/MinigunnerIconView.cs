
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class MinigunnerIconView
    {

        // TODO Consider ToolBarIconSprite instead of UnitSprite
        private ToolbarBuildIconSprite toolbarBuildIconSprite;

        public const string SPRITE_KEY = "MinigunnerIcon";

//        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
//        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "Content\\e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

//        private double angleInDegrees = 315;

        public MinigunnerIconView()
        {
            toolbarBuildIconSprite =
                new ToolbarBuildIconSprite(
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].Texture,
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].FrameData);
            toolbarBuildIconSprite.isBuilding = true;
            toolbarBuildIconSprite.RemapAllPixels(315);
        }


        //            DrawLine(spriteBatch, new Vector2(33, 24), 315);  // 45 degrees further, going clockwise
        //            DrawLine(spriteBatch, new Vector2(33, 24), 360);  // 90 degrees further, going clockwise
        //            DrawLine(spriteBatch, new Vector2(33, 24), 45); // 135 degrees
        //            DrawLine(spriteBatch, new Vector2(33, 24), 90);
        //            DrawLine(spriteBatch, new Vector2(33, 24), 135);
        //            DrawLine(spriteBatch, new Vector2(33, 24), 180);
        //            DrawLine(spriteBatch, new Vector2(33, 24), 225);


        public void Update(GameTime gameTime)
        {

            GDIBarracks barracks = MikeAndConquerGame.instance.gameWorld.GDIBarracks;
            if (barracks.IsBuildingMinigunner)
            {
                int percentComplete = barracks.PercentMinigunnerBuildComplete;
                int angle = 360 * percentComplete / 100;
                angle += 270;
                if (angle > 360)
                {
                    angle -= 360;
                }
                toolbarBuildIconSprite.RemapAllPixels(angle);                

            }
            else
            {

            }


        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            toolbarBuildIconSprite.Draw(gameTime, spriteBatch, new Vector2(toolbarBuildIconSprite.Width / 2, toolbarBuildIconSprite.Height / 2));
        }



    }
}
