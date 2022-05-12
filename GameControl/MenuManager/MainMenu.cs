using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Text;

namespace EksamensProjekt2022
{
    public class MainMenu
    {
        public List<Button> Buttons = new List<Button>();
        public MainMenu()
        {
            //Button TestButton = new Button(new Rectangle(72, 72, 144, 72), "PLAY");
            //TestButton.CLICK += PLAY;
            //Button TestButton2 = new Button(new Rectangle(72, 396, 144, 72), "EXIT");
            //TestButton2.CLICK += EXIT;
            //Buttons.Add(TestButton);
            //Buttons.Add(TestButton2);

        }



        private void PLAY(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        private void EXIT(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        public void LoadContent()
        {

        }
        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }

}
