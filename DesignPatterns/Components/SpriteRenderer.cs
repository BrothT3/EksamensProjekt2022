using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjekt2022
{
    public class SpriteRenderer : Component
    {
        public Texture2D Sprite { get; set; }
        public Vector2 Origin { get; set; }
        public int OffSetY { get; set; } = 0;
        public int OffSetX { get; set; } = 0;
        public Rectangle Rectangle { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
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
            spriteBatch.Draw(Sprite, new Vector2(GameObject.Transform.Position.X + OffSetX, GameObject.Transform.Position.Y + OffSetY ), null, Color.White, 0, Origin, 1, SpriteEffect, 1);
            else
            {
                spriteBatch.Draw(Sprite, new Vector2(GameObject.Transform.Position.X + OffSetX, GameObject.Transform.Position.Y + OffSetY), Rectangle, Color.White, 0, Origin, 1, SpriteEffect, 1);
            }

        }
    }
}
