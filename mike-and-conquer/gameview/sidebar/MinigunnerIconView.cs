

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using mike_and_conquer.gamesprite;
using OpenRA.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview.sidebar
{
    public class MinigunnerIconView : ToolbarIconView
    {

        public const string SPRITE_KEY = "MinigunnerIcon";
        public const string SHP_FILE_NAME = "SideBar/e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();



        public MinigunnerIconView(Point position) : base(position)
        {
        }


        protected override string GetSpriteKey()
        {
            return SPRITE_KEY;
        }

        // This code for updating the buildIcon with percent progress shading
        // Manually sets and restore GraphicsDevice.renderTarget, so it needs to happen in the "Update" 
        // part of the loop, rather than the "Draw" part.  I initial put it in Draw and it didn't work
        public override void Update(GameTime gameTime)
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

    }
}
