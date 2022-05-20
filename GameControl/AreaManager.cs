﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading;

namespace EksamensProjekt2022
{
    public class AreaManager
    {
        public List<GameObject>[] currentGameObjects = new List<GameObject>[4];
        public List<Cell>[] currentGrid = new List<Cell>[4];
        public Dictionary<Point, Cell>[] currentCells = new Dictionary<Point, Cell>[4];


        private CurrentArea currentArea;
        private Thread loadAreasThread;
        private bool HasRun = false;

        public event EventHandler AreaChange;

        public AreaManager()
        {
            //   Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();

            currentArea = 0;
            for (int i = 0; i < currentGameObjects.Length; i++)
            {
                currentGameObjects[i] = new List<GameObject>();

            }
            if (!HasRun)
            {
                CreateGrids();
                HasRun = true;
            }
           
         
        }

        public void CreateGrids()
        {
            
            loadAreasThread = new Thread(LoadArea);
            loadAreasThread.IsBackground = true;
            loadAreasThread.Start();
        }
        public void OnAreaChange()
        {
            if (GameControl.Instance.playing.newGameObjects.Count == 0)
            {
                currentGameObjects[(int)currentArea].Clear();
                currentGrid[(int)currentArea].Clear();

                foreach (Cell grid in GameControl.Instance.playing.currentGrid)
                {
                    currentGrid[(int)currentArea].Add(grid);
                }
                foreach (GameObject go in GameControl.Instance.playing.currentGameObjects)
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

        public void LoadArea()
        {
            CreateGrid();
            CreateCells();
            LoadGridAndCell();

        }

        public void CreateGrid()
        {
            for (int i = 1; i < currentGrid.Length; i++)
            {
                currentGrid[i] = GameControl.Instance.playing.grid.CreateGrid();
            }
        }

        public void CreateCells()
        {
            for (int i = 1; i < currentCells.Length; i++)
            {
                currentCells[i] = GameControl.Instance.playing.grid.CreateCells();
            }
        }

        public void LoadGridAndCell()
        {
            for (int i = 1; i < currentGrid.Length; i++)
            {
                foreach (Cell c in currentGrid[i])
                {
                    c.LoadContent();
                    if (currentGrid[i] == currentGrid[1])
                    {
                        c.Sprite = GameWorld.Instance.Content.Load<Texture2D>("AreaSprites/Field");
                    }
                }

                foreach (Cell item in currentCells[i].Values)
                {
                    item.LoadContent();
                }
            }

        }

    }
}
