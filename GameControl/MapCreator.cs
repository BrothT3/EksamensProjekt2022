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
        private bool mLeftReleased;
        private string[] spriteNames = new string[]
        { 
            "Tree",
            "Rock",
            "Field",
         

        };

        private Texture2D[] sprites = new Texture2D[3];
        private Texture2D selectedSprite;
        private Camera camera;
        private MapManager mapManager;
        private CurrentArea currentArea;
        private GameObjectType currentObject;
        public List<GameObject> addedGameObjects = new List<GameObject>();

        public MapCreator()
        {
            for (int i = 0; i < spriteNames.Length; i++)
            {
                sprites[i] = GameWorld.Instance.Content.Load<Texture2D>($"AreaSprites/{spriteNames[i]}");
            }
            currentObject = GameObjectType.Tree;
            currentArea = CurrentArea.Hills;
            selectedSprite = sprites[(int)currentObject];
            camera = GameControl.Instance.camera;
            mapManager = new MapManager();
        }


        public void Update(GameTime gameTime)
        {
            
            MouseState mouseState = Mouse.GetState();
            foreach (Cell c in mapManager.areaLoader.currentGrid[(int)currentArea])
            {

                if (c.background.Intersects(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 10, 10))
                    && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased )
                {
                    InsertItem(mouseState);
                }
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    mLeftReleased = true;
                }
            }
            
        }

        public void InsertItem(MouseState mouseState)
        {
           Cell cell = mapManager.areaLoader.currentGrid[(int)currentArea].Find(x => x.cellVector == new Point(mouseState.X, mouseState.Y).ToVector2());

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
                        mapManager.areaLoader.currentGrid[(int)currentArea].Find(x=> x.Position == cell.Position), 500)));
                    break;
                case "Boulder":
                    GameControl.Instance.playing.Instantiate((BoulderFactory.Instance.CreateGameObject(
                       mapManager.areaLoader.currentGrid[(int)currentArea].Find(x => x.Position == cell.Position), 500)));
                    break;
                default:
                    break;
            }
        }


    }
}
