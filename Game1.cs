using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;


namespace mike_and_conquer_6
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        Vector2 position;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 640;
            //graphics.PreferredBackBufferHeight = 400;

            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1080;


            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            position = new Vector2(0, 0);
                        this.IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            texture = this.Content.Load<Texture2D>("m3");
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            double velocity = .15;
            double delta = gameTime.ElapsedGameTime.TotalMilliseconds * velocity;


            position.X = position.X + (float)delta;
            //position.X += 1;
            if (position.X > this.GraphicsDevice.Viewport.Width)
                position.X = 0;

            position.Y = position.Y + (float)delta;
            if (position.Y > this.GraphicsDevice.Viewport.Height)
                position.Y = 0;


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp);


            //            spriteBatch.Draw(texture, position);
            Vector2 plottedPosition = new Vector2();
            plottedPosition.X = (float)Math.Round(position.X);
            plottedPosition.Y = (float)Math.Round(position.Y);


            Debug.WriteLine("x,y=" + plottedPosition.X + "," + plottedPosition.Y);

            float scale = 5f;
            spriteBatch.Draw(texture, plottedPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            //(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

            //spriteBatch.Draw(texture, position, Color.White);

            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
