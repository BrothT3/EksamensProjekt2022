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
        private bool updated;
        private bool click;
        private bool ifInventoryOpen = true;
        private List<Button> transferButtons = new List<Button>();
        private Rectangle inventoryBox;
        private int cameraOffsetX;
        private int cameraOffsetY;

        public bool Updated = false;

        public UserInterface()
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }

        public void Update(GameTime gameTime)
        {
            cameraOffsetX = (int)GameControl.Instance.camera.Position.X;
            cameraOffsetY = (int)GameControl.Instance.camera.Position.Y;
            if ((GameControl.Instance.playing.currentGameObjects.Exists(x => (x.Tag == "Player"))))
            {
                UpdateButtons();
            }
            
            mState = Mouse.GetState();
            if (mState.LeftButton == ButtonState.Released && updated)
            {
                click = true;
            }
            if (ifInventoryOpen)
            {

                
                InputHandler.Instance.playerInventoryBox = inventoryBox;

            }
            if (!ifInventoryOpen)
            {

               
                InputHandler.Instance.playerInventoryBox = Rectangle.Empty;
            }
            foreach (Button button in transferButtons)
            {
                button.Update(gameTime);
                if (updated)
                {
                    int offsetX = button.Rectangle.X - cameraOffsetX;
                    int offsetY = button.Rectangle.Y - cameraOffsetY;
                    button.Rectangle = new Rectangle(offsetX, offsetY, button.Rectangle.Width, button.Rectangle.Height);
                    button.Update(gameTime);
                }
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


                    transferButtons.Add(itembutton);
                    itemSlot.X += 44;
                    inventoryBox.X += 44;
                    rowNumber++;
                    if (rowNumber >= 4)
                    {
                        itemSlot.Y += 44;
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
                GameObject selectedBuilding = GameControl.Instance.playing.currentGameObjects.First(x => x.Tag == "selectedBuilding");
                click = false;
                if (selectedBuilding != null)
                {
                    Button button = (Button)captured_button;
                    Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
                    Inventory playerInv = player.GameObject.GetComponent<Inventory>() as Inventory;
                    Inventory buildingInv = selectedBuilding.GetComponent<Inventory>() as Inventory;
                    Type type = button.Item.GetType();
                    Item item1 = (Item)Activator.CreateInstance(type);
                    Item item2 = (Item)Activator.CreateInstance(type);
                    item1.Quantity = button.Item.Quantity;
                    item2.Quantity = button.Item.Quantity;
                    playerInv.AddItem(item2);
                    int notRoomFor = playerInv.notAddedAmount;
                    int toRemove = item1.Quantity - notRoomFor;
                    buildingInv.RemoveItem(item1, toRemove);
                    playerInv.notAddedAmount = 0;


                    updated = false;
                }
                
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();

            if(player!=null)
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
