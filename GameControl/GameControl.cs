using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class GameControl
    {
        public GameState currentGameState = GameState.MainMenu;
        private GameState nextGameState;
        private bool initializeGameState = false;
        private bool endingGameState = false;

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

        public void ChangeGameState(GameState gameState)
        {
            nextGameState = gameState;
            endingGameState = true;
            
        }

        public void UpdateGameState()
        {
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    MainMenu();
                    break;
                case GameState.Begin:
                    Begin();
                    break;
                case GameState.Playing:
                    Playing();
                    break;
                case GameState.PauseMenu:
                    PauseMenu();
                    break;
                case GameState.End:
                    End();
                    break;
            }

        }
        public void MainMenu()
        {
            if (endingGameState) //det data den resetter før den ændre GameState
            {

                
                endingGameState = false;
                initializeGameState = true;
                currentGameState = nextGameState;
            }
            else if (!endingGameState && initializeGameState) //tilsvarer den "Initialize". bliver kun kørt en gang i starten af GameState når den har skiftet
            {

                initializeGameState = false;
            }
            else //tilsvarer dens Update
            {
                
            }

        }
        public void Begin()
        {


        }
        public void Playing()
        {

        }
        public void PauseMenu()
        {

        }
        public void End()
        {

        }
    }
}
        
