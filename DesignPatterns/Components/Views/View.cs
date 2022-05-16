using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjekt2022
{
    public class View : Viewer
    {

     //   public float FPS { get; private set; }
        public string Name { get; private set; }
        public Texture2D[] Sprites { get; private set; }

        public View(string name, Texture2D[] sprites)
        {
            Name = name;
            Sprites = sprites;
           
        }
    }




}
