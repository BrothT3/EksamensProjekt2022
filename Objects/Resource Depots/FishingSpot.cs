using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EksamensProjekt2022
{
    public class FishingSpot : ResourceDepot
    {
        private Thread taskThread;

        public FishingSpot(Cell _mycell, int maxAmount) : base(_mycell, maxAmount)
        {
            taskThread = new Thread(FishReplenishment);
            taskThread.IsBackground = true;
            taskThread.Start();
        }

        public void FishReplenishment()
        {
            while (true)
            {
                if (amount <= maxAmount)
                {
                    amount++;
                }
                Thread.Sleep(10000);
            }
        }
    }
}
