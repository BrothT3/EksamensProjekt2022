using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Text;

namespace EksamensProjekt2022
{
    public class PauseMenu
    {
        public List<Button> pauseMenuButtons = new List<Button>();
        public List<Button> pauseMenuExitButtons = new List<Button>();
        private SpriteFont exitFont;
        private Texture2D sprite;
        public bool wantToExit = false;
        public PauseMenu()
        {
            Button ResumeButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 236, 108, 72), "RESUME");
            ResumeButton.OnClicking += ClickedResume;
            pauseMenuButtons.Add(ResumeButton);

            Button ExitButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 36, 108, 72), "EXIT");
            ExitButton.OnClicking += ClickedExit;            
            pauseMenuButtons.Add(ExitButton);

            Button YesExitButton = new Button(new Rectangle(500, 250, 144, 72), "YES");
            YesExitButton.OnClicking += ClickedYesExitGame;
            pauseMenuExitButtons.Add(YesExitButton);

            Button NoExitButton = new Button(new Rectangle(175, 250, 144, 72), "NO");
            NoExitButton.OnClicking += ClickedNoExitGame;
            pauseMenuExitButtons.Add(NoExitButton);

            sprite = GameWorld.Instance.Content.Load<Texture2D>("mainMenuBackground");
            exitFont = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }



        private void ClickedResume(object sender, EventArgs e)
        {
            InputHandler.Instance.mLeftReleased = false;
            GameControl.Instance.paused = false;
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
