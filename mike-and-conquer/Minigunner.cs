using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace mike_and_conquer
{

    public class Minigunner
    {
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int health { get; set; }
        public bool selected { get; set; }


        private Vector2 position;
        Texture2D texture;

        private int worldWidth;
        private int worldHeight;
        private Minigunner()
        {

        }

        private static int globalId = 1;

        public Minigunner(int aWorldWidth, int aWorldHeight, Texture2D aTexture, int anX, int aY )
        {
            this.worldWidth = aWorldWidth;
            this.worldHeight = aWorldHeight;
            this.texture = aTexture;
            x = anX;
            y = aY;
            health = 1000;
            id = Minigunner.globalId;
            Minigunner.globalId++;
        }

        public void Update(GameTime gameTime)
        {
            double velocity = .15;
            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * velocity;


            position.X = position.X + (float)delta;
            //position.X += 1;
            if (position.X > worldWidth)
                position.X = 0;

            position.Y = position.Y + (float)delta;
            if (position.Y > worldHeight)
                position.Y = 0;


        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 plottedPosition = new Vector2();
            plottedPosition.X = (float)Math.Round(position.X);
            plottedPosition.Y = (float)Math.Round(position.Y);


            //            Debug.WriteLine("x,y=" + plottedPosition.X + "," + plottedPosition.Y);

            float scale = 5f;
            spriteBatch.Draw(texture, plottedPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

        }
    }



}
