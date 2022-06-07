using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EksamensProjekt2022
{
    public class UserInterface
    {
        private Vector2 firstItemSlot = new Vector2(10, 36);
        private SpriteFont font;
        private MouseState mState;
        private KeyboardState kState;
        private bool updated;
        private bool click;
        public bool transferBool;
        private List<Button> transferButtons = new List<Button>();
        public Rectangle inventoryBox;
        private int cameraOffsetX;
        private int cameraOffsetY;

        public bool Updated = false;

        public UserInterface()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }

        public void Update(GameTime gameTime)
        {

            if (GameControl.Instance.playing.currentGameObjects.Exists(x => x.Tag == "selectedChest"))
                InputHandler.Instance.playerInventoryBox = inventoryBox;
            else
                InputHandler.Instance.playerInventoryBox = Rectangle.Empty;

            if ((GameControl.Instance.playing.currentGameObjects.Exists(x => (x.Tag == "Player"))))
            {
                UpdateButtons();
            }

            mState = Mouse.GetState();
            kState = Keyboard.GetState();
            if (mState.LeftButton == ButtonState.Released)
            {
                click = true;
            }

            foreach (Button button in transferButtons)
            {
                button.Update(gameTime);
                Vector3 cameraVector = GameControl.Instance.camera.GetTransform().Translation;
                int recX = (int)GameControl.Instance.camera.Position.X;
                int recY = (int)GameControl.Instance.camera.Position.Y;
                button.Rectangle = new Rectangle((-recX) + button.RecX, (-recY) + button.RecY, 30, 30);
            }


        }
        public void UpdateButtons()
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;

            if (!Updated)
            {
                transferButtons.Clear();
                int i = 0;
                int rowNumber = 0;
                Vector2 itemSlot = firstItemSlot;
                inventoryBox = new Rectangle((int)itemSlot.X, (int)itemSlot.Y, 40, 40);

                foreach (Item item in inv.items)
                {
                    Button itembutton = new Button(inv.items[i]);
                    Button captured_button = itembutton;
                    itembutton.OnClicking += TransferEvent();
                    itembutton.Rectangle = new Rectangle((int)itemSlot.X, (int)itemSlot.Y, 36, 36);
                    itembutton.RecX = (int)itemSlot.X;
                    itembutton.RecY = (int)itemSlot.Y;

                    transferButtons.Add(itembutton);
                    itemSlot.X += 44;
                    inventoryBox.Width += 44;
                    rowNumber++;
                    if (rowNumber >= 4)
                    {
                        itemSlot.Y += 44;
                        inventoryBox.Height += 44;
                        itemSlot.X = firstItemSlot.X;
                        rowNumber = 0;
                    }
                    i++;
                    
                }
                
            }
            Updated = true;

        }

        private EventHandler TransferEvent()
        {

            return Transfer;
        }

        public void Transfer(object captured_button, EventArgs e)
        {
            if (click)
            {

                if (GameControl.Instance.playing.currentGameObjects.Exists(x => x.Tag == "selectedChest"))
                {
                    GameObject selectedChest = GameControl.Instance.playing.currentGameObjects.First(x => x.Tag == "selectedChest");
                    if (selectedChest != null && selectedChest.GetComponent<Inventory>() != null)
                    {
                        click = false;
                        Button button = (Button)captured_button;
                        Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
                        Inventory playerInv = player.GameObject.GetComponent<Inventory>() as Inventory;
                        Inventory chestInv = selectedChest.GetComponent<Inventory>() as Inventory;
                        Type type = button.Item.GetType();
                        Item item1 = (Item)Activator.CreateInstance(type);
                        Item item2 = (Item)Activator.CreateInstance(type);
                        item1.Quantity = button.Item.Quantity;
                        item2.Quantity = button.Item.Quantity;
                        chestInv.AddItem(item2);
                        int notRoomFor = chestInv.notAddedAmount;
                        int toRemove = item1.Quantity - notRoomFor;
                        playerInv.RemoveItem(item1, toRemove);
                        chestInv.notAddedAmount = 0;

                        Chest chestChest = selectedChest.GetComponent<Chest>() as Chest;
                        chestChest.Updated = false;
                        Updated = false;
                    }

                }





            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();

            if (player != null)
            {
                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;


                if (inv != null)
                {
                    foreach (Button button in transferButtons)
                    {
                        spriteBatch.Draw(button.Item.Sprite, button.Rectangle, Color.White);
                        spriteBatch.DrawString(font, $"{button.Item.Quantity}", new Vector2(button.Rectangle.X, button.Rectangle.Y), Color.White);
                    }
                }

                SurvivalAspect sa = (SurvivalAspect)player.GameObject.GetComponent<SurvivalAspect>() as SurvivalAspect;

                spriteBatch.Draw(GameWorld.Instance.pixel, new Rectangle(10 - (int)GameControl.Instance.camera.Position.X,
                    500 - ((int)GameControl.Instance.camera.Position.Y), 10, sa.CurrentHealth), Color.Green);

                spriteBatch.Draw(GameWorld.Instance.pixel, new Rectangle(25 - (int)GameControl.Instance.camera.Position.X,
                   500 - ((int)GameControl.Instance.camera.Position.Y), 10, sa.CurrentEnergy), Color.Red);

                spriteBatch.Draw(GameWorld.Instance.pixel, new Rectangle(40 - (int)GameControl.Instance.camera.Position.X,
                   500 - ((int)GameControl.Instance.camera.Position.Y), 10, sa.CurrentHunger), Color.Orange);
            }

        }
    }
}
