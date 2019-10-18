
using Vector2 = Microsoft.Xna.Framework.Vector2;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;


namespace mike_and_conquer.gameview
{
    public class TerrainView
    {

        private TerrainSprite terrainSprite;

        public static readonly ShpFileColorMapper SHP_FILE_COLOR_MAPPER = new GdiShpFileColorMapper();


        private TerrainItem terrainItem;

        public TerrainView(TerrainItem terrainItem)
        {
            this.terrainItem = terrainItem;
            this.terrainSprite = new TerrainSprite( terrainItem.TerrainItemType, terrainItem.PositionInWorldCoordinates);
        }

//        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
//        {
//            Vector2 positionAsVector2 = new Vector2(terrainItem.PositionInWorldCoordinates.X, terrainItem.PositionInWorldCoordinates.Y);
//            terrainSprite.Draw(gameTime, spriteBatch, positionAsVector2);
//        }

        public void DrawFull(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawNoShadow(gameTime, spriteBatch);
            DrawShadowOnly(gameTime,spriteBatch);
        }

        public void DrawShadowOnly(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 positionAsVector2 = new Vector2(terrainItem.PositionInWorldCoordinates.X, terrainItem.PositionInWorldCoordinates.Y);
            this.terrainSprite.DrawShadowOnly(gameTime, spriteBatch, positionAsVector2, terrainItem.LayerDepthOffset);
        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 positionAsVector2 = new Vector2(terrainItem.PositionInWorldCoordinates.X, terrainItem.PositionInWorldCoordinates.Y);
            this.terrainSprite.DrawNoShadow(gameTime, spriteBatch, positionAsVector2, terrainItem.LayerDepthOffset);
        }
    }
}
