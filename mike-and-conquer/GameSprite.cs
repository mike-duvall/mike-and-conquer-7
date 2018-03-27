using AnimationSequence = mike_and_conquer.util.AnimationSequence;
using System.Collections.Generic;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Boolean = System.Boolean;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using ShpTDSprite = OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;


namespace mike_and_conquer
{
    public class GameSprite
    {

        //AnimationSequence animationSequence;



        Dictionary<int, AnimationSequence> animationSequenceMap;

        int currentAnimationSequenceIndex;

        //List<Texture2D> textureList;
        static SpriteTextureList spriteTextureList = null;
        Texture2D currentTexture;

        Texture2D spriteBorderRectangleTexture;
        Boolean drawBoundingRectangle;


        private int worldWidth;
        private int worldHeight;

        private Vector2 middleOfSprite;

        private ShpFileColorMapper shpFileColorMapper;

        public int unscaledWidth;
        public int unscaledHeight;

        public GameSprite(ShpFileColorMapper shpFileColorMapper)
        {
            this.animationSequenceMap = new Dictionary<int, util.AnimationSequence>();
            this.shpFileColorMapper = shpFileColorMapper;
            this.worldWidth = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Width;
            this.worldHeight = MikeAndConqueryGame.instance.GraphicsDevice.Viewport.Height;
            //            this.textureList = new List<Texture2D>();

            if (spriteTextureList == null)
            {
                spriteTextureList = LoadAllTexturesFromShpFile("Content\\e1.shp");
            }

            spriteBorderRectangleTexture = createSpriteBorderRectangleTexture();

            middleOfSprite = new Vector2();

            //middleOfSprite.X = textureList[0].Width / 2;
            //middleOfSprite.Y = textureList[0].Height / 2;
            middleOfSprite.X = spriteTextureList.textureWidth / 2;
            middleOfSprite.Y = spriteTextureList.textureHeight / 2;


            drawBoundingRectangle = false;
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

            spriteBatch.Draw(currentTexture, position, null, Color.White, 0f, middleOfSprite, MikeAndConqueryGame.instance.scale, SpriteEffects.None, 0f);


            if (drawBoundingRectangle)
            {
                spriteBatch.Draw(spriteBorderRectangleTexture, position, null, Color.White, 0f, middleOfSprite, MikeAndConqueryGame.instance.scale, SpriteEffects.None, 0f);

            }


        }


        internal Texture2D createSpriteBorderRectangleTexture()
        {
            //            Texture2D rectangle = new Texture2D(MikeAndConqueryGame.instance.GraphicsDevice, textureList[0].Width, textureList[0].Height);
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





        internal SpriteTextureList LoadAllTexturesFromShpFile(string shpFileName)
        {

            int[] remap = { };

            SpriteTextureList spriteTextureList = new SpriteTextureList();
            OpenRA.Graphics.ImmutablePalette palette = new OpenRA.Graphics.ImmutablePalette("Content\\temperat.pal", remap);

            System.IO.FileStream shpStream = System.IO.File.Open(shpFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader loader = new OpenRA.Mods.Common.SpriteLoaders.ShpTDLoader();
            ShpTDSprite shpTDSprite = new OpenRA.Mods.Common.SpriteLoaders.ShpTDSprite(shpStream);

            unscaledWidth = shpTDSprite.Frames[0].Size.Width;
            unscaledHeight = shpTDSprite.Frames[0].Size.Height;

            spriteTextureList.textureWidth = unscaledWidth;
            spriteTextureList.textureHeight = unscaledHeight;

            foreach (OpenRA.Graphics.ISpriteFrame frame in shpTDSprite.Frames)
            {
                //            OpenRA.Graphics.ISpriteFrame frame = shpTDSprite.Frames[indexOfFrameToLoad];
                byte[] frameData = frame.Data;

                Texture2D texture2D = new Texture2D(MikeAndConqueryGame.instance.GraphicsDevice, shpTDSprite.Size.Width, shpTDSprite.Size.Height);
                int numPixels = texture2D.Width * texture2D.Height;
                Color[] texturePixelData = new Color[numPixels];

                for (int i = 0; i < numPixels; i++)
                {
                    int basePaletteIndex = frameData[i];
                    int mappedPaletteIndex = MapColorIndex(basePaletteIndex);
                    uint mappedColor = palette[mappedPaletteIndex];

                    System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)mappedColor);
                    Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                    texturePixelData[i] = xnaColor;
                }

                texture2D.SetData(texturePixelData);
                shpStream.Close();
                spriteTextureList.textureList.Add(texture2D);
            }
            return spriteTextureList;

        }


        protected virtual int MapColorIndex(int index)
        {
            return shpFileColorMapper.MapColorIndex(index);
        }



    }





}
