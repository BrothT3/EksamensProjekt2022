using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Data.SQLite;

namespace EksamensProjekt2022
{
    public class MapManager
    {
        public SQLiteConnection connection = new SQLiteConnection("Data Source=userinfo.db");
        private SaveSlots currentSave;
     
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

            IdentifyComponent(currentSave, area);
            Close();
        }

        /// <summary>
        /// Gets component information from the database and adds them to the correct lists
        /// </summary>
        /// <param name="currentSave"></param>
        /// <param name="area"></param>
        public void IdentifyComponent(SaveSlots currentSave, CurrentArea area)
        {
            var cmd = new SQLiteCommand($"SELECT AreaIndex, TileType, PositionX, PositionY FROM areacells WHERE SaveSlotID={(int)currentSave} AND AreaIndex={(int)area}", connection);
            var dataread = cmd.ExecuteReader();
            while (dataread.Read())
            {
                int areaIndex = dataread.GetInt32(0);
                string texture = dataread.GetString(1);

                if (texture != null)
                {
                    position = new Point(dataread.GetInt32(2), dataread.GetInt32(3));


                    foreach (Cell c in GameControl.Instance.playing.areaManager.currentGrid[areaIndex])
                    {
                        if (c.Position == position)
                        {
                            c.Sprite = GameWorld.Instance.Content.Load<Texture2D>(texture);

                        }
                    }
                }
            }
            cmd = new SQLiteCommand($"SELECT ObjectTag, PositionX, PositionY, Quantity FROM areadata WHERE SaveSlotID={(int)currentSave} AND AreaIndex={(int)area}", connection);
            dataread = cmd.ExecuteReader();

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
                    GameControl.Instance.playing.areaManager.currentGameObjects[(int)area].Add(
                        TreeFactory.Instance.CreateGameObject
                        (GameControl.Instance.playing.areaManager.currentGrid[(int)area].Find(x => x.Position == position), amount, false));
                    break;
                case "Boulder":
                    GameControl.Instance.playing.areaManager.currentGameObjects[(int)area].Add(
                        (BoulderFactory.Instance.CreateGameObject
                        (GameControl.Instance.playing.areaManager.currentGrid[(int)area].Find(x => x.Position == position), amount, false)));
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

            //clear saved areadata
            var cmd = new SQLiteCommand($"DELETE FROM areadata WHERE SaveSlotID={(int)currentSave} AND AreaIndex={(int)area}", connection);
            cmd.ExecuteNonQuery();

            //save new areadata
            foreach (GameObject go in gameObjects)
            {

                var Cell = grid.Find(x => x.cellVector == go.Transform.Position);

                cmd = new SQLiteCommand($"INSERT INTO areadata (ID, SaveSlotID, AreaIndex, ObjectTag, PositionX, PositionY, Quantity) " +
                   $"VALUES (null, {(int)currentSave}, {(int)area}, '{go.Tag}', {Cell.Position.X}, {Cell.Position.Y}, {go.Amount})", connection);
                cmd.ExecuteNonQuery();



            }


            foreach (Cell c in grid)
            {
                if (c.Sprite != null && c.IsNew)
                {
                    cmd = new SQLiteCommand($"INSERT INTO areacells (ID, SaveSlotID, AreaIndex, TileType, PositionX, PositionY) " +
                   $"VALUES (null, {(int)currentSave}, {(int)area}, '{c.Sprite}', {c.Position.X}, {c.Position.Y})", connection);
                    cmd.ExecuteNonQuery();
                }
                else if (c.Sprite == null && c.IsNew)
                {
                    cmd = new SQLiteCommand($"DELETE FROM areacells WHERE(PositionX={c.Position.X} AND PositionY={c.Position.Y})", connection);
                    cmd.ExecuteNonQuery();
                }

            }

            Close();
        }


        private void Open()
        {
            connection.Open();
        }

        private void Close()
        {
            connection.Close();
        }

    }
}
