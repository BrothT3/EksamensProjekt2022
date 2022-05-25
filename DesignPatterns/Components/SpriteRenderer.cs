using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjekt2022
{
    public class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Origin { get; set; }
        public int OffSet { get; set; } = 0;
        public Rectangle Rectangle { get; set; }
        public override void Start()
        {
            Origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
        }

        public void SetSprite(string spriteName)
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>(spriteName);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Rectangle.IsEmpty)
            spriteBatch.Draw(Sprite, new Vector2(GameObject.Transform.Position.X, GameObject.Transform.Position.Y + OffSet ), null, Color.White, 0, Origin, 1, SpriteEffects.None, 1);
            else
            {
                spriteBatch.Draw(Sprite, new Vector2(GameObject.Transform.Position.X, GameObject.Transform.Position.Y + OffSet), Rectangle, Color.White, 0, Origin, 1, SpriteEffects.None, 1);
            }

        }
    }
}
