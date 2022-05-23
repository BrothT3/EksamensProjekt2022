using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public abstract class GameState
    {

        public bool initializeGameState = true;
        public virtual void EndingGameState()
        {

        }
        public virtual void Initialize()
        {

        }
        public virtual void Update(GameTime gameTime)
        {

            
        }
    }
}
