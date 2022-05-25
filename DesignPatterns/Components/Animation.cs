using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    public class Animation
    {
        public float FPS { get; private set; }
        public string Name { get; private set; }
        public Texture2D[] Sprites { get; private set; }
        public Texture2D Sprite { get; private set; }
        public Rectangle[] Rectangles { get; private set; }

        /// <summary>
        /// Creates animation by combining several individual textures
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sprites"></param>
        /// <param name="fps"></param>
        public Animation(string name, Texture2D[] sprites, float fps)
        {
            Name = name;
            Sprites = sprites;
            FPS = fps;
        }
        /// <summary>
        /// Creates animation from sprite sheet
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Texture"></param>
        /// <param name="frames"></param>
        /// <param name="fps"></param>
        public Animation(string name, Texture2D Texture, int frames, float fps)
        {
            Name = name;
            Sprite = Texture;
            FPS = fps;
            int width = Sprite.Width / frames;
            Rectangles = new Rectangle[frames];
            for (int i = 0; i < frames; i++)
            {
                Rectangles[i] = new Rectangle(i * width, 0, width, Sprite.Height);
            }
            
        }
    }
}
