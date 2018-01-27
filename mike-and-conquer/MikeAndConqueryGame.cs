using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace mike_and_conquer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MikeAndConqueryGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D texture;
        Vector2 position;
        List<Minigunner> minigunnerList;

        public MikeAndConqueryGame()
        {
            graphics = new GraphicsDeviceManager(this);
            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 1024;

            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;


            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            position = new Vector2(0, 0);
                        this.IsFixedTimeStep = false;

            minigunnerList = new List<Minigunner>();
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

            Minigunner theMinigunner = new Minigunner(this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height, texture);
            minigunnerList.Add(theMinigunner);
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
            //double velocity = .15;
            //double delta = gameTime.ElapsedGameTime.TotalMilliseconds * velocity;


            //position.X = position.X + (float)delta;
            //if (position.X > this.GraphicsDevice.Viewport.Width)
            //    position.X = 0;

            //position.Y = position.Y + (float)delta;
            //if (position.Y > this.GraphicsDevice.Viewport.Height)
            //    position.Y = 0;

            foreach (Minigunner nextMinigunner in minigunnerList)
            {
                nextMinigunner.Update(gameTime);
            }

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

            //Vector2 plottedPosition = new Vector2();
            //plottedPosition.X = (float)Math.Round(position.X);
            //plottedPosition.Y = (float)Math.Round(position.Y);
            //float scale = 5f;
            //spriteBatch.Draw(texture, plottedPosition, null, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            foreach (Minigunner nextMinigunner in minigunnerList)
            {
                nextMinigunner.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
