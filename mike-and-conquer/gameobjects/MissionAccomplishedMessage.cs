
using GameTime = Microsoft.Xna.Framework.GameTime;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameobjects
{
    class MissionAccomplishedMessage
    {

        public const string MISSION_SPRITE_KEY = "MissionMessage";
        public const string ACCOMPLISHED_SPRITE_KEY = "AccomplishedMessage";

        private GameSprite missionGameSprite;
        private Vector2 missionPosition;

        private GameSprite accomplishedGameSprite;
        private Vector2 accomplishedPosition;

        public MissionAccomplishedMessage()
        {
            missionGameSprite = new GameSprite(MISSION_SPRITE_KEY);
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            missionGameSprite.AddAnimationSequence(0, animationSequence);
            missionGameSprite.SetCurrentAnimationSequenceIndex(0);
            missionGameSprite.Scale = 2;
            missionPosition = new Vector2(500, 400);

            accomplishedGameSprite = new GameSprite(ACCOMPLISHED_SPRITE_KEY);
            accomplishedGameSprite.AddAnimationSequence(0, animationSequence);
            accomplishedGameSprite.SetCurrentAnimationSequenceIndex(0);
            accomplishedGameSprite.Scale = 2;
            accomplishedPosition = new Vector2(600, 500);



        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            missionGameSprite.Draw(gameTime, spriteBatch, missionPosition);
            accomplishedGameSprite.Draw(gameTime, spriteBatch, accomplishedPosition);

        }
    }
}
