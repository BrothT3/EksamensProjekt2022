using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EksamensProjekt2022
{
    public class Inventory : Component
    {
        public List<Item> items = new List<Item>();
        private int inventoryMax = 5;
        public int notAddedAmount = 0;

        public int InventoryMax { get => inventoryMax; set => inventoryMax = value; }

        public Inventory(int inventoryMax)
        {
            InventoryMax = inventoryMax;
        }
        public override void Update(GameTime gameTime)
        {

        }
        public void AddItem(Item item)
        {
            int amountToAdd = item.Quantity;
            while (amountToAdd > 0)
            {
                if (items.Exists(x => (x.Name == item.Name) && x.RoomForMore))
                {
                    Item itemStack = items.First(x => x.Name == item.Name && x.RoomForMore);

                    while (itemStack.RoomForMore && amountToAdd > 0)
                    {
                        amountToAdd--;
                        itemStack.Quantity++;
                    }


                }
                else if (items.Count < inventoryMax)
                {
                    Item newItem = item;
                    amountToAdd++;
                    newItem.Quantity = amountToAdd;
                    amountToAdd -= amountToAdd;
                    items.Add(newItem);



                }
                else if (items.Count == inventoryMax)
                {
                    notAddedAmount = amountToAdd;
                    break;
                }
                    

                item.Quantity--;

            }

        }
        public void RemoveItem(Item item, int amountToRemove)
        {
            while (amountToRemove > 0)
            {
                if (items.Exists(x => (x.Name == item.Name) && x.RoomForMore))
                {
                    Item itemStack = items.First(x => x.Name == item.Name && x.RoomForMore);

                    while (itemStack.RoomForMore && amountToRemove > 0 && !itemStack.ToBeRemoved)
                    {
                        amountToRemove--;
                        itemStack.Quantity--;
                    }
                    if (itemStack.ToBeRemoved)
                    {
                        items.Remove(itemStack);
                    }

                }
                else if (items.Exists(x => x.Name == item.Name))
                {
                    Item itemStack = items.First(x => x.Name == item.Name);
                    amountToRemove--;
                    itemStack.Quantity--;

                    if (itemStack.ToBeRemoved)
                    {
                        items.Remove(itemStack);
                    }
                }

            }

        }
       
        public int GetItemCount<T>() where T : Item
        {
            int sum = 0;
            foreach (Item item in items)
            {
                if (item is T itemAsT)
                {
                    sum += itemAsT.Quantity;
                }
            }
            return sum;
        }
        public int GetItemCount<T>(Item specificItem) where T : Item
        {

            int sum = 0;
            foreach (Item item in items)
            {
                if (item.Name == specificItem.Name)
                {
                    if (item is T itemAsT)
                    {
                        sum += itemAsT.Quantity;
                    }
                }

            }
            return sum;
        }

    }
    internal static class Constants
    {
        internal const int MAX_STONE_QUANTITY = 5;
        internal const int MAX_WOOD_QUANTITY = 10;
        internal const int MAX_FISH_QUANTITY = 8;
        internal const int MAX_VEGETABLE_QUANTITY = 10;
        internal const int MAX_COOKEDFISH_QUANTITY = 1;
    }

}
