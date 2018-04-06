
namespace mike_and_conquer
{
    class GdiMinigunner : Minigunner
    {

        public const string SPRITE_KEY = "GDIMinigunner";
        public const string SHP_FILE_NAME = "Content\\e1.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        public GdiMinigunner(int x, int y) : base(x, y, GdiMinigunner.SPRITE_KEY)
        {
        }


    }



}
