

namespace mike_and_conquer.gameview
{
    public class GdiMinigunnerView : MinigunnerView
    {

        public const string SPRITE_KEY = "GDIMinigunner";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "e1.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public GdiMinigunnerView(Minigunner minigunner) : base(minigunner, SPRITE_KEY)
        {

        }

    }
}
