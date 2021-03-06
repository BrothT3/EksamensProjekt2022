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
        private KeyboardState oldKState;
        private int stoneCount;
        private int woodCount;
        private bool mReleased = false;
        private bool nReleased = false;
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
            
            Player p = (Player)GameWorld.Instance.FindObjectOfType<Player>() as Player;
            if(p!=null)
            {
                SurvivalAspect sa = p.GameObject.GetComponent<SurvivalAspect>() as SurvivalAspect;

                if (kState.IsKeyDown((Keys.M)) && mReleased && sa != null)
                {
                    sa.TakeDamage(25);
                    mReleased = false;
                }
                if (kState.IsKeyUp(Keys.M))
                    mReleased = true;

                if (kState.IsKeyDown(Keys.N) && nReleased)
                {
                    sa.Heal(10);
                    nReleased = false;
                }
                if (kState.IsKeyUp(Keys.N))
                {
                    nReleased = true;
                }
            }
          

            #region inventoryTests
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            Chest chest = (Chest)GameWorld.Instance.FindObjectOfType<Chest>();
            if (player != null)
            {

                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
                stoneCount = inv.GetItemCount<Stone>();
                woodCount = inv.GetItemCount<Wood>();
                if (kState.IsKeyDown(Keys.J) && kState != oldKState)
                {
                    Stone stone = new Stone(1);
                    inv.AddItem(stone);
                    GameControl.Instance.playing.userInterface.Updated = false;
                }
                if (kState.IsKeyDown(Keys.K) && kState != oldKState)
                {
                    Stone stone = new Stone(4);
                    inv.AddItem(stone);
                    GameControl.Instance.playing.userInterface.Updated = false;
                }
                if (kState.IsKeyDown(Keys.L) && kState != oldKState)
                {
                    Wood wood = new Wood(3);
                    inv.AddItem(wood);
                    GameControl.Instance.playing.userInterface.Updated = false;
                }
                if (kState.IsKeyDown(Keys.I) && kState != oldKState)
                {
                    if (inv.GetItemCount<Stone>() > 3)
                    {
                        Stone stone = new Stone(1);
                        inv.RemoveItem(stone, 3);
                        GameControl.Instance.playing.userInterface.Updated = false;
                    }

                }
                if (kState.IsKeyDown(Keys.O) && kState != oldKState)
                {
                    if (inv.GetItemCount<Wood>() > 2)
                    {
                        Wood wood = new Wood(1);
                        inv.RemoveItem(wood, 2);
                        GameControl.Instance.playing.userInterface.Updated = false;
                    }

                }

            }
            if (chest != null)
            {
                Inventory inv = chest.GameObject.GetComponent<Inventory>() as Inventory;
                if (kState.IsKeyDown(Keys.H) && kState != oldKState)
                {
                    Stone stone = new Stone(3);
                    inv.AddItem(stone);
                    chest.Updated = false;
                }
                if (kState.IsKeyDown(Keys.G) && kState != oldKState)
                {
                    Wood wood = new Wood(2);
                    inv.AddItem(wood);
                    chest.Updated = false;
                }
                if (kState.IsKeyDown(Keys.T) && kState != oldKState)
                {
                    if (inv.GetItemCount<Wood>() >= 4)
                    {
                        Wood wood = new Wood(1);
                        inv.RemoveItem(wood, 2);
                        chest.Updated = false;
                    }

                }
                if (kState.IsKeyDown(Keys.Y) && kState != oldKState)
                {
                    if (inv.GetItemCount<Stone>() >= 3)
                    {
                        Stone stone = new Stone(1);
                        inv.RemoveItem(stone, 3);
                        chest.Updated = false;
                    }

                }
            }
            #endregion
            oldKState = kState;

        }
        public void Draw(SpriteBatch spriteBatch)
        {

            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            if (player != null)
            {
                int x = (int)player.GameObject.Transform.Position.X;
                int y = (int)player.GameObject.Transform.Position.Y;

                spriteBatch.DrawString(font, $"({x}, {y})", new Vector2(player.GameObject.Transform.Position.X - 40,
                    player.GameObject.Transform.Position.Y + 50), Color.White);

                int pointX = (int)player.currentCell.Position.X;
                int pointY = (int)player.currentCell.Position.Y;

                spriteBatch.DrawString(font, $"({pointX}, {pointY})", new Vector2(player.GameObject.Transform.Position.X - 40,
                    player.GameObject.Transform.Position.Y + 80), Color.White);
               
                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;

                spriteBatch.DrawString(font, $"stones:{stoneCount}", new Vector2(340, 350), Color.Black);
                spriteBatch.DrawString(font, $"wood:{woodCount}", new Vector2(340, 380), Color.Black);

                SurvivalAspect sa = player.GameObject.GetComponent<SurvivalAspect>() as SurvivalAspect;
                int currentHealth = sa.CurrentHealth;
                spriteBatch.DrawString(font, $"{currentHealth}", new Vector2(player.GameObject.Transform.Position.X - 40,
                    player.GameObject.Transform.Position.Y + 120), Color.White);

            }
            Camera ca = MapCreator.Instance.Camera;
           

            if (ShowCellPoints)
            {
                foreach (Cell c in GameControl.Instance.playing.currentGrid)
                {
                    spriteBatch.DrawString(font, $"({c.Position.X}\n{c.Position.Y})", new Vector2(c.background.X, c.background.Y), Color.Black, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
                if (ca != null)
                {
                    spriteBatch.DrawString(font, $"{(MapCreator.Instance.CurrentArea)}", new Vector2(ca.Position.X - 0,
                        ca.Position.Y + 100), Color.White);
                }
            }

            if (MapCreator.DevMode)
            {
                spriteBatch.DrawString(font, $"Use the arrow keys to go through objects and sprites.\nHit left alt to save or close program to undo\nPlace with left click, remove with right\n" +
                    $"You can only remove tiles if you have a tile selected\n same with objects.\nYou can hold P to see Cell points\n" +
                    $"Change the current area with F1 and F2", new Vector2(-500, 100), Color.Black);

                spriteBatch.DrawString(font, "<- controls", new Vector2(0, 100), Color.Black);
            }

        }
    }
}
