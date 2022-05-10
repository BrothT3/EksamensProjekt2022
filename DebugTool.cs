using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjekt2022
{
    public class DebugTool
    {
        private SpriteFont font;

        public DebugTool()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            if (player != null)
            {
                int x = (int)player.GameObject.Transform.Position.X;
                int y = (int)player.GameObject.Transform.Position.Y;

                spriteBatch.DrawString(font, $"({x}, {y})", new Vector2(player.GameObject.Transform.Position.X - 40,
                    player.GameObject.Transform.Position.Y +50), Color.White);

                int pointX = (int)player.currentCell.Position.X;
                int pointY = (int)player.currentCell.Position.Y;

                spriteBatch.DrawString(font, $"({pointX}, {pointY})", new Vector2(player.GameObject.Transform.Position.X - 40,
                    player.GameObject.Transform.Position.Y + 80), Color.White);
            }


        }
    }
}
