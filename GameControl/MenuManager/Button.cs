using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EksamensProjekt2022
{
    public class Button
    {

        private SpriteFont buttonFont;
        private Texture2D sprite;
        private bool isHovering;
        private bool isAvailable;
        private bool isOffset;
        private Color hoverColor = Color.White;
        private Rectangle rectangle;
        private Vector2 defaultbottonPos;
        private string buttonText;
        private Item item;
        public Rectangle Rectangle { get => rectangle; set => rectangle = value; }
        public Vector2 DefaultbottonPos { get => defaultbottonPos; set => defaultbottonPos = value; }
        public MouseState mstate { get; set; }
        public bool IsOffset { get => isOffset; set => isOffset = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
        public string ButtonText { get => buttonText; set => buttonText = value; }
        public Item Item { get => item; set => item = value; }

        private bool mReleased = true;
        public event EventHandler OnClicking;

        public Button(Rectangle ButtonRectangle, string buttonText)
        {
            Rectangle = ButtonRectangle;
            DefaultbottonPos = new Vector2(ButtonRectangle.X, ButtonRectangle.Y);
            this.ButtonText = buttonText;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
            buttonFont = GameWorld.Instance.Content.Load<SpriteFont>("Font");
            IsOffset = false;
            IsAvailable = true;
        }

        public Button(string buttonText)
        {
            this.ButtonText = buttonText;
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Pixel");
            buttonFont = GameWorld.Instance.Content.Load<SpriteFont>("Font");
            IsOffset = false;
        }
        public Button(Item item)
        {
            IsOffset = false;
            Item = item;
            isAvailable = true;
        }

        public void Update(GameTime gameTime)
        {
            Hovering();
            

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, Rectangle, hoverColor);
            spriteBatch.DrawString(buttonFont, ButtonText, new Vector2(rectangle.Center.X - (buttonFont.MeasureString(ButtonText).X / 2), rectangle.Center.Y - (buttonFont.MeasureString(ButtonText).Y) / 2), Color.Black);

        }

        public virtual void Hovering()
        {
            mstate = Mouse.GetState();
            if (rectangle.Contains(new Vector2(mstate.X - (int)GameControl.Instance.camera.Position.X, mstate.Y - (int)GameControl.Instance.camera.Position.Y)) && isAvailable)
            {
                isHovering = true;
                hoverColor = Color.Wheat;
                if (mstate.LeftButton == ButtonState.Pressed && mReleased == true)
                {
                    mReleased = false;
                    OnClicking?.Invoke(this, new EventArgs());
                }
                
            }
            else
            {
                isHovering = false;
                hoverColor = Color.Cornsilk;
            }
            if (mstate.LeftButton == ButtonState.Released)
            {
                mReleased = true;
            }
        }
    }
   
}
