using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class UserInterface
    {
        private Vector2 firstItemSlot = new Vector2(10, 36);
        private SpriteFont font;
        public UserInterface()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();

            if(player!=null)
            {
                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;

                Vector2 itemSlot = firstItemSlot;
                foreach (Item item in inv.items)
                {
                    spriteBatch.Draw(item.Sprite, itemSlot, Color.White);
                    spriteBatch.DrawString(font, $"{item.Quantity}", new Vector2(itemSlot.X + 20, itemSlot.Y + 40), Color.White);
                    itemSlot.X += 36;
                }

                SurvivalAspect sa = (SurvivalAspect)player.GameObject.GetComponent<SurvivalAspect>() as SurvivalAspect;

                spriteBatch.Draw(GameWorld.Instance.pixel, new Rectangle(10 - (int)GameControl.Instance.camera.Position.X ,
                    500- ((int)GameControl.Instance.camera.Position.Y), 10, sa.CurrentHealth), Color.Green);

                spriteBatch.Draw(GameWorld.Instance.pixel, new Rectangle(25 - (int)GameControl.Instance.camera.Position.X,
                   500 - ((int)GameControl.Instance.camera.Position.Y), 10, sa.CurrentEnergy), Color.Red);

                spriteBatch.Draw(GameWorld.Instance.pixel, new Rectangle(40 - (int)GameControl.Instance.camera.Position.X,
                   500 - ((int)GameControl.Instance.camera.Position.Y), 10, sa.CurrentHunger), Color.Orange);
            }
           
        }
    }
}
