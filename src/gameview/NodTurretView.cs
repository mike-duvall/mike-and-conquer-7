using System;
using mike_and_conquer.gameobjects;
using mike_and_conquer.gamesprite;
using mike_and_conquer.main;
using AnimationSequence = mike_and_conquer.util.AnimationSequence;

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class NodTurretView
    {

        // TODO:  Using UnitSprite for now.  May need new Sprite
        // unique to sandbag like things, things like trees, etc
        private UnitSprite unitSprite;
        private NodTurret myNodTurret;

        public const string SPRITE_KEY = "NodTurret";

        // TODO:  SHP_FILE_NAME and ShpFileColorMapper don't really belong in this view
        // Views should be agnostic about where the sprite data was loaded from
        public const string SHP_FILE_NAME = "Shp\\gun.shp";
        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new NodSecondaryShpFileColorMapper();


        public NodTurretView(NodTurret nodTurret)
        {
            this.myNodTurret = nodTurret;
            this.unitSprite = new UnitSprite(SPRITE_KEY);
            this.unitSprite.drawShadow = true;
            SetupAnimations();
        }

        private void SetupAnimations()
        {
            AnimationSequence animationSequence = new AnimationSequence(1);

            for (int i = 0; i < 32; i++)
            {
                animationSequence.AddFrame(i + (myNodTurret.TurretType * 32));
            }


            unitSprite.AddAnimationSequence(0, animationSequence);
            unitSprite.SetAnimate(false);
        }

        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.DrawShadowOnly(gameTime, spriteBatch, myNodTurret.MapTileLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            unitSprite.DrawNoShadow(gameTime, spriteBatch, myNodTurret.MapTileLocation.WorldCoordinatesAsVector2, SpriteSortLayers.UNIT_DEPTH);
        }

        private int previousFrameOffset = -1;
//        private GameTime previousFrameStartTime = null;
        private int previousFrameStartTimeInMillis = -1;

        public void Update(GameTime gameTime)
        {
            int frameOffset = CalculateFrameOffsetFromDirection(this.myNodTurret.Direction);
//            MikeAndConquerGame.instance.log.Information("Setting animation index to:{0}", frameOffset);
            unitSprite.SetFrameOfCurrentAnimationSequence(frameOffset);


//            MikeAndConquerGame.instance.log.Information("previousFrameStartTimeInMillis:{0}, current:{1}",
//                previousFrameStartTimeInMillis, gameTime.TotalGameTime.Milliseconds);



            if (frameOffset != previousFrameOffset)
            {
                int currentElapsedMilliseconds = (int) gameTime.TotalGameTime.TotalMilliseconds;

                if (previousFrameStartTimeInMillis != -1)
                {
                    MikeAndConquerGame.instance.log.Information("previousFrameOffset:{0}, time spent at previous offset:{1}",
                        previousFrameOffset, currentElapsedMilliseconds - previousFrameStartTimeInMillis);
                    MikeAndConquerGame.instance.log.Information("currentElapsedMilliseconds:{0}, previousFrameStartTimeInMillis:{1}",
                        currentElapsedMilliseconds,  previousFrameStartTimeInMillis);

                }

                previousFrameStartTimeInMillis = currentElapsedMilliseconds;
                previousFrameOffset = frameOffset;

            }

        }

        private int CalculateFrameOffsetFromDirection(float direction)
        {

            int frameOffset = (int) Math.Round(direction / 11.5f);

            // the frames are stored in counterclockwise order
            // but `direction` is in clockwise order
            // so we have to translate it
            int mappedFrameOffset = 32 - frameOffset;

            if (mappedFrameOffset == 32)
            {
                mappedFrameOffset = 0;
            }

            MikeAndConquerGame.instance.log.Information("direction:{0}", direction);
            MikeAndConquerGame.instance.log.Information("mappedFrameOffset:{0}", mappedFrameOffset);
            return mappedFrameOffset;
        }

    }
}
