
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using String = System.String;

using GameTime = Microsoft.Xna.Framework.GameTime;

namespace mike_and_conquer
{
    public abstract class GameState
    {
        abstract public String GetName();
        abstract public GameState Update(GameTime gameTime);
        abstract public void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}
