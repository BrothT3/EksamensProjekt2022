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

        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> newGameObjects = new List<GameObject>();
        private List<GameObject> destroyedGameObjects = new List<GameObject>();
        static public Dictionary<Point, Cell> Cells = new Dictionary<Point, Cell>();

        private Astar pathfinder;
        private Cell start, goal;
        public List<Node> finalPath;

        public Grid _grid;
        public List<Cell> grid;


        public GraphicsDeviceManager Graphics { get => _graphics; }

        public static float DeltaTime;
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

            //opret gid, skal måske gøres på en anden måde
            _grid = new Grid(80, 35, 35);
            grid = _grid.CreateGrid();
            Cells = _grid.CreateCells();
            _camera = new Camera();
           
        }

        protected override void Initialize()
        {

            GameObject player = new GameObject();
            player.AddComponent(new Player());
            player.AddComponent(new SpriteRenderer());
            Instantiate(player);          
            _debugTools = new DebugTool();

           


            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Awake();
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

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Start();
            }
            foreach (Cell item in grid)
            {
                item.LoadContent();
            }



        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
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

            foreach (Cell item in grid)
            {
                item.Update(gameTime);
            }
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
            }

            CleanUp();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            var screenScale = _camera.GetScreenScale();
            var viewMatrix = _camera.GetTransform();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                null, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));

            if (grid != null)
                foreach (Cell item in grid)
                {
                    item.Draw(_spriteBatch);                 
                }

            //for (int i = 0; i < gameObjects.Count; i++)
            //{
            //    gameObjects[i].Draw(_spriteBatch);
                
            //}

            foreach (GameObject go in gameObjects)
            {
               
                go.Draw(_spriteBatch);
                if (go.GetComponent<Player>() == null)
                    go.Transform.Position = new Vector2( go.Transform.Position.X- _camera.Position.X , go.Transform.Position.Y - _camera.Position.Y);

            }

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
                gameObjects.Add(newGameObjects[i]);
                newGameObjects[i].Awake();
                newGameObjects[i].Start();

            }

            for (int i = 0; i < destroyedGameObjects.Count; i++)
            {
                gameObjects.Remove(destroyedGameObjects[i]);
            }

            destroyedGameObjects.Clear();
            newGameObjects.Clear();

        }

        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in gameObjects)
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
