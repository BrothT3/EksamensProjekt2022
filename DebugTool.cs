using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EksamensProjekt2022
{
    public class DebugTool
    {
        private SpriteFont font;
        public bool ShowCellPoints { get; set; } = false;
        private KeyboardState kState;
        public DebugTool()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }

        public void Update(GameTime gameTime)
        {
            kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.P))
            {
                ShowCellPoints = true;
            }
            else
            {
                ShowCellPoints = false;
            }
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

            if(ShowCellPoints)
            {
                foreach (Cell c in GameWorld.Instance.currentCells.Values)
                {
                    spriteBatch.DrawString(font, $"({c.Position.X.ToString()}\n{c.Position.Y.ToString()})", new Vector2(c.background.X, c.background.Y), Color.Black, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
            }

        }
    }
}
