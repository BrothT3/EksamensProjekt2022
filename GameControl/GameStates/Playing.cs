using Microsoft.Xna.Framework;
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
        private bool hasSaved = false;
        private bool hasLoadedInventory = false;
        public MapManager _mapManager;
        private Vector2 buttonOffset;
        public Cell selectedCell;

        public Playing()
        {
            CellCount = 40;
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
            selectedCell = null;
            initializeGameState = true;
        }


        public override void Initialize()
        {
            GameControl.Instance.selectedGameState = this;
            areaManager.currentGrid[0] = grid.CreateGrid();
            areaManager.currentCells = grid.CreateCells();


            if (!MapCreator.DevMode)
            {




                timeManager = new TimeManager();
                timeManager.LoadContent();
                currentGrid = areaManager.currentGrid[0];
                currentCells = areaManager.currentCells;
                currentGameObjects = areaManager.currentGameObjects[0];


                _mapManager = new MapManager();
                _mapManager.GetDbInfo(SaveSlots.Slot1, Area.Camp);
                _mapManager.GetDbInfo(SaveSlots.Slot1, Area.River);
                _mapManager.GetDbInfo(SaveSlots.Slot1, Area.Hills);
                _mapManager.GetDbInfo(SaveSlots.Slot1, Area.Desert);
                _mapManager.LoadPlayer(_mapManager.CurrentSave);


                for (int i = 0; i < _mapManager.areaLoader.currentGameObjects[0].Count; i++)
                {
                    areaManager.currentGameObjects[i] = _mapManager.areaLoader.currentGameObjects[i];
                    areaManager.currentGrid[i] = _mapManager.areaLoader.currentGrid[i];

                }

                for (int i = 0; i < currentGameObjects.Count; i++)
                {
                    currentGameObjects[i].Awake();
                    currentGameObjects[i].Start();
                }
                foreach (Cell cell in currentGrid)
                {
                    cell.LoadContent();
                }

                #region ToBeRemoved
                userInterface = new UserInterface();
                GameObject chest = new GameObject();
                Chest c = new Chest(new Point(7, 7));
                SpriteRenderer sr = new SpriteRenderer();
                sr.SetSprite("chest");
                Inventory inv = new Inventory(10);
                chest.AddComponent(sr);
                chest.AddComponent(c);
                chest.AddComponent(inv);


                Instantiate(chest);

                GameObject craftingTable = new GameObject();
                SpriteRenderer ctsr = new SpriteRenderer();
                CraftingTable ct = new CraftingTable(new Point(11, 11));
                CraftingMenu cm = new CraftingMenu();
                Inventory ctinv = new Inventory(2);
                ctsr.SetSprite("craftingTable");

                cm.AddRecipe(new FermentedBreastMilkRecipe());
                cm.AddRecipe(new ChestRecipe());
                craftingTable.AddComponent(cm);
                craftingTable.AddComponent(ct);
                craftingTable.AddComponent(ctsr);
                craftingTable.AddComponent(ctinv);
                ctinv.AddItem(new Wood(8));
                ctinv.AddItem(new Stone(3));

                Instantiate(craftingTable);

                GameObject campfire = new GameObject();
                SpriteRenderer cfsr = new SpriteRenderer();
                CampFire cf = new CampFire(new Point(4, 5));
                Inventory cfinv = new Inventory(2);
                CraftingMenu cfcm = new CraftingMenu();
                cfsr.SetSprite("campFire");
                cfcm.AddRecipe(new CookedFishRecipe());
                campfire.AddComponent(cf);
                campfire.AddComponent(cfinv);
                campfire.AddComponent(cfcm);
                campfire.AddComponent(cfsr);

                Instantiate(campfire);
                #endregion

            }
            else
            {


                currentGrid = areaManager.currentGrid[0];
                currentCells = areaManager.currentCells;
                currentGameObjects = areaManager.currentGameObjects[0];

                MapManager m = MapCreator.Instance.mapManager;
                m.GetDbInfo(SaveSlots.Slot1, Area.Camp);
                m.GetDbInfo(SaveSlots.Slot1, Area.River);
                m.GetDbInfo(SaveSlots.Slot1, Area.Hills);
                m.GetDbInfo(SaveSlots.Slot1, Area.Desert);

                for (int i = 1; i < m.areaLoader.currentGameObjects[0].Count; i++)
                {

                    areaManager.currentGameObjects[i] = m.areaLoader.currentGameObjects[i];
                    areaManager.currentGrid[i] = m.areaLoader.currentGrid[i];

                }


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

            #region ButtonCreation
            Button ResumeButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 300, 108, 72), "RESUME");
            ResumeButton.OnClicking += ClickedResume;
            pauseMenuButtons.Add(ResumeButton);

            Button ExitButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 + 36, 108, 72), "EXIT");
            ExitButton.OnClicking += ClickedExit;
            pauseMenuButtons.Add(ExitButton);

            Button MainMenuButton = new Button(new Rectangle(GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 54, GameWorld.Instance.Graphics.PreferredBackBufferWidth / 2 - 136, 108, 72), "MAIN MENU");
            MainMenuButton.OnClicking += ClickedMainMenu;
            pauseMenuButtons.Add(MainMenuButton);

            Button YesExitButton = new Button(new Rectangle(500, 250, 50, 50), "YES");
            YesExitButton.OnClicking += ClickedYesExitGame;
            pauseMenuExitButtons.Add(YesExitButton);

            Button NoExitButton = new Button(new Rectangle(500, 320, 50, 50), "NO");
            NoExitButton.OnClicking += ClickedNoExitGame;
            pauseMenuExitButtons.Add(NoExitButton);

            #endregion

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

                if (!MapCreator.DevMode)
                {
                    timeManager.Update(gameTime);
                }

                userInterface.Update(gameTime);


                for (int i = 0; i < currentGameObjects.Count; i++)
                {
                    currentGameObjects[i].Update(gameTime);
                }
                foreach (Cell c in currentGrid)
                {
                    c.Update(gameTime);
                }

                CleanUp();

                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && enterReleased == true)
                {

                    enterReleased = false;
                    buttonOffset = new Vector2(GameControl.Instance.camera.Position.X, GameControl.Instance.camera.Position.Y);
                    paused = true;
                    offsetPauseButtons = true;
                    hasSaved = false;
                }
                enterReleased = true;
                Player p = (Player)GameWorld.Instance.FindObjectOfType<Player>() as Player;
                if (p != null)
                {
                    PlayerChangeArea(p);
                }
                hasSaved = false;
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

                  

                    
                
                    

                //save inventory
                if (!hasSaved)
                {
                    Player p = (Player)GameWorld.Instance.FindObjectOfType<Player>() as Player;
                    if (p != null)
                    {
                        Inventory inv = p.GameObject.GetComponent<Inventory>() as Inventory;
                        //hasSaved = true;
                        _mapManager.SavePlayerInventory(inv.items);
                       // _mapManager.SaveComponents(currentGameObjects, currentGrid, _mapManager.CurrentSave, p.MyArea);
                       
                        for (int i = 0; i < areaManager.currentGrid.Length; i++)
                        {
                            _mapManager.SaveComponents(areaManager.currentGameObjects[i], areaManager.currentGrid[i], _mapManager.CurrentSave, (Area)i);
                        }
                        hasSaved = true;
                    }

                    //GameObject player;
                    //foreach (GameObject go in currentGameObjects)
                    //{
                    //    if (go.GetComponent<Player>() != null)
                    //    {
                    //        player = go;
                    //        Inventory inv = player.GetComponent<Inventory>() as Inventory;
                    //        _mapManager.SavePlayerInventory(inv.items);
                    //        hasSaved = true;
                    //    }
                    //}



                }




            }

            if (!hasLoadedInventory && GameWorld.Instance.FindObjectOfType<Player>() != null)
            {
                _mapManager.LoadPlayerInventory(_mapManager.CurrentSave);
                hasLoadedInventory = true;
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
                spriteBatch.DrawString(exitFont, "Are you sure you want to exit?", new Vector2(420, 400), Color.White);
            }
            if (paused && !pauseWantToExit)
            {
                foreach (Button item in GameControl.Instance.playing.pauseMenuButtons)
                {
                    item.Draw(spriteBatch);
                }

            }
            if (pauseWantToExit)
            {
                foreach (Button item in GameControl.Instance.playing.pauseMenuExitButtons)
                {
                    item.Draw(spriteBatch);
                }
            }
            if (userInterface != null)
                userInterface.Draw(spriteBatch);
        }
        #endregion

        public void PlayerChangeArea(Player p)
        {


            if (p.currentCell.IsAreaChangeCell)
            {
                Cell c = p.currentCell;
                if (c.Position.X == 0 && c.Position.Y == CellCount / 2 && p.MyArea != Area.River||
                    c.Position.X == 0 && c.Position.Y == CellCount / 2 -1 && p.MyArea != Area.River)
                {
                    currentGameObjects.Remove(p.GameObject);
                    areaManager.AreaChange(p.MyArea, Area.River);
                    p.MyArea = Area.River;
                    currentGameObjects.Add(p.GameObject);
                }

                if (c.Position.X == CellCount/2 && c.Position.Y == 0 && p.MyArea != Area.Hills||
                    c.Position.X == CellCount/2 +1 && c.Position.Y == 0 && p.MyArea != Area.Hills)
                {
                    currentGameObjects.Remove(p.GameObject);
                    areaManager.AreaChange(p.MyArea, Area.Hills);
                    p.MyArea = Area.Hills;
                    currentGameObjects.Add(p.GameObject);
                }

                if (c.Position.X == CellCount -1 && c.Position.Y == CellCount /2 && p.MyArea != Area.Camp||
                    c.Position.X == CellCount -1 && c.Position.Y == CellCount /2 -1 && p.MyArea != Area.Camp)
                {
                    currentGameObjects.Remove(p.GameObject);
                    areaManager.AreaChange(p.MyArea, Area.Camp);
                    p.MyArea = Area.Camp;
                    currentGameObjects.Add(p.GameObject);
                }

                if (c.Position.X == CellCount/2 && c.Position.Y == CellCount-1 && p.MyArea != Area.Desert
                    || c.Position.X == CellCount/2-1 && c.Position.Y == CellCount-1 && p.MyArea != Area.Desert)
                {
                    currentGameObjects.Remove(p.GameObject);
                    areaManager.AreaChange(p.MyArea, Area.Desert);
                    p.MyArea = Area.Desert;
                    currentGameObjects.Add(p.GameObject);
                }
            }

        }

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
