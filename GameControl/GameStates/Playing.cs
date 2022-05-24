﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class Playing : GameState
    {
        public UserInterface userInterface;
        public AreaManager areaManager;
        public TimeManager timeManager;
        public DebugTool _debugTools;
        #region Lists
        public List<GameObject> currentGameObjects = new List<GameObject>();
        public List<GameObject> newGameObjects = new List<GameObject>();
        private List<GameObject> destroyedGameObjects = new List<GameObject>();
        public Dictionary<Point, Cell> currentCells = new Dictionary<Point, Cell>();
        public List<Collider> Colliders = new List<Collider>();
        public Grid grid;
        public List<Cell> currentGrid;
        public List<Button> pauseMenuButtons = new List<Button>();
        public List<Button> pauseMenuExitButtons = new List<Button>();
        private SpriteFont exitFont;
        private Texture2D sprite;
        public int CellSize { get; set; }
        public int CellCount { get; set; }
        #endregion
        public bool paused = false;
        private bool offsetPauseButtons;
        public bool enterReleased;
        public bool pauseWantToExit = false;
        private Vector2 buttonOffset;

        public Playing()
        {
            CellCount = 80;
            CellSize = 36;
            grid = new Grid(CellCount, CellSize, CellSize);
            _debugTools = new DebugTool();
            areaManager = new AreaManager();
        }
        public override void EndingGameState()
        {
            paused = false;
            pauseWantToExit = false;
            GameControl.Instance.camera.Position = new Vector2(0, 0);
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            if (player != null)
                Destroy(player.GameObject);

            initializeGameState = true;
            //GameControl.Instance.currentGameState = GameControl.Instance.nextGameState;

        }
        public override void Initialize()
        {
            GameControl.Instance.selectedGameState = this;
            areaManager.currentGrid[0] = grid.CreateGrid();
            areaManager.currentCells[0] = grid.CreateCells();


            if (!MapCreator.DevMode)
            {
                MapManager m = new MapManager();
                m.GetDbInfo(SaveSlots.Slot1, CurrentArea.Camp);

                GameObject player = new GameObject();
                Player p = (player).AddComponent(new Player()) as Player;
                p.MyArea = CurrentArea.Camp;
                player.AddComponent(new SpriteRenderer());
                player.AddComponent(new Collider());

                player.AddComponent(new Inventory());
               

                Instantiate(player);
                Inventory inv = player.GetComponent<Inventory>() as Inventory;
                inv.items.Add(new Stone(3));
                timeManager = new TimeManager();
                timeManager.LoadContent();


                currentGrid = areaManager.currentGrid[0];
                currentCells = areaManager.currentCells[0];
                currentGameObjects = areaManager.currentGameObjects[0];

                //TODO Dette er midlertidig, skal have en metode der flytter objekterne over
                for (int i = 0; i < m.areaLoader.currentGameObjects[(int)p.MyArea].Count; i++)
                {
                    currentGameObjects.Add(m.areaLoader.currentGameObjects[0][i]);
                }


                userInterface = new UserInterface();

            }
            else
            {
                MapManager m = MapCreator.Instance.mapManager;
                m.GetDbInfo(SaveSlots.Slot1, CurrentArea.Camp);

                MapCreator.Instance.mapManager.areaLoader.currentGrid[0] = grid.CreateGrid();
                MapCreator.Instance.mapManager.areaLoader.currentCells[0] = grid.CreateCells();
                for (int i = 0; i < m.areaLoader.currentGameObjects[0].Count; i++)
                {
                    currentGameObjects.Add(m.areaLoader.currentGameObjects[0][i]);
                }
                
                currentGrid = MapCreator.Instance.mapManager.areaLoader.currentGrid[0];
                currentCells = MapCreator.Instance.mapManager.areaLoader.currentCells[0];
                currentGameObjects = MapCreator.Instance.mapManager.areaLoader.currentGameObjects[0];
            }






            for (int i = 0; i < currentGameObjects.Count; i++)
            {
                currentGameObjects[i].Awake();
                currentGameObjects[i].Start();
            }

            foreach (Cell c in currentGrid)
            {
                c.LoadContent();
            }
            Button ResumeButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 300, 108, 72), "RESUME");
            ResumeButton.OnClicking += ClickedResume;
            pauseMenuButtons.Add(ResumeButton);

            Button ExitButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 + 36, 108, 72), "EXIT");
            ExitButton.OnClicking += ClickedExit;
            pauseMenuButtons.Add(ExitButton);

            Button MainMenuButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 136, 108, 72), "MAIN MENU");
            MainMenuButton.OnClicking += ClickedMainMenu;
            pauseMenuButtons.Add(MainMenuButton);

            Button YesExitButton = new Button(new Rectangle(500, 250, 144, 72), "YES");
            YesExitButton.OnClicking += ClickedYesExitGame;
            pauseMenuExitButtons.Add(YesExitButton);

            Button NoExitButton = new Button(new Rectangle(175, 250, 144, 72), "NO");
            NoExitButton.OnClicking += ClickedNoExitGame;
            pauseMenuExitButtons.Add(NoExitButton);


            exitFont = GameWorld.Instance.Content.Load<SpriteFont>("Font");




        }
        public override void Update(GameTime gameTime)
        {
            if (initializeGameState)
            {
                Initialize();
            }
            initializeGameState = false;
            if (!paused)
            {
                _debugTools.Update(gameTime);

                //TODO lav noget bedre
                if (!MapCreator.DevMode)
                {
                    timeManager.Update(gameTime);
                }


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
                    buttonOffset = new Vector2(GameControl.Instance.camera.Position.X, GameControl.Instance.camera.Position.Y);
                    paused = true;
                    offsetPauseButtons = true;
                }
                enterReleased = true;
            }
            else
            {
                if (offsetPauseButtons)
                {

                    foreach (Button item in GameControl.Instance.playing.pauseMenuButtons)
                        item.Rectangle = new Rectangle((int)item.DefaultbottonPos.X - (int)buttonOffset.X, (int)item.DefaultbottonPos.Y - (int)buttonOffset.Y, item.Rectangle.Width, item.Rectangle.Height);

                    foreach (Button item in GameControl.Instance.playing.pauseMenuExitButtons)
                        item.Rectangle = new Rectangle((int)item.DefaultbottonPos.X - (int)buttonOffset.X, (int)item.DefaultbottonPos.Y - (int)buttonOffset.Y, item.Rectangle.Width, item.Rectangle.Height);

                    offsetPauseButtons = false;
                }


                foreach (Button item in pauseMenuButtons)
                    item.Update(gameTime);

                if (pauseWantToExit)
                    foreach (Button item in pauseMenuExitButtons)
                        item.Update(gameTime);

            }

        }

        #region PauseMenu
        private void ClickedMainMenu(object sender, EventArgs e)
        {

            GameControl.Instance.ChangeGameState(CurrentGameState.StartMenu);
            MapCreator.DevMode = false;
        }

        private void ClickedResume(object sender, EventArgs e)
        {
            InputHandler.Instance.mLeftReleased = false;
            GameControl.Instance.playing.paused = false;
        }
        private void ClickedExit(object sender, EventArgs e)
        {
            pauseWantToExit = true;
        }
        private void ClickedYesExitGame(object sender, EventArgs e)
        {
            GameWorld.Instance.Exit();
        }
        private void ClickedNoExitGame(object sender, EventArgs e)
        {
            pauseWantToExit = false;
        }
        public void Draw(SpriteBatch spriteBatch)
        {

            if (pauseWantToExit)
            {
                spriteBatch.DrawString(exitFont, "ARE U SURE TO WANT TO EXIT NOW!?", new Vector2(300, 400), Color.White);
            }
            if (paused)
            {
                foreach (Button item in GameControl.Instance.playing.pauseMenuButtons)
                {
                    item.Draw(spriteBatch);
                }
                if (pauseWantToExit)
                {
                    foreach (Button item in GameControl.Instance.playing.pauseMenuExitButtons)
                    {
                        item.Draw(spriteBatch);
                    }
                }
            }
            if (userInterface != null)
            userInterface.Draw(spriteBatch);

        }
        #endregion

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
