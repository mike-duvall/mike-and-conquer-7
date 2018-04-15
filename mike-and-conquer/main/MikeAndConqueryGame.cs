﻿using System.Collections.Generic;

using Game = Microsoft.Xna.Framework.Game;
using GameTime = Microsoft.Xna.Framework.GameTime;
using GraphicsDeviceManager = Microsoft.Xna.Framework.GraphicsDeviceManager;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using GraphicsProfile = Microsoft.Xna.Framework.Graphics.GraphicsProfile;
using Color = Microsoft.Xna.Framework.Color;

using SpriteSortMode = Microsoft.Xna.Framework.Graphics.SpriteSortMode;
using SamplerState = Microsoft.Xna.Framework.Graphics.SamplerState;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using AsyncGameEvent = mike_and_conquer.gameevent.AsyncGameEvent;
using CreateGDIMinigunnerGameEvent = mike_and_conquer.gameevent.CreateGDIMinigunnerGameEvent;
using GetGDIMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetGDIMinigunnerByIdGameEvent;
using GetNodMinigunnerByIdGameEvent = mike_and_conquer.gameevent.GetNodMinigunnerByIdGameEvent;

namespace mike_and_conquer
{

    public class MikeAndConqueryGame : Game
    {
        public static MikeAndConqueryGame instance;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public List<Minigunner> gdiMinigunnerList { get; }
        public List<Minigunner> nodMinigunnerList { get; }

        public float scale { get; }

        private GameState currentGameState;

        private TextureListMap textureListMap;

        public TextureListMap TextureListMap
        {
            get { return textureListMap; }
        }


        private List<AsyncGameEvent> gameEvents;


        public MikeAndConqueryGame()
        {
            graphics = new GraphicsDeviceManager(this);
            scale = 5f;

            bool makeFullscreen = true;
            //bool makeFullscreen = false;
            if (makeFullscreen)
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferWidth = 1920;
                graphics.PreferredBackBufferHeight = 1080;
            }
            else
            {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferWidth = 1280;
                graphics.PreferredBackBufferHeight = 1024;

            }


            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            this.IsFixedTimeStep = false;

            gdiMinigunnerList = new List<Minigunner>();
            nodMinigunnerList = new List<Minigunner>();

            currentGameState = new PlayingGameState();


            textureListMap = new TextureListMap();

            gameEvents = new List<AsyncGameEvent>();

            MikeAndConqueryGame.instance = this;

        }

        internal Minigunner GetGdiMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
                if (nextMinigunner.id == id)
                {
                    foundMinigunner = nextMinigunner;
                }
            }

            return foundMinigunner;
        }

        internal Minigunner GetNodMinigunner(int id)
        {
            Minigunner foundMinigunner = null;
            foreach (Minigunner nextMinigunner in nodMinigunnerList)
            {
                if (nextMinigunner.id == id)
                {
                    foundMinigunner = nextMinigunner;
                }

            }

            return foundMinigunner;
        }



        internal Minigunner AddGdiMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new GdiMinigunner(x, y);
            gdiMinigunnerList.Add(newMinigunner);
            return newMinigunner;
        }


        internal Minigunner AddNodMinigunner(int x, int y)
        {
            Minigunner newMinigunner = new NodMinigunner(x, y);
            nodMinigunnerList.Add(newMinigunner);
            return newMinigunner;
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

            this.IsMouseVisible = true;
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
            textureListMap.LoadSpriteListFromShpFile(GdiMinigunner.SPRITE_KEY, GdiMinigunner.SHP_FILE_NAME, GdiMinigunner.SHP_FILE_COLOR_MAPPER);
            textureListMap.LoadSpriteListFromShpFile(NodMinigunner.SPRITE_KEY, GdiMinigunner.SHP_FILE_NAME, NodMinigunner.SHP_FILE_COLOR_MAPPER);

            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.MISSION_SPRITE_KEY, "Mission");
            LoadSingleTextureFromFile(gameobjects.MissionAccomplishedMessage.ACCOMPLISHED_SPRITE_KEY, "Accomplished");
            LoadSingleTextureFromFile(gameobjects.MissionFailedMessage.FAILED_SPRITE_KEY, "Failed");


            // TODO: use this.Content to load your game content here
        }

        internal void SelectSingleGDIUnit(Minigunner minigunner)
        {
            minigunner.selected = true;
            foreach (Minigunner nextMinigunner in gdiMinigunnerList)
            {
               if (nextMinigunner.id != minigunner.id)
               {
                    nextMinigunner.selected = false;
               }
           }

        }

        private void LoadSingleTextureFromFile(string key, string fileName)
        {
            SpriteTextureList spriteTextureList = new SpriteTextureList();
            Texture2D texture2D = Content.Load<Texture2D>(fileName);
            spriteTextureList.textureList.Add(texture2D);
            spriteTextureList.textureWidth = texture2D.Width;
            spriteTextureList.textureHeight = texture2D.Height;

            TextureListMap.AddTextureList(key, spriteTextureList);
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
            currentGameState = currentGameState.Update(gameTime);
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

            currentGameState.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        internal GameState GetCurrentGameState()
        {
            return currentGameState;
        }


        public void HandleReset()
        {
            gdiMinigunnerList.Clear();
            nodMinigunnerList.Clear();
            currentGameState = new PlayingGameState();

        }


        public GameState ProcessGameEvents()
        {
            GameState newGameState = null;

            //std::vector<AsyncGameEvent*>::iterator iter;
            //std::lock_guard < std::mutex > lock (gameEventsMutex) ;
            //for (iter = gameEvents.begin(); iter != gameEvents.end(); ++iter)
            //{
            //    AsyncGameEvent* nextGameEvent = *iter;
            //    GameState* returnedGameState = nextGameEvent->Process();
            //    if (returnedGameState != nullptr && newGameState == nullptr)
            //    {
            //        newGameState = returnedGameState;
            //    }
            //}
            //gameEvents.clear();


            lock(gameEvents)
            {
                foreach(AsyncGameEvent nextGameEvent in gameEvents)
                {
                    GameState returnedGameState = nextGameEvent.Process();
                    if (returnedGameState != null && newGameState == null)
                    {
                        newGameState = returnedGameState;
                    }
                }
                gameEvents.Clear();
            }


            return newGameState;

        }


        public Minigunner CreateGDIMinigunnerViaEvent(int x, int y)
        {
            CreateGDIMinigunnerGameEvent gameEvent = new CreateGDIMinigunnerGameEvent(x, y);
            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;

        }


        public Minigunner GetGDIMinigunnerByIdViaEvent(int id)
        {
            GetGDIMinigunnerByIdGameEvent gameEvent = new GetGDIMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }

        public Minigunner GetNodMinigunnerByIdViaEvent(int id)
        {
            GetNodMinigunnerByIdGameEvent gameEvent = new GetNodMinigunnerByIdGameEvent(id);

            lock (gameEvents)
            {
                gameEvents.Add(gameEvent);
            }

            Minigunner gdiMinigunner = gameEvent.GetMinigunner();
            return gdiMinigunner;
        }


    }

}
