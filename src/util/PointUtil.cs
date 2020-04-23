
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using mike_and_conquer.main;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;

namespace mike_and_conquer.util
{
    public class PointUtil
    {

        public static Point ConvertVector2ToPoint(Vector2 aVector2)
        {
            return new Point((int) aVector2.X, (int) aVector2.Y);
        }
    }
}
