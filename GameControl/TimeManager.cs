using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public enum CurrentTime
    {
        Day,
        Dusk,
        Night
    }
    public class TimeManager
    {
        public CurrentTime currentTime;
        private Texture2D Sprite;
        private float dayTimer = 120;
        private float duskTimer = 15;
        private float NightTimer = 60;
        private float timeColor = 0f;

        public TimeManager()
        {
            this.currentTime = CurrentTime.Day;
        }

        public void Update(GameTime gameTime)
        {

            switch (this.currentTime)
            {
                case CurrentTime.Day:
                    dayTimer -= GameWorld.DeltaTime;
                    timeColor = 0f;
                    if (dayTimer <= 0)
                    {
                        currentTime = CurrentTime.Dusk;
                        dayTimer = 3;
                    }
                    break;
                case CurrentTime.Dusk:
                    duskTimer -= GameWorld.DeltaTime;
                    timeColor = 0.25f;
                    if (duskTimer <= 0)
                    {
                        currentTime = CurrentTime.Night;
                        duskTimer = 3;
                    }
                    break;
                case CurrentTime.Night:
                    NightTimer -= GameWorld.DeltaTime;
                    timeColor = 0.5f;
                    if (NightTimer <= 0)
                    {
                        currentTime = CurrentTime.Day;
                        NightTimer = 3;
                    }
                    break;

            }
        }
        public void LoadContent()
        {
            Sprite = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
        }
        public void draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(Sprite, new Rectangle(0, 0, GameWorld.Instance.CellCount * GameWorld.Instance.CellSize, GameWorld.Instance.CellCount * GameWorld.Instance.CellSize), Color.Black * timeColor);
            
        }
    }
}
