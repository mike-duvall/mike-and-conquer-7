


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GraphicsDevice = Microsoft.Xna.Framework.Graphics.GraphicsDevice;
using Color = Microsoft.Xna.Framework.Color;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;

namespace mike_and_conquer.gameview.sidebar
{
    public class SidebarView
    {

        public MinigunnerSidebarIconView minigunnerSidebarIconView;
        public BarracksSidebarIconView barracksSidebarIconView;
        // private Texture2D sidebarBackgroundRectangle;


        public SidebarView(GraphicsDevice graphicsDevice)
        {
            // sidebarBackgroundRectangle = new Texture2D(graphicsDevice, 1, 1);
            // sidebarBackgroundRectangle.SetData(new[] { Color.LightSkyBlue });

        }

        public void Update(GameTime gameTime)
        {
            if (GameWorld.instance.GDIBarracks != null)
            {
                minigunnerSidebarIconView.Update(gameTime);
            }
            
            if (GameWorld.instance.GDIConstructionYard != null)
            {
                barracksSidebarIconView.Update(gameTime);
            }

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (minigunnerSidebarIconView != null)
            {
                minigunnerSidebarIconView.Draw(gameTime, spriteBatch);
            }

            if (barracksSidebarIconView != null)
            {
                barracksSidebarIconView.Draw(gameTime, spriteBatch);
            }

        }

        public void AddMinigunnerSidebarIconView()
        {
            minigunnerSidebarIconView = new MinigunnerSidebarIconView(new Point(112, 24));
        }

        public void AddBarracksSidebarIconView()
        {
            barracksSidebarIconView = new BarracksSidebarIconView(new Point(32, 24));
        }

        public void HandleReset()
        {
            barracksSidebarIconView = null;
            minigunnerSidebarIconView = null;

        }
    }
}
