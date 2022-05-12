using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public Camera _camera;
        private DebugTool _debugTools;

        public List<GameObject> gameObjects = new List<GameObject>();
        public List<GameObject> currentGameObjects = new List<GameObject>();

        public List<GameObject> newGameObjects = new List<GameObject>();
        private List<GameObject> destroyedGameObjects = new List<GameObject>();


        static public Dictionary<Point, Cell> Cells = new Dictionary<Point, Cell>();
        public Dictionary<Point, Cell> currentCells = new Dictionary<Point, Cell>();

        public List<Collider> Colliders = new List<Collider>();

        private Astar pathfinder;
        private Cell start, goal;
        public List<Node> finalPath;

        public Grid _grid;
        public List<Cell> grid;
        public List<Cell> currentGrid;
        private int cellCount;
        private int cellSize;

        private AreaManager areaManager;
        private TimeManager timeManager;
        public static float DeltaTime;

        public GraphicsDeviceManager Graphics { get => _graphics; }
        public int CellSize { get => cellSize; set => cellSize = value; }
        public int CellCount { get => cellCount; set => cellCount = value; }

        private static GameWorld instance;
        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameWorld();
                }
                return instance;
            }

        }

        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            CellCount = 80;
            CellSize = 36;
            //opret gid, skal måske gøres på en anden måde
            _grid = new Grid(CellCount, CellSize, CellSize);

            _camera = new Camera();



        }

        protected override void Initialize()
        {

            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 600;
            Graphics.ApplyChanges();

            GameObject player = new GameObject();
            player.AddComponent(new Player());
            player.AddComponent(new SpriteRenderer());
            player.AddComponent(new Collider());
            Instantiate(player);
            _debugTools = new DebugTool();
            areaManager = new AreaManager();
            timeManager = new TimeManager();

            for (int i = 0; i < 4; i++)
            {
                areaManager.currentGrid[i] = _grid.CreateGrid();
                foreach (Cell item in areaManager.currentGrid[i])
                {
                    item.LoadContent();                   
                }
                areaManager.currentCells[i] = _grid.CreateCells();
                foreach (Cell item in areaManager.currentCells[i].Values)
                {
                    item.LoadContent();
                }

            }


            currentCells = areaManager.currentCells[0];
            currentGameObjects = areaManager.currentGameObjects[0];
            currentGrid = areaManager.currentGrid[0];

            for (int i = 0; i < 50; i++)
            {
                areaManager.currentGrid[1][250 + i].IsWalkable = false;

            }

            for (int i = 0; i < currentGameObjects.Count; i++)
            {
                currentGameObjects[i].Awake();
            }

            base.Initialize();

            ////instans af Astar, start og slut punkt (som så bliver spillerens position og hvor end man vil hen
            //pathfinder = new Astar();
            //start = Cells[new Point(15, 0)];
            //goal = Cells[new Point(1, 12)];
            ////kører algoritmen, den flyttes også ind i input/movement senere men bare for at det virker
            //FindPath();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < currentGameObjects.Count; i++)
            {
                currentGameObjects[i].Start();
            }
            foreach (Cell item in currentGrid)
            {
                item.LoadContent();
            }
            timeManager.LoadContent();


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _debugTools.Update(gameTime);
            KeyboardState keystate = Keyboard.GetState();
            if (keystate.IsKeyDown(Keys.W))
            {
                _camera.Move(new Vector2(0, 8));
            }
            if (keystate.IsKeyDown(Keys.A))
            {
                _camera.Move(new Vector2(8, 0));
            }
            if (keystate.IsKeyDown(Keys.D))
            {
                _camera.Move(new Vector2(-8, 0));
            }
            if (keystate.IsKeyDown(Keys.S))
            {
                _camera.Move(new Vector2(0, -8));
            }

            foreach (Cell item in currentGrid)
            {
                item.Update(gameTime);
            }
            for (int i = 0; i < currentGameObjects.Count; i++)
            {
                currentGameObjects[i].Update(gameTime);
            }
            timeManager.Update(gameTime);
            CleanUp();


            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            if(player != null && player.currentCell.Position == currentCells[new Point(1, 1)].Position)
            {
                player.MyArea = CurrentArea.River;
                areaManager.ChangeArea(player.MyArea);
                currentGameObjects.Remove(player.GameObject);
                currentCells = areaManager.currentCells[(int)player.MyArea];
                currentGrid = areaManager.currentGrid[(int)player.MyArea];
                currentGameObjects = areaManager.currentGameObjects[(int)player.MyArea];
                currentGameObjects.Add(player.GameObject);


            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            var screenScale = _camera.GetScreenScale();
            var viewMatrix = _camera.GetTransform();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                null, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));

            if (currentGrid != null)
                foreach (Cell item in currentGrid)
                {
                    item.Draw(_spriteBatch);

                }

            for (int i = 0; i < currentGameObjects.Count; i++)
            {
                currentGameObjects[i].Draw(_spriteBatch);



                if (currentGameObjects[i].GetComponent<Player>() == null)
                {
                    currentGameObjects[i].Transform.Position = new Vector2(currentGameObjects[i].Transform.Position.X - _camera.Position.X,
                        currentGameObjects[i].Transform.Position.Y - _camera.Position.Y);


                }
            }

            timeManager.draw(_spriteBatch);

            _debugTools.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Instantiate(GameObject go)
        {
            newGameObjects.Add(go);
        }

        public void Destroy(GameObject go)
        {
            destroyedGameObjects.Add(go);
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

        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in currentGameObjects)
            {
                Component c = gameObject.GetComponent<T>();

                if (c != null)
                {
                    return c;
                }
            }

            return null;


        }





    }
}
