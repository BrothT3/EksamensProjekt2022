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


       }
           
        
        }
    }
}
