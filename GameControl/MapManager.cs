using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Data.SQLite;

namespace EksamensProjekt2022
{
    public class MapManager
    {
        public SQLiteConnection connection = new SQLiteConnection("Data Source=userinfo.db");
        private SaveSlots currentSave;

        public AreaManager areaLoader;
        private List<GameObject> dbGameObjects;



        private string componentName;
        private Point position;
        private int amount;


        public MapManager()
        {
            areaLoader = new AreaManager();
            //dbGameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Creates lists for each area that matches with the SaveSlot Id, the MapManagers' lists then has
        /// to override the GameControls' instance of AreaManager in order to use the save file data
        /// </summary>
        /// <param name="currentSave"></param>
        public void GetDbInfo(SaveSlots currentSave)
        {
            Open();
            this.currentSave = currentSave;

            for (int i = 0; i < areaLoader.currentGrid.Length; i++)
            {
                GetComponents(currentSave, (int)i);
            }
            Close();
        }

        /// <summary>
        /// Gets component information from the database and adds them to the correct lists
        /// </summary>
        /// <param name="currentSave"></param>
        /// <param name="area"></param>
        public void GetComponents(SaveSlots currentSave, int area)
        {

            int positionX;
            int positionY;

            var cmd = new SQLiteCommand($"SELECT * FROM area WHERE UserID={(int)currentSave}", connection);
            var dataread = cmd.ExecuteReader();

            while (dataread.Read())
            {
                //area = dataread.GetInt32(3);
                componentName = dataread.GetString(4);
                positionX = dataread.GetInt32(5);
                positionY = dataread.GetInt32(6);
                position = new Point(positionX, positionY);
                amount = dataread.GetInt32(7);

                CreateComponent(componentName, area);

            }

        }

        /// <summary>
        /// Instantiates the components, meaning ressource deposits, depending on their Tag and area index
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="area"></param>
        public void CreateComponent(string componentName, int area)
        {

            switch (componentName)
            {
                case "Tree":
                    areaLoader.currentGameObjects[area].Add((TreeFactory.Instance.CreateGameObject(areaLoader.currentGrid[area].Find(x => x.Position == position), amount)));
                    break;
                case "Boulder":
                    areaLoader.currentGameObjects[area].Add((BoulderFactory.Instance.CreateGameObject(areaLoader.currentGrid[area].Find(x => x.Position == position), amount)));
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// Save objects on all area lists to the save slot
        /// </summary>
        /// <param name="currentSave"></param>
        public void SaveComponents(SaveSlots currentSave)
        {
            Open();
            for (int i = 0; i < areaLoader.currentGameObjects.Length; i++)
            {
                foreach (GameObject go in areaLoader.currentGameObjects[i])
                {
                    //Find the correct position on grid
                    var Cell = areaLoader.currentGrid[0].Find(x => x.cellVector == go.Transform.Position);

                    var cmd = new SQLiteCommand($"INSERT INTO area (ID, UserID, GameObject, PositionX, PositionY, Amount) " +
                        $"VALUES (null, {currentSave}, {go.Tag}, {Cell.cellVector.X}, {Cell.cellVector.Y}, {go.Amount})");
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
