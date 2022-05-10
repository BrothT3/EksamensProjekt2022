using Microsoft.Xna.Framework;

namespace EksamensProjekt2022
{
    public class Camera
    {
        public float Zoom { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public Vector2 Origin = Vector2.Zero;

        public Camera()
        {
            Zoom = 1f;
            Rotation = 0;
            Position = Vector2.Zero;
            
        }
        public void Move(Vector2 direction)
        {
            Position += direction;
        }
        public Matrix GetTransform()
        {
            var translationMatrix = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0));
            var rotationMatrix = Matrix.CreateRotationZ(Rotation);
            var scaleMatrix = Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
            var originMatrix = Matrix.CreateTranslation(new Vector3(Origin.X, Origin.Y, 0));

            return translationMatrix * rotationMatrix * scaleMatrix * originMatrix;
        }

        public Vector3 GetScreenScale()
        {
            var scaleX = (float)GameWorld.Instance.GraphicsDevice.Viewport.Width / (float)GameWorld.Instance.Graphics.PreferredBackBufferWidth;
            var scaleY = (float)GameWorld.Instance.GraphicsDevice.Viewport.Height / (float)GameWorld.Instance.Graphics.PreferredBackBufferHeight;
            return new Vector3(scaleX, scaleY, 1.0f);
        }
    }
}
