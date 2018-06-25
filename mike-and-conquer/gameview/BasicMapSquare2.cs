
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquare2
    {
        private GameSprite gameSprite;

        public const string SPRITE_KEY = "BasicMapSquare2";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        //        public const string SHP_FILE_NAME = "Content\\v14.tem";
        public const string SHP_FILE_NAME = "Content\\s12.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();

        Vector2 position;
        private int imageIndex;

        public BasicMapSquare2(int x, int y, int imageIndex)
        {
            this.position = new Vector2(x, y);
            this.gameSprite = new GameSprite(BasicMapSquare2.SPRITE_KEY);
            this.imageIndex = imageIndex;
            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(imageIndex);
            gameSprite.AddAnimationSequence(0, animationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameSprite.Draw(gameTime, spriteBatch, this.position);
        }




    }
}
