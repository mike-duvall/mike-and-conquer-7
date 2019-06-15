
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class MinigunnerIconView
    {
//        private GameSprite gameSprite;

        // TODO Consider ToolBarIconSprite instead of UnitSprite
        private UnitSprite unitSprite;


        public const string SPRITE_KEY = "MinigunnerIcon";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "Content\\e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


//        public GDIBarracksView(GDIBarracks barracks)
        public MinigunnerIconView()
        {
//            this.mySandbag = sandbag;
            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawShadow = true;
            SetupAnimations();
        }


        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);
            animationSequence.AddFrame(0);
            unitSprite.AddAnimationSequence(0, animationSequence);
        }


        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //            gameSprite.Draw(gameTime, spriteBatch, mySandbag.positionInWorldCoordinates);
            unitSprite.Draw(gameTime, spriteBatch, new Vector2(1,1));
        }



    }
}
