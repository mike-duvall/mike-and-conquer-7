
using GameTime = Microsoft.Xna.Framework.GameTime;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameobjects
{
    class MissionAccomplishedMessage
    {

        public const string SPRITE_KEY = "MissionMessage";

        private GameSprite missionGameSprite;
        private Vector2 missionPosition;

        private GameSprite accomplishedGameSprite;

        public MissionAccomplishedMessage()
        {
            missionGameSprite = new GameSprite(SPRITE_KEY);

            AnimationSequence standingStillAnimationSequence = new AnimationSequence(10);
            standingStillAnimationSequence.AddFrame(0);
            missionGameSprite.AddAnimationSequence(0, standingStillAnimationSequence);
            missionGameSprite.SetCurrentAnimationSequenceIndex(0);
            missionGameSprite.Scale = 2;



            missionPosition = new Vector2(300, 300);
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            missionGameSprite.Draw(gameTime, spriteBatch, missionPosition);
        }
    }
}
