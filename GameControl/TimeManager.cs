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
        private float dayTimer = 15;
        private float duskTimer = 5;
        private float NightTimer = 10;
        private float timeColor = 0f;

        public TimeManager()
        {
            currentTime = CurrentTime.Day;
        }

        public void Update(GameTime gameTime)
        {

            switch (currentTime)
            {
                case CurrentTime.Day:
                    dayTimer -= GameWorld.DeltaTime;
                    timeColor = 0f;
                    if (dayTimer <= 0)
                    {
                        currentTime = CurrentTime.Dusk;
                        dayTimer = 10;
                    }
                    break;
                case CurrentTime.Dusk:
                    duskTimer -= GameWorld.DeltaTime;
                    timeColor = 0.25f;
                    if (duskTimer <= 0)
                    {
                        currentTime = CurrentTime.Night;
                        duskTimer = 10;
                    }
                    break;
                case CurrentTime.Night:
                    NightTimer -= GameWorld.DeltaTime;
                    timeColor = 0.5f;
                    if (NightTimer <= 0)
                    {
                        currentTime = CurrentTime.Day;
                        NightTimer = 10;
                    }
                    break;

            }
        }
        public void LoadContent()
        {
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
        }
        public void draw(SpriteBatch spriteBatch)
        {
            
            spriteBatch.Draw(sprite, new Rectangle(0, 0, GameControl.Instance.playing.CellCount * GameControl.Instance.playing.CellSize, GameControl.Instance.playing.CellCount * GameControl.Instance.playing.CellSize), Color.Black * timeColor);
            
        }
    }
}
