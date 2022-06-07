using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public class MapCreator
    {

        private readonly string[] objects = new string[]
        {
            "Tree",
            "Rock",
            "Field",
            "FieldWaterEdge",
            "Hill",
            "HillWaterEdge",
            "Snow",
            "SnowWaterEdge",
            "Water",
            "Desert"
        };
        public static bool DevMode = false;
        private int objectIndex = 0;
        private int areaIndex = 0;

        #region ButtonBools
        private bool mRightReleased;
        private bool mLeftReleased;
        private bool tileMode = false;
        private bool rightButtonReleased = false;
        private bool leftButtonReleased = false;
        private bool lAltReleased = true;
        private bool f1Released = false;
        private bool f2Released = false;
        #endregion

        private Texture2D[] sprites = new Texture2D[10];
        private Texture2D selectedSprite;
        private Camera camera;
        public MapManager mapManager;

        private Area currentArea;
        private GameObjectType currentObject;
        private Cell cell;
        public Camera Camera { get => camera; }
        public Area CurrentArea { get => currentArea; }
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
            currentArea = Area.Camp;
            selectedSprite = sprites[(int)currentObject];
            camera = new Camera();
            mapManager = new MapManager();
        }




        public void Update(GameTime gameTime)
        {
            //Inputhandler normally works through the player, so to use it in DevMode it is updated here
            InputHandler.Instance.Execute(camera);
            InputHandler.Instance.Update(gameTime);

            UpdateObjectType();

            

            MouseState mouseState = Mouse.GetState();
            if (GameControl.Instance.playing.areaManager.currentGrid[(int)currentArea] != null)
                foreach (Cell c in GameControl.Instance.playing.areaManager.currentGrid[(int)currentArea])
                {

                    if (c.background.Intersects(new Rectangle(mouseState.X - (int)camera.Position.X, mouseState.Y - (int)camera.Position.Y, 10, 10))
                        && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased && !c.IsAreaChangeCell)
                    {
                        cell = c;
                        //Instantiate object
                        if (!tileMode && c.IsWalkable)
                        {
                            CreateObject(objects[(int)currentObject], cell);
                            c.IsWalkable = false;
                        }
                        else if (tileMode)
                        {//change tile

                            c.Sprite = selectedSprite;
                            c.IsNew = true;
                        }



                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        mLeftReleased = true;
                    }

                    if (c.background.Intersects(new Rectangle(mouseState.X - (int)camera.Position.X, mouseState.Y - (int)camera.Position.Y, 10, 10))
                    && mouseState.RightButton == ButtonState.Pressed && mRightReleased && !c.IsAreaChangeCell)
                    {

                        RemoveItem(mouseState, c);
                    }
                    if (mouseState.RightButton == ButtonState.Released)
                    {
                        mRightReleased = true;
                    }



                }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt) && lAltReleased)
            {
                mapManager.SaveComponents(GameControl.Instance.playing.areaManager.currentGameObjects[(int)currentArea], GameControl.Instance.playing.currentGrid, mapManager.CurrentSave, currentArea);
                foreach (GameObject go in GameControl.Instance.playing.currentGameObjects)
                {
                    go.IsNew = false;
                }
                lAltReleased = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.LeftAlt))
            {
                lAltReleased = true;
            }

            ChangeCurrentLists();

        }

        /// <summary>
        /// Changes the area index with F1 and F2 to go between areas
        /// </summary>
        private void ChangeCurrentLists()
        {


            if (Keyboard.GetState().IsKeyDown(Keys.F1) && f1Released && areaIndex != 3)
            {
                areaIndex++;
                GameControl.Instance.playing.areaManager.AreaChange(currentArea, (Area)areaIndex);
                f1Released = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.F1))
            {
                f1Released = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F2) && f2Released && areaIndex > 0)
            {
                areaIndex--;
                GameControl.Instance.playing.areaManager.AreaChange(currentArea, (Area)areaIndex);

                f2Released = false;
            }
            if (Keyboard.GetState().IsKeyUp(Keys.F2))
            {
                f2Released = true;
            }


        }



        /// <summary>
        /// If currentObject is a tile it will remove the sprite and set the cells IsNew to true, so that it is updated in the database upon saving.
        /// If currentObject is Component it will use the Collider component to remove the GameObject is attached to, and make the cell Walkable again.
        /// </summary>
        /// <param name="mouseState"></param>
        /// <param name="cell"></param>
        public void RemoveItem(MouseState mouseState, Cell cell)
        {
            if (!tileMode)
            {
                foreach (GameObject go in GameControl.Instance.playing.areaManager.currentGameObjects[(int)currentArea])
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
                cell.IsNew = true;
            }


        }


        /// <summary>
        /// Creates an object matching the current index in the objects array
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="cell"></param>
        public void CreateObject(string objectTag, Cell cell)
        {
            switch (objectTag)
            {
                case "Tree":
                    GameControl.Instance.playing.Instantiate((TreeFactory.Instance.CreateGameObject(
                        GameControl.Instance.playing.currentGrid.Find(x => x.Position == cell.Position), GameWorld.Instance.rand.Next(10, 50), true)));
                    break;
                case "Rock":
                    GameControl.Instance.playing.Instantiate((BoulderFactory.Instance.CreateGameObject(
                       GameControl.Instance.playing.currentGrid.Find(x => x.Position == cell.Position), GameWorld.Instance.rand.Next(10, 50), true)));
                    break;
                default:
                    break;
            }
        }

    
        /// <summary>
        /// Updates the objectIndex integer with Left and Right arrow keys
        /// </summary>
        public void UpdateObjectType()
        {
            selectedSprite = sprites[(int)currentObject];
            currentArea = (Area)areaIndex;

            if (Keyboard.GetState().IsKeyDown(Keys.Right) && objectIndex != sprites.Length - 1 && rightButtonReleased)
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

            currentObject = (GameObjectType)objectIndex;
            if (objectIndex < 2)
            {
                tileMode = false;
            }
            else
            {
                tileMode = true;
            }

        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(selectedSprite, new Rectangle((Mouse.GetState().X - 36) - (int)camera.Position.X, (Mouse.GetState().Y - 36) - (int)camera.Position.Y, 80, 80), Color.White * 0.8f);
        }

    }
}
