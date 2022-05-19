using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace EksamensProjekt2022
{
    public class GameControl
    {
        public GameState currentGameState = GameState.MainMenu;
        private GameState nextGameState;
        private bool initializeGameState = true;
        private bool endingGameState = false;
        public bool paused = false;
        public bool enterReleased;
        #region References
        public Camera camera;
        public DebugTool _debugTools;
        private AreaManager areaManager;
        public TimeManager timeManager;
        public MainMenu mainmenu;
        public PauseMenu pauseMenu;
        #endregion

        #region Lists
        public List<GameObject> currentGameObjects = new List<GameObject>();
        public List<GameObject> newGameObjects = new List<GameObject>();
        private List<GameObject> destroyedGameObjects = new List<GameObject>();
        public Dictionary<Point, Cell> currentCells = new Dictionary<Point, Cell>();
        public List<Collider> Colliders = new List<Collider>();
        #endregion

        public Grid grid;
        public List<Cell> currentGrid;
        public int CellSize { get; set; }
        public int CellCount { get; set; }

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
            CellCount = 80;
            CellSize = 36;
            grid = new Grid(CellCount, CellSize, CellSize);
            camera = new Camera();
            
            _debugTools = new DebugTool();
            
            areaManager = new AreaManager();
        }


        public void ChangeGameState(GameState gameState)
        {
            endingGameState = true;
            nextGameState = gameState;
            
            
        }

        public void UpdateGameState(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.MainMenu:
                    StartMenu(gameTime);
                    break;
                case GameState.Playing:
                    Playing(gameTime);
                    break;
                case GameState.PauseMenu:
                    PauseMenu();
                    break;
                case GameState.End:
                    End();
                    break;
            }

        }
        public void StartMenu(GameTime gameTime)
        {
            if (endingGameState) //det data den resetter før den ændre GameState
            {


                mainmenu = null;
                initializeGameState = true;
                currentGameState = nextGameState;
                endingGameState = false;
            }
            
            else if (!endingGameState && initializeGameState) //tilsvarer den "Initialize". bliver kun kørt en gang i starten af GameState når den har skiftet
            {
                mainmenu = new MainMenu();
                initializeGameState = false;
            }
            else //tilsvarer dens Update
            {

                foreach (Button item in mainmenu.mainMenuButtons)
                {
                    item.Update(gameTime);
                }
                if (mainmenu.wantToExit)
                {
                    foreach (Button item in mainmenu.mainMenuExitButtons)
                    {
                        item.Update(gameTime);
                    }
                }


            }

        }

        public void Playing(GameTime gameTime)
        {
            if (endingGameState) //det data den resetter før den ændre GameState
            {

                pauseMenu = null;
                endingGameState = false;
                initializeGameState = true;
                currentGameState = nextGameState;
            }
            else if (!endingGameState && initializeGameState) //tilsvarer den "Initialize". bliver kun kørt en gang i starten af GameState når den har skiftet
            {
                areaManager.currentGrid[0] = grid.CreateGrid();
                areaManager.currentCells[0] = grid.CreateCells();
                //add player
                GameObject player = new GameObject();
                player.AddComponent(new Player());
                player.AddComponent(new SpriteRenderer());
                player.AddComponent(new Collider());
                Instantiate(player);
                currentGrid = areaManager.currentGrid[0];
                currentCells = areaManager.currentCells[0];
                currentGameObjects = areaManager.currentGameObjects[0];
                timeManager = new TimeManager();
                timeManager.LoadContent();
                pauseMenu = new PauseMenu();
                for (int i = 0; i < currentGameObjects.Count; i++)
                {
                    currentGameObjects[i].Awake();
                    currentGameObjects[i].Start();
                }

                foreach (Cell c in currentGrid)
                {
                    c.LoadContent();
                }
                initializeGameState = false;


            }
            else //tilsvarer dens Update
            {
                if (!paused)
                {
                    _debugTools.Update(gameTime);
                    timeManager.Update(gameTime);

                    for (int i = 0; i < currentGameObjects.Count; i++)
                    {
                        currentGameObjects[i].Update(gameTime);
                    }
                    foreach (Cell c in currentGrid)
                    {
                        c.Update(gameTime);
                    }

                    CleanUp();

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && enterReleased == true)
                    {
                        enterReleased = false;
                        paused = true;
                    }
                    enterReleased = true;
                }
                if (paused)
                {

                }
                enterReleased = true;
                foreach (Button item in pauseMenu.pauseMenuButtons)
                {
                    item.Update(gameTime);
                }
                if (pauseMenu.wantToExit)
                {
                    foreach (Button item in pauseMenu.pauseMenuExitButtons)
                    {
                        item.Update(gameTime);
                    }
                }

            }
            
            
        }
        public void PauseMenu()
        {

        }
        public void End()
        {

        }

        

        /// <summary>
        /// Adds new objects to gameObjects list, runs Awake and Start.
        /// while also removing all the objects that are destroyed before clearing both lists
        /// </summary>
        public void CleanUp()
        {
            for (int i = 0; i < newGameObjects.Count; i++)
            {
                currentGameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();

            }

            for (int i = 0; i < destroyedGameObjects.Count; i++)
            {
                currentGameObjects.Remove(destroyedGameObjects[i]);
            }

            destroyedGameObjects.Clear();
            newGameObjects.Clear();

        }
        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
        }
    }
}
        
