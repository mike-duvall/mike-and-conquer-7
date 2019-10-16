

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
                basicMapSquareView.Draw(gameTime, spriteBatch, 1.0f);
            }

            foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            {
                nextTerrainView.DrawShadowOnly(gameTime,spriteBatch, 0.9f);
            }

            GameWorldView.instance.GDIBarracksView.Draw(gameTime, spriteBatch, 0.85f);

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.GdiMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch, 0.8f);
            }

            foreach (MinigunnerView nextMinigunnerView in GameWorldView.instance.NodMinigunnerViewList)
            {
                nextMinigunnerView.Draw(gameTime, spriteBatch, 0.7f);
            }

            foreach (SandbagView nextSandbagView in GameWorldView.instance.SandbagViewList)
            {
                nextSandbagView.Draw(gameTime, spriteBatch, 0.6f);
            }


  
            foreach (TerrainView nextTerrainView in GameWorldView.instance.terrainViewList)
            {
                nextTerrainView.DrawNoShadow(gameTime,spriteBatch, 0.5f);
//                nextTerrainView.Draw(gameTime, spriteBatch);
            }


            MikeAndConquerGame.instance.unitSelectionBox.Draw(gameTime, spriteBatch, 0.4f);


        }
    }
}
