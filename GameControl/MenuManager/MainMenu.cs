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
        public List<Button> MainMenuButtons = new List<Button>();
        public List<Button> ExitButtons = new List<Button>();
        private SpriteFont exitFont;
        private Texture2D sprite;
        public bool wantToExit = false;
        public MainMenu()
        {
            Button TestButton = new Button(new Rectangle(72, 72, 72, 36), "PLAY");
            TestButton.CLICK += PLAY;
            Button TestButton2 = new Button(new Rectangle(72, 396, 72, 36), "EXIT");
            TestButton2.CLICK += EXIT;
            MainMenuButtons.Add(TestButton);
            MainMenuButtons.Add(TestButton2);
            Button EXITYES = new Button(new Rectangle(500, 250, 144, 72), "YES");
            EXITYES.CLICK += EXITGAME;
            ExitButtons.Add(EXITYES);
            Button EXITNO = new Button(new Rectangle(175, 250, 144, 72), "NO");
            EXITNO.CLICK += DONTEXITGAME;
            ExitButtons.Add(EXITNO);



            sprite = GameWorld.Instance.Content.Load<Texture2D>("mainMenuBackground");
            exitFont = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }



        private void PLAY(object sender, EventArgs e)
        {
            GameControl.Instance.ChangeGameState(GameState.Playing);
        }
        private void EXIT(object sender, EventArgs e)
        {
            wantToExit = true;
        }
        private void EXITGAME(object sender, EventArgs e)
        {
            GameWorld.Instance.Exit();
        }
        private void DONTEXITGAME(object sender, EventArgs e)
        {
            wantToExit = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Vector2(0, 0), Color.White);
            if (wantToExit)
            {
                spriteBatch.DrawString(exitFont, "ARE U SURE TO WANT TO EXIT NOW!?", new Vector2(300, 400), Color.White);
            }
        }
    }

}
