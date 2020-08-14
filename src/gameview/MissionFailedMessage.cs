
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using GameTime = Microsoft.Xna.Framework.GameTime;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    class MissionFailedMessage
    {

        public const string MISSION_SPRITE_KEY = "MissionMessage";
        public const string FAILED_SPRITE_KEY = "FailedMessage";

        private SingleTextureSprite missionSprite;
        private Vector2 missionPosition;

        private SingleTextureSprite accomplishedSprite;
        private Vector2 accomplishedPosition;

        public MissionFailedMessage()
        {
            int baseX = 350;
            int baseY = 100;

            missionSprite = new SingleTextureSprite(MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(MISSION_SPRITE_KEY));
            missionPosition = new Vector2(baseX, baseY);

            accomplishedSprite = new SingleTextureSprite(MikeAndConquerGame.instance.SpriteSheet.GetTextureForKey(FAILED_SPRITE_KEY));
            accomplishedPosition = new Vector2(baseX - 5, baseY + 50);

        }

//        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
//        {
//            missionSprite.Draw(gameTime, spriteBatch, missionPosition, 1.0f);
//            accomplishedSprite.Draw(gameTime, spriteBatch, accomplishedPosition, 1.0f);
//
//        }
    }
}
