
using mike_and_conquer.gameobjects;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class MinigunnerIconView
    {

        // TODO Consider ToolBarIconSprite instead of UnitSprite
        private SingleTextureSprite singleTextureSprite;

        public const string SPRITE_KEY = "MinigunnerIcon";

//        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
//        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "Content\\e1icnh.tem";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        public MinigunnerIconView()
        {
            singleTextureSprite =
                new SingleTextureSprite(
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].Texture,
                    MikeAndConquerGame.instance.SpriteSheet.GetUnitFramesForShpFile(SPRITE_KEY)[0].FrameData);
            singleTextureSprite.drawCloudTexture = true;
            singleTextureSprite.RemapAllPixels();
        }




        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

//            unitSprite.Draw(gameTime, spriteBatch, new Vector2(1,1));
            singleTextureSprite.Draw(gameTime, spriteBatch, new Vector2(singleTextureSprite.Width / 2, singleTextureSprite.Height / 2));
        }



    }
}
