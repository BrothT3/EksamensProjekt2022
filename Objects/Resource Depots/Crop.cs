using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace EksamensProjekt2022
{
    public class Crop : ResourceDepot
    {
        private Thread growthCycle;

        private GrowthState _growthState;
        public Crop(Cell _mycell, int maxAmount) : base(_mycell, maxAmount)
        {
            this._growthState = GrowthState.Sprout;
            growthCycle = new Thread(Growth);
            growthCycle.IsBackground = true;
            growthCycle.Start();
        }

        public void Growth()
        {
            _growthState = GrowthState.Sprout;
            Thread.Sleep(6000);
            _growthState = GrowthState.Budding;
            Thread.Sleep(6000);
            _growthState = GrowthState.Flowering;
            Thread.Sleep(6000);
            _growthState = GrowthState.Ripe;
            
        }
    }
}
