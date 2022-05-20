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
