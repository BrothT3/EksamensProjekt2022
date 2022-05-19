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
        public List<Button> PauseMenuButtons = new List<Button>();
        public List<Button> PauseExitButtons = new List<Button>();
        private SpriteFont exitFont;
        private Texture2D sprite;
        public bool wantToExit = false;
        public PauseMenu()
        {
            Button TestButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 236, 108, 72), "RESUME");
            TestButton.CLICK += RESUME;
            Button TestButton2 = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 36, 108, 72), "EXIT");
            TestButton2.CLICK += EXIT;
            PauseMenuButtons.Add(TestButton);
            PauseMenuButtons.Add(TestButton2);
            Button EXITYES = new Button(new Rectangle(500, 250, 144, 72), "YES");
            EXITYES.CLICK += EXITGAME;
            PauseExitButtons.Add(EXITYES);
            Button EXITNO = new Button(new Rectangle(175, 250, 144, 72), "NO");
            EXITNO.CLICK += DONTEXITGAME;
            PauseExitButtons.Add(EXITNO);



            sprite = GameWorld.Instance.Content.Load<Texture2D>("mainMenuBackground");
            exitFont = GameWorld.Instance.Content.Load<SpriteFont>("Font");
        }



        private void RESUME(object sender, EventArgs e)
        {
            InputHandler.Instance.mLeftReleased = false;
            GameControl.Instance.paused = false;
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
