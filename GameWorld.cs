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
        public List<Collider> Colliders = new List<Collider>();


        public static float DeltaTime;

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
            GameControl.Instance.currentGameState = GameState.MainMenu;

            base.Initialize();

        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            KeyboardState keystate = Keyboard.GetState();


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

            var screenScale = GameControl.Instance.camera.GetScreenScale();
            var viewMatrix = GameControl.Instance.camera.GetTransform();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied,
                null, null, null, null, viewMatrix * Matrix.CreateScale(screenScale));

            if (GameControl.Instance.currentGrid != null)
                foreach (Cell item in GameControl.Instance.currentGrid)
                {
                    item.Draw(_spriteBatch);

                }

            for (int i = 0; i < GameControl.Instance.currentGameObjects.Count; i++)
            {
                GameControl.Instance.currentGameObjects[i].Draw(_spriteBatch);

            }

            if (GameControl.Instance.currentGameState == GameState.MainMenu)
            {
                GameControl.Instance.mainmenu.Draw(_spriteBatch);
                foreach (Button item in GameControl.Instance.mainmenu.MainMenuButtons)
                {
                    item.Draw(_spriteBatch);
                }
                if (GameControl.Instance.mainmenu.wantToExit)
                {
                    foreach (Button item in GameControl.Instance.mainmenu.ExitButtons)
                    {
                        item.Draw(_spriteBatch);
                    }
                }


            }



            if (GameControl.Instance.timeManager != null)
            {
                GameControl.Instance.timeManager.draw(_spriteBatch);
            }

            GameControl.Instance._debugTools.Draw(_spriteBatch);


            _spriteBatch.End();

            base.Draw(gameTime);
        }



        public Component FindObjectOfType<T>() where T : Component
        {
            foreach (GameObject gameObject in GameControl.Instance.currentGameObjects)
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
