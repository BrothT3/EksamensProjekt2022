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
            _grid = new Grid(20, 35, 35);
            grid = _grid.CreateGrid();
            Cells = _grid.CreateCells();

        }

        protected override void Initialize()
        {

            GameObject player = new GameObject();
            player.AddComponent(new Player());
            player.AddComponent(new SpriteRenderer());
            gameObjects.Add(player);

            GameObject fishingSpot = new GameObject();
            fishingSpot.AddComponent(new FishingSpot(new Point(3, 3), 10));
            gameObjects.Add(fishingSpot);

            GameObject crop = new GameObject();
            crop.AddComponent(new Crop(new Point(5, 5), 10));
            gameObjects.Add(crop);


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

            foreach (Cell item in grid)
            {
                item.Update(gameTime);
            }
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(gameTime);
            }
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (grid != null)
                foreach (Cell item in grid)
                {
                    item.Draw(_spriteBatch);
                }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(_spriteBatch);
            }




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
