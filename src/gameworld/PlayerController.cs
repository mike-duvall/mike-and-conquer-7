
using Microsoft.Xna.Framework;
using mike_and_conquer.gameobjects;

namespace mike_and_conquer.gameworld
{
    public abstract class PlayerController
    {
        public abstract void Update(GameTime gameTime);


        public abstract void Add(Minigunner minigunner, bool aiIsOn);
    }
}
