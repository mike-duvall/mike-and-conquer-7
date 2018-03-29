using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer
{
    class GdiMinigunner : Minigunner
    {

        public const string SPRITE_KEY = "GDIMinigunner";

        public GdiMinigunner(int x, int y) : base(x, y, GdiMinigunner.SPRITE_KEY)
        {
        }


    }



}
