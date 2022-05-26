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
        Boulder,
        Field,
        FieldWaterEdge,
        Mountain
    }
    public class MapCreator
    {

        private string[] objects = new string[]
        {
            "Tree",
            "Rock",
            "Field",
            "FieldWaterEdge",
            "Mountain"
        };
        public static bool DevMode = false;
        private int objectIndex = 0;
        private bool mRightReleased;
        private bool mLeftReleased;
       
        private bool tileMode = false;
        private bool rightButtonReleased = false;
        private bool leftButtonReleased = false;

        private Texture2D[] sprites = new Texture2D[5];
        private Texture2D selectedSprite;
        private Camera camera;
        public MapManager mapManager;

        private CurrentArea currentArea;
        private GameObjectType currentObject;
        private Cell cell;
        public Camera Camera { get => camera; }
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
            for (int i = 0; i < objects.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>($"AreaSprites/{objects[i]}");
            }
            currentObject = GameObjectType.Tree;
            currentArea = CurrentArea.Camp;
            selectedSprite = sprites[(int)currentObject];
            camera = new Camera();
            mapManager = new MapManager();
        }


        bool enterReleased = true;
        public void Update(GameTime gameTime)
        {
            InputHandler.Instance.Execute(camera);
            InputHandler.Instance.Update(gameTime);
            UpdateObjectType();
            ChangeObjectType();
            selectedSprite = sprites[(int)currentObject];

            MouseState mouseState = Mouse.GetState();
            if (GameControl.Instance.playing.currentGrid != null)
                foreach (Cell c in GameControl.Instance.playing.currentGrid)
                {

                    if (c.background.Intersects(new Rectangle(mouseState.X - (int)camera.Position.X, mouseState.Y - (int)camera.Position.Y, 10, 10))
                        && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased && c.IsWalkable)
                    {
                        cell = c;
                        InsertItem();
                        c.IsWalkable = false;
                        
                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        mLeftReleased = true;
                    }

                    if (c.background.Intersects(new Rectangle(mouseState.X - (int)camera.Position.X, mouseState.Y - (int)camera.Position.Y, 10, 10))
                    && mouseState.RightButton == ButtonState.Pressed && mRightReleased)
                    {

                        RemoveItem(mouseState, c);
                    }
                    if (mouseState.RightButton == ButtonState.Released)
                    {
                        mRightReleased = true;
                    }

                }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && enterReleased)
            {
                mapManager.SaveComponents(GameControl.Instance.playing.currentGameObjects, GameControl.Instance.playing.currentGrid, mapManager.CurrentSave, currentArea);
                enterReleased = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
            {
                enterReleased = true;
            }


        }

        /// <summary>
        /// Deletes the object on tile and resets the walkable bool, or resets the tile sprite
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="cell"></param>
        public void RemoveItem(MouseState mouseState, Cell cell)
        {
            if (!tileMode)
            {
                foreach (GameObject go in GameControl.Instance.playing.currentGameObjects)
                {
                    if (go.GetComponent<Collider>() != null)
                    {
                        Collider col = go.GetComponent<Collider>() as Collider;
                        if (col.CollisionBox.Intersects(new Rectangle(mouseState.X - (int)camera.Position.X, mouseState.Y - (int)camera.Position.Y, 10, 10))
                         && mouseState.RightButton == ButtonState.Pressed && mRightReleased)
                        {
                            cell.IsWalkable = true;
                            GameControl.Instance.playing.Destroy(go);

                        }

                    }


                }
            }
            else
            {
                cell.Sprite = null;
            }
            
        
        }

        /// <summary>
        /// Manages which object is inserted to the grid depending on currentObject index
        /// </summary>
        /// <param name="mouseState"></param>
        public void InsertItem()
        {

            switch (objects[(int)currentObject])
            {
                case "Tree":
                    CreateObject(objects[0], cell);
                    break;

                case "Rock":
                    CreateObject(objects[1], cell);
                    break;
                case "Field":
                    ChangeAreaTile(objects[2], cell);
                    break;
                case "FieldWaterEdge":
                    ChangeAreaTile(objects[3], cell);
                    break;
                case "Mountain":
                    ChangeAreaTile(objects[4], cell);
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Uses a tag string and a Cell to instantiate objects
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="cell"></param>
        public void CreateObject(string tag, Cell cell)
        {
            switch (tag)
            {
                case "Tree":
                    GameControl.Instance.playing.Instantiate((TreeFactory.Instance.CreateGameObject(
                        GameControl.Instance.playing.currentGrid.Find(x => x.Position == cell.Position), 500)));
                    break;
                case "Rock":
                    GameControl.Instance.playing.Instantiate((BoulderFactory.Instance.CreateGameObject(
                       GameControl.Instance.playing.currentGrid.Find(x => x.Position == cell.Position), 500)));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Uses tag string and Cell to update Cell sprite
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="cell"></param>
        public void ChangeAreaTile(string tag, Cell cell)
        {
            switch (tag)
            {
                case "Field":
                    //TODO skriv celle sprites ind i databasen når det gemmes
                    cell.Sprite = selectedSprite;
                    break;
                case "FieldWaterEdge":
                    cell.Sprite = selectedSprite;
                    break;
                case "Mountain":
                    cell.Sprite = selectedSprite;
                    cell.IsWalkable = false;
                    break;
                default:
                    break;
            }
        }
      
        /// <summary>
        /// Updates the objectIndex integer
        /// </summary>
        public void UpdateObjectType()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && objectIndex != sprites.Length && rightButtonReleased)
            {
                objectIndex++;
                rightButtonReleased = false;

            }
            if (Keyboard.GetState().IsKeyUp(Keys.Right))
            {

                rightButtonReleased = true;

            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && objectIndex != 0 && leftButtonReleased)
            {
                objectIndex--;
                leftButtonReleased = false;
                              

            }
            if (Keyboard.GetState().IsKeyUp(Keys.Left))
            {

                leftButtonReleased = true;

            }

            if (objectIndex > 5 || objectIndex < 0)
            {
                System.Diagnostics.Debugger.Break();
            }

        }

        /// <summary>
        /// Changes the currentObject reference to match the objectIndex
        /// </summary>
        private void ChangeObjectType()
        {

            switch (objectIndex)
            {
                case 0:
                    currentObject = GameObjectType.Tree;
                    tileMode = false;
                    break;
                case 1:
                    currentObject = GameObjectType.Boulder;
                    tileMode = false;
                    break;
                case 2:
                    currentObject = GameObjectType.Field;
                    tileMode = true;
                    break;
                case 3:
                    currentObject = GameObjectType.FieldWaterEdge;
                    tileMode = true;
                    break;
                case 4:
                    currentObject = GameObjectType.Mountain;
                    tileMode = true;
                    break;
                default:
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectedSprite, new Rectangle((Mouse.GetState().X - 36) - (int)camera.Position.X, (Mouse.GetState().Y - 36) - (int)camera.Position.Y, 80, 80), Color.White * 0.8f);
        }

    }
}
