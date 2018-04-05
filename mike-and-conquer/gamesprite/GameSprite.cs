using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using System.Collections.Generic;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;


namespace mike_and_conquer
{
    public class GameSprite
    {

        Dictionary<int, AnimationSequence> animationSequenceMap;
        int currentAnimationSequenceIndex;

        SpriteTextureList spriteTextureList;
        Texture2D currentTexture;

        Texture2D spriteBorderRectangleTexture;
        Boolean drawBoundingRectangle;


        private int worldWidth;
        private int worldHeight;

        private Vector2 middleOfSprite;

        public int unscaledWidth;
        public int unscaledHeight;
        private float scale;


        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public GameSprite(string spriteListKey)
        {
            this.animationSequenceMap = new Dictionary<int, util.AnimationSequence>();
            this.worldWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            this.worldHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height;
            spriteTextureList = MikeAndConqueryGame.instance.TextureListMap.GetTextureList(spriteListKey);

            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSprite = new Vector2();

            middleOfSprite.X = spriteTextureList.textureWidth / 2;
            middleOfSprite.Y = spriteTextureList.textureHeight / 2;

            drawBoundingRectangle = false;
            scale = MikeAndConqueryGame.instance.scale;
        }

        public void SetCurrentAnimationSequenceIndex(int aniatmionSequenceIndex)
        {
            if (currentAnimationSequenceIndex == aniatmionSequenceIndex)
            {
                return;
            }

            currentAnimationSequenceIndex = aniatmionSequenceIndex;

            AnimationSequence animationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            animationSequence.SetCurrentFrameIndex(0);

            int currentTextureIndex = animationSequence.GetCurrentFrame();
            currentTexture = spriteTextureList.textureList[currentTextureIndex];
        }


        public void AddAnimationSequence(int key, AnimationSequence  animationSequence)
        {
            animationSequenceMap[key] = animationSequence;
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position)
        {

            AnimationSequence currentAnimationSequence = animationSequenceMap[currentAnimationSequenceIndex];
            currentAnimationSequence.Update();
            int currentTextureIndex = currentAnimationSequence.GetCurrentFrame();
            currentTexture = spriteTextureList.textureList[currentTextureIndex];

            spriteBatch.Draw(currentTexture, position, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);


            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, position, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);

            }


        }


        internal Texture2D createSpriteBorderRectangleTexture()
        {
            Texture2D rectangle = new Texture2D(MikeAndConqueryGame.instance.GraphicsDevice, spriteTextureList.textureWidth, spriteTextureList.textureHeight);
            Color[] data = new Color[rectangle.Width * rectangle.Height];
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            fillHorizontalLine(data, rectangle.Width, rectangle.Height, rectangle.Height - 1, Color.White);
            fillVerticalLine(data, rectangle.Width, rectangle.Height, 0, Color.White);
            fillVerticalLine(data, rectangle.Width, rectangle.Height, rectangle.Width - 1, Color.White);
            int centerX = rectangle.Width / 2;
            int centerY = rectangle.Height / 2;
            int centerOffset = (centerY * rectangle.Width) + centerX;

            data[centerOffset] = Color.Red;


            rectangle.SetData(data);
            return rectangle;

        }


        internal void fillHorizontalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = width * lineIndex;
            for (int i = beginIndex; i < (beginIndex + width); ++i)
            {
                data[i] = color;
            }
        }

        internal void fillVerticalLine(Color[] data, int width, int height, int lineIndex, Color color)
        {
            int beginIndex = lineIndex;
            for (int i = beginIndex; i < (width * height); i += width)
            {
                data[i] = color;
            }
        }

        public void SetAnimate(bool animateFlag)
        {
            animationSequenceMap[currentAnimationSequenceIndex].SetAnimate(animateFlag);
        }


    }





}
