using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Threading;

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
        public void GetDbInfo(SaveSlots currentSave, Area area)
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
        public void IdentifyComponent(SaveSlots currentSave, Area area)
        {
            var cmd = new SQLiteCommand($"SELECT ObjectTag, PositionX, PositionY, Quantity FROM areadata WHERE SaveSlotID={(int)currentSave} AND AreaIndex={(int)area}", connection);
            var dataread = cmd.ExecuteReader();

            while (dataread.Read())
            {
                componentName = dataread.GetString(0);
                position = new Point(dataread.GetInt32(1), dataread.GetInt32(2));
                amount = dataread.GetInt32(3);
                CreateComponent(componentName, area);
            }

            cmd = new SQLiteCommand($"SELECT AreaIndex, TileType, PositionX, PositionY FROM areacells WHERE SaveSlotID={(int)currentSave} AND AreaIndex={(int)area}", connection);
            dataread = cmd.ExecuteReader();
            while (dataread.Read())
            {
                int areaIndex = dataread.GetInt32(0);
                string texture = dataread.GetString(1);

                if (texture != null && areaIndex == (int)area)
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

        public void LoadPlayer(SaveSlots currentSave)
        {
            Open();
            var cmd = new SQLiteCommand($"SELECT Health, Energy, Hunger, PositionX, PositionY, Time FROM SaveSlots WHERE SaveSlotID={(int)currentSave}", connection);
            var dataread = cmd.ExecuteReader();

            while (dataread.Read())
            {
                int health = dataread.GetInt32(0);
                int energy = dataread.GetInt32(1);
                int hunger = dataread.GetInt32(2);
                int x = dataread.GetInt32(3);
                int y = dataread.GetInt32(4);
                int time = dataread.GetInt32(5);

                Director d = new Director(new PlayerBuilder());
                GameControl.Instance.playing.Instantiate(d.Construct(health, energy, hunger, x, y, time));
            }
            Close();
        }

        /// <summary>
        /// Instantiates the components, meaning ressource deposits, depending on their Tag and area index
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="area"></param>
        public void CreateComponent(string componentName, Area area)
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
        /// Clears the database table with the correlating SaveSlot and Area index before saving the GameObjects and Cells.
        /// </summary>
        /// <param name="currentSave"></param>
        public void SaveComponents(List<GameObject> gameObjects, List<Cell> grid, SaveSlots currentSave, Area area)
        {
            Open();

            //clear saved areadata
            var cmd = new SQLiteCommand($"DELETE FROM areadata WHERE SaveSlotID={(int)currentSave} AND AreaIndex={(int)area}", connection);
            cmd.ExecuteNonQuery();

            //save new areadata
            foreach (GameObject go in gameObjects)
            {
                if (go.GetComponent<Player>() != null)
                {
                    Player p = go.GetComponent<Player>() as Player;
                    SurvivalAspect sa = p.GameObject.GetComponent<SurvivalAspect>() as SurvivalAspect;

                    cmd = new SQLiteCommand($"UPDATE saveslots SET Health ={sa.CurrentHealth}, Energy = {sa.CurrentEnergy}, Hunger = {sa.CurrentHunger}," +
                        $" PositionX = {p.currentCell.Position.X}, PositionY = {p.currentCell.Position.Y}, Time = {(int)GameControl.Instance.playing.timeManager.Time} " +
                        $"WHERE SaveSlotID={(int)currentSave} ", connection);
                    cmd.ExecuteNonQuery();
                   
                }
                else
                {
                    var Cell = grid.Find(x => x.cellVector == go.Transform.Position);

                    cmd = new SQLiteCommand($"INSERT INTO areadata (ID, SaveSlotID, AreaIndex, ObjectTag, PositionX, PositionY, Quantity) " +
                       $"VALUES (null, {(int)currentSave}, {(int)area}, '{go.Tag}', {Cell.Position.X}, {Cell.Position.Y}, {go.Amount})", connection);
                    cmd.ExecuteNonQuery();
                }
             



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
                    cmd = new SQLiteCommand($"DELETE FROM areacells WHERE(PositionX={c.Position.X} AND PositionY={c.Position.Y} AND AreaIndex={(int)area})", connection);
                    cmd.ExecuteNonQuery();
                }

            }

            Close();

        }

        /// <summary>
        /// Load the inventory of the correlating SaveSlotID
        /// </summary>
        /// <param name="currentSave"></param>
        public void LoadPlayerInventory(SaveSlots currentSave)
        {
            Open();
            var cmd = new SQLiteCommand($"SELECT Name, Quantity FROM item WHERE(InventoryID={(int)currentSave})", connection);
            var dataread = cmd.ExecuteReader();
            Player p = GameWorld.Instance.FindObjectOfType<Player>() as Player;
            if (p != null)
            {
                Inventory inv = p.GameObject.GetComponent<Inventory>() as Inventory;

                while (dataread.Read())
                {
                    string name = dataread.GetString(0);
                    int quantity = dataread.GetInt32(1);

                    switch (name)
                    {
                        case "stone":
                            inv.AddItem(new Stone(quantity));
                            break;
                        case "wood":
                            inv.AddItem(new Wood(quantity));
                            break;

                    }
                }
            }

            Close();
        }

        /// <summary>
        /// Saves the players inventory to the correlating SaveSlotID
        /// </summary>
        /// <param name="items"></param>
        public void SavePlayerInventory(List<Item> items)
        {

            Open();
            var cmd = new SQLiteCommand($"DELETE FROM item WHERE(InventoryID={(int)currentSave})", connection);
            var execute = cmd.ExecuteNonQuery();

            foreach (Item i in items)
            {
                var name = i.Name;
                var quantity = i.Quantity;

                cmd = new SQLiteCommand($"INSERT INTO item VALUES (null, {(int)currentSave}, '{name}', {quantity}) ", connection);
                execute = cmd.ExecuteNonQuery();
            }

            Close();
        }


        /// <summary>
        /// Opens connection to database. 
        /// Is just more convenient
        /// </summary>
        private void Open()
        {
            connection.Open();
        }

        /// <summary>
        /// Closes connection to database.
        /// Is just more convenient
        /// </summary>
        private void Close()
        {
            connection.Close();
        }

    }
}
