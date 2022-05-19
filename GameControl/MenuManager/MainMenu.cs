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
        public List<Button> mainMenuButtons = new List<Button>();
        public List<Button> mainMenuExitButtons = new List<Button>();
        private SpriteFont exitFont;
        private Texture2D sprite;
        public bool wantToExit = false;
        public MainMenu()
        {
            Button PlayButton = new Button(new Rectangle(72, 72, 72, 36), "PLAY");
            PlayButton.OnClicking += ClickedPlay;
            mainMenuButtons.Add(PlayButton);

            Button ExitButton = new Button(new Rectangle(72, 396, 72, 36), "EXIT");
            ExitButton.OnClicking += ClickedExit;
            mainMenuButtons.Add(ExitButton);

            Button YesExitButton = new Button(new Rectangle(500, 250, 144, 72), "YES");
            YesExitButton.OnClicking += ClickedYesExitGame;
            mainMenuExitButtons.Add(YesExitButton);

            Button NoExitButton = new Button(new Rectangle(175, 250, 144, 72), "NO");
            NoExitButton.OnClicking += ClickedNoExitGame;
            mainMenuExitButtons.Add(NoExitButton);



            sprite = GameWorld.Instance.Content.Load<Texture2D>("mainMenuBackground");
            exitFont = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }



        private void ClickedPlay(object sender, EventArgs e)
        {
            GameControl.Instance.ChangeGameState(GameState.Playing);
        }
        private void ClickedExit(object sender, EventArgs e)
        {
            wantToExit = true;
        }
        private void ClickedYesExitGame(object sender, EventArgs e)
        {
            GameWorld.Instance.Exit();
        }
        private void ClickedNoExitGame(object sender, EventArgs e)
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
