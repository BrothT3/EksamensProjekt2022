using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class Wood : Item
    {

        public Wood(int _quantity)
        {
            Name = "wood";
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("AreaSprites/Tree");
            Quantity = _quantity;


            this.MaxQuantity = Constants.MAX_WOOD_QUANTITY;
        }

        
    }
}
