
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using Point = Microsoft.Xna.Framework.Point;


namespace mike_and_conquer.gameview
{
    public class GDIBarracksView
    {
        private GameSprite gameSprite;
//        private Sandbag mySandbag;

        private Point position;


        public const string SPRITE_KEY = "Barracks";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "Content\\pyle.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


//        public GDIBarracksView(GDIBarracks barracks)
        public GDIBarracksView(Point aPosition)
        {
//            this.mySandbag = sandbag;
            this.gameSprite = new GameSprite(SPRITE_KEY);
            this.gameSprite.drawShadow = true;
            this.position = aPosition;
            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            gameSprite.AddAnimationSequence(0, animationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
//            gameSprite.Draw(gameTime, spriteBatch, mySandbag.positionInWorldCoordinates);
            gameSprite.Draw(gameTime, spriteBatch, new Vector2(this.position.X, this.position.Y));
        }



    }
}
