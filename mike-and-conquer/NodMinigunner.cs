﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer
{
    class NodMinigunner : Minigunner
    {

        public const string SPRITE_KEY = "NODMinigunner";

        public NodMinigunner(int x, int y) : base(x, y, NodMinigunner.SPRITE_KEY)
        {
        }


    }



}
