using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public abstract class ResourceDepot : Component
    {
        Random rand = new Random();

        protected int maxAmount;
        protected int amount;
        protected Point position;
        protected Cell myCell;


        public ResourceDepot(Point position, int maxAmount)
        {
            this.maxAmount = maxAmount;
            this.myCell = GameWorld.Cells[position];
            this.amount = rand.Next(2, maxAmount);

        }
    }
}
