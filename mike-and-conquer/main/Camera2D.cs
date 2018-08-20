
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Vector3 = Microsoft.Xna.Framework.Vector3;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using Matrix = Microsoft.Xna.Framework.Matrix;
using Viewport = Microsoft.Xna.Framework.Graphics.Viewport;

namespace mike_and_conquer_6
{
    public class Camera2D
    {
        public float Zoom { get; set; }
        public Vector2 Location { get; set; }
        public float Rotation { get; set; }

        private Rectangle Bounds { get; set; }

        public Matrix TransformMatrix
        {
            get {
                return
                    Matrix.CreateTranslation(new Vector3(-Location.X, -Location.Y, 0)) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(Zoom) * 
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));

            }
        }
        



        public Camera2D(Viewport viewport)
        {
            Bounds = viewport.Bounds;
        }
    }
}
