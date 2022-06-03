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
        public int Amount { 
            get
            { 
                return amount;
            }
            set 
            { 
                amount = value; 
            }
        }


        public ResourceDepot(Cell _myCell, int maxAmount)
        {
            this.maxAmount = maxAmount;
            this.myCell = _myCell;
            this.amount = rand.Next(2, maxAmount);
            
        }

        public override void Update(GameTime gameTime)
        {
            if (amount <= 0)
            {
                GameControl.Instance.playing.Destroy(this.GameObject);
                myCell.IsWalkable = true;
            }
        }
    }
}
