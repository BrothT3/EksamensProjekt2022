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

        #region References
        public Camera camera;


        
        
        #endregion
        #region Lists
        //public List<GameObject> currentGameObjects = new List<GameObject>();
        //public List<GameObject> newGameObjects = new List<GameObject>();
        //private List<GameObject> destroyedGameObjects = new List<GameObject>();
        //public Dictionary<Point, Cell> currentCells = new Dictionary<Point, Cell>();
        //public List<Collider> Colliders = new List<Collider>();
        #endregion
        //public Grid grid;
        //public List<Cell> currentGrid;
        //public int CellSize { get; set; }
        //public int CellCount { get; set; }

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
        #region oldGameControl()
        //private GameControl()
        //{
        //    CellCount = 80;
        //    CellSize = 36;
        //    grid = new Grid(CellCount, CellSize, CellSize);
        //    camera = new Camera();

        //    _debugTools = new DebugTool();

        //    areaManager = new AreaManager();
        //}
        #endregion
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



        #region old Cleanup, Instantiate, Destroy
        /// <summary>
        /// Adds new objects to gameObjects list, runs Awake and Start.
        /// while also removing all the objects that are destroyed before clearing both lists
        /// </summary>
        //public void CleanUp()
        //{
        //    for (int i = 0; i < newGameObjects.Count; i++)
        //    {
        //        currentGameObjects.Add(newGameObjects[i]);
        //        newGameObjects[i].Awake();
        //        newGameObjects[i].Start();

        //    }

        //    for (int i = 0; i < destroyedGameObjects.Count; i++)
        //    {
        //        currentGameObjects.Remove(destroyedGameObjects[i]);
        //    }

        //    destroyedGameObjects.Clear();
        //    newGameObjects.Clear();

        //}
        //public void Instantiate(GameObject go)
        //{
        //    newGameObjects.Add(go);
        //}

        //public void Destroy(GameObject go)
        //{
        //    destroyedGameObjects.Add(go);
        //}
        #endregion
    }
}

