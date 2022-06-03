using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Threading;

namespace EksamensProjekt2022
{
    public class GameWorld : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        public List<Collider> Colliders = new List<Collider>();
        public Texture2D pixel;
        public Random rand = new Random();
        public static float DeltaTime;
        public Thread createDBThread;

        public GraphicsDeviceManager Graphics { get => _graphics; }

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

        }

        protected override void Initialize()
        {

            Graphics.PreferredBackBufferWidth = 800;
            Graphics.PreferredBackBufferHeight = 600;
            Graphics.ApplyChanges();
            GameControl.Instance.currentGameState = CurrentGameState.StartMenu;

            createDBThread = new Thread(CreateDB);
            createDBThread.IsBackground = true;
            createDBThread.Start();


            base.Initialize();

        }
        private void CreateDB()
        {
            if (File.Exists("userinfo.db") == false)
            {
                string sqlConnectionString = "Data Source=userinfo.db;new=True;";
                var sqlConnection = new SQLiteConnection(sqlConnectionString);
                sqlConnection.Open();
                string cmd = File.ReadAllText("CreateUserInfoDB.sql");
                var createDB = new SQLiteCommand(cmd, sqlConnection);
                createDB.ExecuteNonQuery();
                sqlConnection.Close();

            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = Content.Load<Texture2D>("Pixel");

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed && !StartScreen.databaseIsLoading || Keyboard.GetState().IsKeyDown(Keys.Escape) && !StartScreen.databaseIsLoading)
                Exit();
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keystate = Keyboard.GetState();

            if (MapCreator.DevMode && GameControl.Instance.currentGameState == CurrentGameState.Playing)
            {
                MapCreator.Instance.Update(gameTime);
            }


            //Eksempel på hvordan man skifter spillerens placering
            //Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            //if (player != null && player.currentCell.Position == currentCells[new Point(1, 1)].Position)
            //{
            //    player.MyArea = CurrentArea.River;
            //    areaManager.ChangeArea(player.MyArea);
            //    currentGameObjects.Remove(player.GameObject);
            //    currentCells = areaManager.currentCells[(int)player.MyArea];
            //    currentGrid = areaManager.currentGrid[(int)player.MyArea];
            //    currentGameObjects = areaManager.currentGameObjects[(int)player.MyArea];
            //    currentGameObjects.Add(player.GameObject);

            //    foreach (GameObject go in currentGameObjects)
            //    {
            //        if (go.GetComponent<Player>() == null)
            //        {
            //            go.Start();
            //        }
            //    }


            //   }

            GameControl.Instance.UpdateGameState(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (MapCreator.DevMode)
            {
                var screenScale = MapCreator.Instance.Camera.GetScreenScale();
                var viewMatrix = MapCreator.Instance.Camera.GetTransform();
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
               null, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));

            }
            else
            {
                var screenScale = GameControl.Instance.camera.GetScreenScale();
                var viewMatrix = GameControl.Instance.camera.GetTransform();
                _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
               null, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));
            }





            if (GameControl.Instance.playing.currentGrid != null)
                foreach (Cell item in GameControl.Instance.playing.currentGrid)
                {
                    item.Draw(_spriteBatch);

                }

            for (int i = 0; i < GameControl.Instance.playing.currentGameObjects.Count; i++)
            {
                GameControl.Instance.playing.currentGameObjects[i].Draw(_spriteBatch);

            }

            if (GameControl.Instance.currentGameState == CurrentGameState.StartMenu)
                GameControl.Instance.startScreen.Draw(_spriteBatch);


            if (GameControl.Instance.currentGameState == CurrentGameState.Playing)
                GameControl.Instance.playing.Draw(_spriteBatch);


#if DEBUG
            GameControl.Instance.playing._debugTools.Draw(_spriteBatch);
#endif

            if (GameControl.Instance.playing.timeManager != null)
            {
                GameControl.Instance.playing.timeManager.Draw(_spriteBatch);
            }
            if (MapCreator.DevMode)
            {
                MapCreator.Instance.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }



        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in GameControl.Instance.playing.currentGameObjects)
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
