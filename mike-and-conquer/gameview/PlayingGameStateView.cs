

using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    class PlayingGameStateView : GameStateView
    {
        public override  void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (MapTileInstanceView basicMapSquareView in GameWorldView.instance.MapTileInstanceViewList)
            {
                basicMapSquareView.Draw(gameTime, spriteBatch);
            }

            GameWorldView.instance.GDIBarracksView.Draw(gameTime, spriteBatch);

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch);
            }

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch);
            }

            foreach (SandbagView nextSandbagView in GameWorldView.instance.SandbagViewList)
            {
                nextSandbagView.Draw(gameTime, spriteBatch);
            }

            foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            {
                nextTerrainView.Draw(gameTime, spriteBatch);
            }


            MikeAndConquerGame.instance.unitSelectionBox.Draw(gameTime, spriteBatch);


        }
    }
}
