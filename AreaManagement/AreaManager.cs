using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    public enum CurrentArea
    {
        Camp,
        River,
        Hills,
        Desert
    }
    public class AreaManager
    {
        public List<GameObject>[] currentGameObjects = new List<GameObject>[4];
        public List<Cell>[] currentGrid = new List<Cell>[4];
        public Dictionary<Point, Cell>[] currentCells = new Dictionary<Point, Cell>[4];
        // public delegate void AreaChangeHandler(Player p);
        private CurrentArea currentArea;

        public event EventHandler AreaChange;

        public AreaManager()
        {
         //   Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            currentArea = 0;
            for (int i = 0; i < currentGameObjects.Length; i++)
            {
                currentGameObjects[i] = new List<GameObject>();
                
            }
           
         
        }

        public void OnAreaChange()
        {
            if (GameWorld.Instance.newGameObjects.Count == 0)
            {
                currentGameObjects[(int)currentArea].Clear();
                currentGrid[(int)currentArea].Clear();

                foreach (Cell grid in GameWorld.Instance.grid)
                {
                    currentGrid[(int)currentArea].Add(grid);
                }
                foreach (GameObject go in GameWorld.Instance.gameObjects)
                {
                    currentGameObjects[(int)currentArea].Add(go);

                }

            }
        }

        public void ChangeArea(CurrentArea area)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();

            currentArea = area;
            
                     
        }
      
    }
}
