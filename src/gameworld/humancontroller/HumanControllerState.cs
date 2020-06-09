using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace mike_and_conquer.gameworld.humancontroller
{
    public abstract  class HumanControllerState
    {
        public abstract HumanControllerState Update(
            GameTime gameTime,
            MouseState newMouseState,
            MouseState oldMouseState);

    }
}
