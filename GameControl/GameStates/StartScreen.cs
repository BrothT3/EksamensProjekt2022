using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class StartScreen : GameState
    {
        public List<Button> mainMenuButtons = new List<Button>();
        public List<Button> mainMenuExitButtons = new List<Button>();
        private SpriteFont exitFont;
        private Texture2D sprite;
        public bool startWantToExit = false;

        public override void EndingGameState()
        {
            startWantToExit = false;
            initializeGameState = true;

            //GameControl.Instance.currentGameState = GameControl.Instance.nextGameState;

        }
        public override void Initialize()
        {
            GameControl.Instance.selectedGameState = this;
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

            initializeGameState = false;

        }
        public override void Update(GameTime gameTime)
        {


            if (initializeGameState)
            {
                Initialize();
            }
            initializeGameState = false;

            foreach (Button item in mainMenuButtons)
            {
                item.Update(gameTime);
            }

            if (startWantToExit)
            {
                foreach (Button item in mainMenuExitButtons)
                {

                    item.Update(gameTime);
                }
            }
#if DEBUG
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                MapCreator.DevMode = true;
            }
#endif

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, new Vector2(0, 0), Color.White);
            if (startWantToExit)
            {
                spriteBatch.DrawString(exitFont, "ARE U SURE TO WANT TO EXIT NOW!?", new Vector2(300, 400), Color.White);
            }
            foreach (Button item in GameControl.Instance.startScreen.mainMenuButtons)
            {
                item.Draw(spriteBatch);
            }
            
            if (startWantToExit)
            {
                foreach (Button item in GameControl.Instance.startScreen.mainMenuExitButtons)
                {
                    item.Draw(spriteBatch);
                }
            }


        }
        private void ClickedPlay(object sender, EventArgs e)
        {
            GameControl.Instance.ChangeGameState(CurrentGameState.Playing);
        }
        private void ClickedExit(object sender, EventArgs e)
        {
            startWantToExit = true;
        }
        private void ClickedYesExitGame(object sender, EventArgs e)
        {
            GameWorld.Instance.Exit();
        }
        private void ClickedNoExitGame(object sender, EventArgs e)
        {            
            startWantToExit = false;
        }
    }
}
