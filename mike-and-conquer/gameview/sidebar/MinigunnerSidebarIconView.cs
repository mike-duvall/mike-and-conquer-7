

using Microsoft.Xna.Framework;


namespace mike_and_conquer.gameview.sidebar
{
    public class MinigunnerSidebarIconView : SidebarIconView
    {

        public const string SPRITE_KEY = "MinigunnerIcon";
        public const string SHP_FILE_NAME = "SideBar/e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        public MinigunnerSidebarIconView(Point position) : base(position)
        {
        }

        protected override string GetSpriteKey()
        {
            return SPRITE_KEY;
        }

        protected override bool IsBuilding()
        {
            GDIBarracks barracks = MikeAndConquerGame.instance.gameWorld.GDIBarracks;
            return barracks.IsBuildingMinigunner;
        }

        protected override int PercentBuildCompleted()
        {
            GDIBarracks barracks = MikeAndConquerGame.instance.gameWorld.GDIBarracks;
            return barracks.PercentMinigunnerBuildComplete;
        }


    }
}
