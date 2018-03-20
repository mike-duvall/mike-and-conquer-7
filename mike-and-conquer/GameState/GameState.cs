using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace mike_and_conquer
{
    abstract class GameState
    {
        abstract public String GetName();
        abstract public GameState Update(GameTime gameTime);
    }
}
