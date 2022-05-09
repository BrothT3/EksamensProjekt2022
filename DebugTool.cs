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
            int x = (int)player.GameObject.Transform.Position.X;
            int y = (int)player.GameObject.Transform.Position.Y;

          
            spriteBatch.DrawString(font,$" ({x.ToString()} , {y.ToString()})", 
                new Vector2(player.GameObject.Transform.Position.X -40, player.GameObject.Transform.Position.Y + 50), Color.White);
         

        }
    }
}
