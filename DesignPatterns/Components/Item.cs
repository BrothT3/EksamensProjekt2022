using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjekt2022
{
    public class Item
    {
        protected string name;
        private Texture2D sprite;

        public string Name { get => name; set => name = value; }

        protected int maxQuantity;
        protected int quantity;
        
        public Texture2D Sprite { get => sprite; set => sprite = value; }

        public int MaxQuantity { get => maxQuantity; set => maxQuantity = value; }
        public int Quantity { get => quantity; set => quantity = value; }

        public bool ToBeRemoved
        {
            get
            {
                if (quantity <= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool RoomForMore
        {
            get
            {
                if (Quantity < MaxQuantity)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }


    }

}
