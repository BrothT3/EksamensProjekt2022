using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public enum GameObjectType
    {
        Tree,
        Rock,
        Field
    }
    public class MapCreator
    {
       
        private string[] spriteNames = new string[]
        {
            "Tree",
            "Rock",
            "Field",
        };
        private bool mRightReleased;
        private bool mLeftReleased;
        public static bool DevMode = false;
        private Texture2D[] sprites = new Texture2D[3];
        private Texture2D selectedSprite;
        public Camera camera;
        private MapManager mapManager;
        private CurrentArea currentArea;
        private GameObjectType currentObject;
        public List<GameObject> addedGameObjects = new List<GameObject>();
        private Cell cell;

        private static MapCreator instance;
        public static MapCreator Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MapCreator();
                }
                return instance;
            }
        }

        public MapCreator()
        {
            for (int i = 0; i < spriteNames.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>($"AreaSprites/{spriteNames[i]}");
            }
            currentObject = GameObjectType.Tree;
            currentArea = CurrentArea.Camp;
            selectedSprite = sprites[(int)currentObject];
            camera = new Camera();
            mapManager = new MapManager();
        }



        public void Update(GameTime gameTime)
        {
            InputHandler.Instance.Execute(camera);
            InputHandler.Instance.Update(gameTime);
          


            MouseState mouseState = Mouse.GetState();
            if (GameControl.Instance.playing.currentGrid != null)
                foreach (Cell c in GameControl.Instance.playing.currentGrid)
                {

                    if (c.background.Intersects(new Rectangle(mouseState.X - (int)camera.Position.X, mouseState.Y - (int)camera.Position.Y, 10, 10))
                        && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased && c.IsWalkable)
                    {
                        cell = c;
                        InsertItem(mouseState);
                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        mLeftReleased = true;
                    }

                  
                }
            foreach (GameObject go in GameControl.Instance.playing.currentGameObjects)
            {
                if(go.GetComponent<Collider>() != null)
                {
                    Collider c = go.GetComponent<Collider>() as Collider;
                    if (c.CollisionBox.Intersects(new Rectangle(mouseState.X - (int)camera.Position.X, mouseState.Y - (int)camera.Position.Y, 10, 10))
                     && mouseState.RightButton == ButtonState.Pressed && mRightReleased)
                    {
                        GameControl.Instance.playing.Destroy(go);
                    }
                    if (mouseState.RightButton == ButtonState.Released)
                    {
                        mRightReleased = true;
                    }
                }
               
            }

        }

        public void RemoveItem(GameObject go)
        {
            switch (go.Tag)
            {
                default:
                    break;
            }
        }

        public void InsertItem(MouseState mouseState)
        {
         
            switch (currentObject)
            {
                case (GameObjectType)0:
                    CreateObject(spriteNames[0], cell);
                    break;

                case (GameObjectType)1:
                    CreateObject(spriteNames[1], cell);
                    break;
                case (GameObjectType)2:
                    break;
                default:
                    break;
            }

        }

        public void CreateObject(string tag, Cell cell)
        {
            switch (tag)
            {
                case "Tree":
                    GameControl.Instance.playing.Instantiate((TreeFactory.Instance.CreateGameObject(
                        GameControl.Instance.playing.currentGrid.Find(x => x.Position == cell.Position), 500)));
                    break;
                case "Boulder":
                    GameControl.Instance.playing.Instantiate((BoulderFactory.Instance.CreateGameObject(
                       GameControl.Instance.playing.currentGrid.Find(x => x.Position == cell.Position), 500)));
                    break;
                default:
                    break;
            }
        }

        public void ChangeObjectType()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right) && (int)currentObject >= sprites.Length)
            {
                
            }
        }


    }
}
