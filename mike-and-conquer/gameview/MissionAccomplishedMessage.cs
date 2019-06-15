
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

//        private GameSprite missionGameSprite;
        private SingleTextureSprite missionSprite;
        private Vector2 missionPosition;

//        private GameSprite accomplishedGameSprite;
        private SingleTextureSprite accomplishedSprite;
        private Vector2 accomplishedPosition;

        public MissionAccomplishedMessage()
        {
            // Todo:  Scale to match current Zoom
            int baseX = 350;
            int baseY = 100;

            missionSprite = new SingleTextureSprite(MISSION_SPRITE_KEY);
            missionPosition = new Vector2(baseX, baseY);

            accomplishedSprite = new SingleTextureSprite(ACCOMPLISHED_SPRITE_KEY);
            accomplishedPosition = new Vector2(baseX - 5, baseY + 50);



        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            missionSprite.Draw(gameTime, spriteBatch, missionPosition);
            accomplishedSprite.Draw(gameTime, spriteBatch, accomplishedPosition);

        }
    }
}
