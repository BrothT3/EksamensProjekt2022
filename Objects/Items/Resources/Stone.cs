using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class Stone : Item
    {
        public Stone(int _quantity)
        {
            Name = "stone";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("AreaSprites/Rock");
            this.Quantity = _quantity;

            this.MaxQuantity = Constants.MAX_STONE_QUANTITY;
        }
        public Stone()
        {
            Name = "stone";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("AreaSprites/Rock");

            this.MaxQuantity = Constants.MAX_STONE_QUANTITY;
        }
    }
}
