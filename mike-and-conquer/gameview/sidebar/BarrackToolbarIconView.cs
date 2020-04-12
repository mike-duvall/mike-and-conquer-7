

using Microsoft.Xna.Framework;


namespace mike_and_conquer.gameview.sidebar
{
    public class BarracksToolbarIconView : ToolbarIconView
    {

        public const string SPRITE_KEY = "BarracksToolbarIcon";
        public const string SHP_FILE_NAME = "SideBar/pyleicnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public BarracksToolbarIconView(Point position) : base(position)
        {
        }

        protected override string GetSpriteKey()
        {
            return SPRITE_KEY;
        }

        protected override bool IsBuilding()
        {
            GDIConstructionYard constructionYard = MikeAndConquerGame.instance.gameWorld.GDIConstructionYard;
            return constructionYard.IsBuildingBarracks;
        }

        protected override int PercentBuildCompleted()
        {
            GDIConstructionYard constructionYard = MikeAndConquerGame.instance.gameWorld.GDIConstructionYard;
            return constructionYard.PercentBarracksBuildComplete;
        }


    }
}
