using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace EksamensProjekt2022
{
    public class GameControl
    {
        public StartScreen startScreen = new StartScreen();
        public Playing playing = new Playing();
        public CurrentGameState currentGameState = CurrentGameState.StartMenu;

        public GameState selectedGameState;
        public GameState previousGameState;
        public bool switchingGameState = false;
        public Camera camera;

        private static GameControl instance;
        public static GameControl Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameControl();
                }
                return instance;
            }

        }
    
        private GameControl()
        {
            camera = new Camera();
        }

        public void ChangeGameState(CurrentGameState gameState)
        {
            
            previousGameState = selectedGameState;
            currentGameState = gameState;
            previousGameState.EndingGameState();
            

        }

        public void UpdateGameState(GameTime gameTime)
        {
            if (!switchingGameState)
            {
                switch (currentGameState)
                {

                    case CurrentGameState.StartMenu:
                        startScreen.Update(gameTime);
                        break;
                    case CurrentGameState.Playing:
                        playing.Update(gameTime);
                        break;
                }

            }
        }

    }
}

