
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


        Pickup here
        screen shot compare with trees is failing, need to fix it
        It's failing because the trees are no longer being sorted
        in the order they are drawn, they are being sorted at the same level,
        and some are improperly overlapping
        Could consider each TerrainView getting initialized with a depth
        that gets decremented as each one is created
        and subtract(or add?) that depth to the base TERRAIN_DEPTH when 
        rendering
        Bottom line:  not all trees are the same layerDepth, it goes
        from top to bottom, farthest to closest

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
            this.terrainSprite.DrawShadowOnly(gameTime, spriteBatch, positionAsVector2);
        }

        public void DrawNoShadow(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 positionAsVector2 = new Vector2(terrainItem.PositionInWorldCoordinates.X, terrainItem.PositionInWorldCoordinates.Y);
            this.terrainSprite.DrawNoShadow(gameTime, spriteBatch, positionAsVector2);
        }
    }
}
