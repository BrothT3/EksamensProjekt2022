using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class Fish : Item
    {
        public Color CookingColor { get; set; }
        public float CookingFloat { get; set; } = 0.1f;
        public Fish(int _quantity)
        {
            Name = "fish";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("Fish");
            this.Quantity = _quantity;
            CookingColor = Color.White;
            this.MaxQuantity = Constants.MAX_FISH_QUANTITY;
        }
        public Fish()
        {
            Name = "fish";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("Fish");
            CookingColor = Color.White;
            this.MaxQuantity = Constants.MAX_FISH_QUANTITY;
        }
    }
}
