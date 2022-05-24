﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Data.SQLite;

namespace EksamensProjekt2022
{
    public class MapManager
    {
        public SQLiteConnection connection = new SQLiteConnection("Data Source=userinfo.db");
        private SaveSlots currentSave;
        private CurrentArea currentArea;

        public AreaManager areaLoader;
        public SaveSlots CurrentSave { get => currentSave; }


        private string componentName;
        private Point position;
        private int amount;



        public MapManager()
        {
            areaLoader = new AreaManager();
        }

        /// <summary>
        /// Creates lists for each area that matches with the SaveSlot Id, the MapManagers' lists then has
        /// to override the GameControls' instance of AreaManager in order to use the save file data
        /// </summary>
        /// <param name="currentSave"></param>
        public void GetDbInfo(SaveSlots currentSave, CurrentArea area)
        {
            Open();
            this.currentSave = currentSave;
            currentArea = area;

            for (int i = 0; i < areaLoader.currentGrid.Length; i++)
            {
                GetComponents(currentSave, area);
            }
            Close();
        }

        /// <summary>
        /// Gets component information from the database and adds them to the correct lists
        /// </summary>
        /// <param name="currentSave"></param>
        /// <param name="area"></param>
        public void GetComponents(SaveSlots currentSave, CurrentArea area)
        {



            var cmd = new SQLiteCommand($"SELECT GameObject, PositionX, PositionY, Amount FROM area WHERE UserID={(int)currentSave}", connection);
            var dataread = cmd.ExecuteReader();

            while (dataread.Read())
            {

                componentName = dataread.GetString(0);
                position = new Point(dataread.GetInt32(1), dataread.GetInt32(2));
                amount = dataread.GetInt32(3);

                CreateComponent(componentName, area);

            }

             cmd = new SQLiteCommand($"SELECT CurrentArea, Texture, PositionX, PositionY FROM areacells WHERE UserID={(int)currentSave}", connection);
            dataread = cmd.ExecuteReader();
            while (dataread.Read())
            {
                string texture = dataread.GetString(1);

                if ( texture!= null)
                {
                    position = new Point(dataread.GetInt32(2), dataread.GetInt32(3));

                    foreach (Cell c in GameControl.Instance.playing.areaManager.currentGrid[(int)area])
                    {
                        if (c.Position == position)
                        {
                            c.Sprite = GameWorld.Instance.Content.Load<Texture2D>(texture);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Instantiates the components, meaning ressource deposits, depending on their Tag and area index
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="area"></param>
        public void CreateComponent(string componentName, CurrentArea area)
        {

            switch (componentName)
            {
                case "Tree":
                    areaLoader.currentGameObjects[(int)area].Add(TreeFactory.Instance.CreateGameObject(GameControl.Instance.playing.areaManager.currentGrid[(int)area].Find(x => x.Position == position), amount));
                    break;
                case "Boulder":
                    areaLoader.currentGameObjects[(int)area].Add((BoulderFactory.Instance.CreateGameObject(GameControl.Instance.playing.areaManager.currentGrid[(int)area].Find(x => x.Position == position), amount)));
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Save objects on all area lists to the save slot
        /// </summary>
        /// <param name="currentSave"></param>
        public void SaveComponents(List<GameObject> gameObjects, List<Cell> grid, SaveSlots currentSave, CurrentArea area)
        {
            Open();

            foreach (GameObject go in gameObjects)
            {
                //Find the correct position on grid
                var Cell = grid.Find(x => x.cellVector == go.Transform.Position);

                var cmd = new SQLiteCommand($"INSERT INTO area (ID, UserID, CurrentArea, GameObject, PositionX, PositionY, Amount) " +
                    $"VALUES (null, {(int)currentSave}, {(int)area}, '{go.Tag}', {Cell.Position.X}, {Cell.Position.Y}, {go.Amount})", connection);
                cmd.ExecuteNonQuery();
            }

            foreach (Cell c in grid)
            {
                if (c.Sprite != null)
                {
                    var cmd = new SQLiteCommand($"INSERT INTO areacells (ID, UserID, CurrentArea, Texture, PositionX, PositionY) " +
                    $"VALUES (null, {(int)currentSave}, {(int)area}, '{c.Sprite.ToString()}', {c.Position.X}, {c.Position.Y})", connection);
                    cmd.ExecuteNonQuery();

                }
               
            }

            Close();
        }


        public void Open()
        {
            connection.Open();
        }

        public void Close()
        {
            connection.Close();
        }

    }
}
