using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mike_and_conquer
{
    public class GdiShpFileColorMapper : ShpFileColorMapper
    {
        public override int MapColorIndex(int index)
        {
            return index;
        }
    }
}
