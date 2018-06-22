
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Math = System.Math;

namespace mike_and_conquer.gameview
{
    public class BasicMapSquare
    {
        private GameSprite gameSprite;

        public const string SPRITE_KEY = "BasicMapSquare";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
//        public const string SHP_FILE_NAME = "Content\\v14.tem";
        public const string SHP_FILE_NAME = "Content\\clear1.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        Vector2 position;

        public  BasicMapSquare(int x, int y )
        {

            this.position = new Vector2(x,y);
            this.gameSprite = new GameSprite(BasicMapSquare.SPRITE_KEY);
            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(50);
            animationSequence.AddFrame(1);
            animationSequence.AddFrame(2);
            animationSequence.AddFrame(3);
            animationSequence.AddFrame(4);
            animationSequence.AddFrame(5);
            animationSequence.AddFrame(6);
            animationSequence.AddFrame(7);
            animationSequence.AddFrame(8);
            animationSequence.AddFrame(9);
            animationSequence.AddFrame(10);
            animationSequence.AddFrame(11);
            animationSequence.AddFrame(12);
            animationSequence.AddFrame(13);
            animationSequence.AddFrame(14);

            gameSprite.AddAnimationSequence(0, animationSequence);

        }




        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            gameSprite.Draw(gameTime, spriteBatch, this.position);
        }




    }
}
