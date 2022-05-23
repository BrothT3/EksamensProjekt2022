using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class TimeManager
    {
        public CurrentTime currentTime;
        private Texture2D sprite;
        private float maxTimeTimer = 12;
        private float timeTimer = 12;
        private float timeColor = 0f;

        public TimeManager()
        {
            currentTime = CurrentTime.Day;
        }

        public void Update(GameTime gameTime)
        {

            timeTimer -= GameWorld.DeltaTime;
            if (timeTimer >= (maxTimeTimer / 2))
            {
                currentTime = CurrentTime.Day;
                timeColor = 0f;
            }
            if (timeTimer <= maxTimeTimer/2 && timeTimer >= maxTimeTimer/3)
            {
                currentTime = CurrentTime.Dusk;
                timeColor = 0.25f;
            }
            if (timeTimer <= maxTimeTimer / 3)
            {
                currentTime = CurrentTime.Night; 
                timeColor = 0.5f;
            }
            if (timeTimer <= 0)
            {
                timeTimer = maxTimeTimer;
            }
            
        }
    public void LoadContent()
    {
        sprite = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(sprite, new Rectangle(0, 0, Playing.Instance.CellCount * Playing.Instance.CellSize, Playing.Instance.CellCount * Playing.Instance.CellSize), Color.Black * timeColor);
    }
}
}
