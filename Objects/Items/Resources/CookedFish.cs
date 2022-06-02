using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class CookedFish: Item
    {
        public CookedFish(int _quantity)
        {
            Name = "cookedfish";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("fish");
            this.Quantity = _quantity;

            this.MaxQuantity = Constants.MAX_COOKEDFISH_QUANTITY;
        }
        public CookedFish()
        {
            Name = "cookedfish";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("fish");
            this.Quantity = 1;
            this.MaxQuantity = Constants.MAX_COOKEDFISH_QUANTITY;
        }
    }
}
