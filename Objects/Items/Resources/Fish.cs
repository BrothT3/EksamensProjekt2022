using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class Fish : Item
    {
        public Fish(int _quantity)
        {
            Name = "fish";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("MinerTest");
            this.Quantity = _quantity;

            this.MaxQuantity = Constants.MAX_FISH_QUANTITY;
        }
        public Fish()
        {
            Name = "fish";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("MinerTest");

            this.MaxQuantity = Constants.MAX_FISH_QUANTITY;
        }
    }
}
