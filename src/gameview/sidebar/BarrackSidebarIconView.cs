

using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;

using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview.sidebar
{
    public class BarracksSidebarIconView : SidebarIconView
    {

        public const string SPRITE_KEY = "BarracksSidebarIcon";
        public const string SHP_FILE_NAME = "SideBar/pyleicnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public BarracksSidebarIconView(Point position) : base(position)
        {
        }

        protected override string GetSpriteKey()
        {
            return SPRITE_KEY;
        }

        protected override bool IsBuilding()
        {
            // TODO:  Make GDIConstructionYard and Barrackas implement Buildable interface?
            // With methods for IsBulding() and PercentBuildCompleted() ?
            GDIConstructionYard constructionYard = MikeAndConquerGame.instance.gameWorld.GDIConstructionYard;
            return constructionYard.IsBuildingBarracks;
        }

        protected override int PercentBuildCompleted()
        {
            GDIConstructionYard constructionYard = MikeAndConquerGame.instance.gameWorld.GDIConstructionYard;
            return constructionYard.PercentBarracksBuildComplete;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime,spriteBatch);
            GDIConstructionYard constructionYard = MikeAndConquerGame.instance.gameWorld.GDIConstructionYard;
            if (constructionYard.IsBarracksReadyToPlace)
            {
                readyOverlay.Draw(gameTime, spriteBatch);
            }

        }


    }
}
