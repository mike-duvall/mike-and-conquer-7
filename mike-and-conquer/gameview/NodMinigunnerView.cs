using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer.gameview
{
    public class NodMinigunnerView : MinigunnerView
    {

        public const string SPRITE_KEY = "NODMinigunner";
        public const string SHP_FILE_NAME = "Content\\e1.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new NodShpFileColorMapper();


        public NodMinigunnerView(Minigunner minigunner) : base(minigunner, SPRITE_KEY)
        {

        }

    }
}
