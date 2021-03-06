using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    public class InputHandler
    {
        private MouseState mouseState;
        private static InputHandler instance;
        private Astar pathfinder;
        private Cell start, goal;
        public List<Node> finalPath;
        public bool mLeftReleased = false;
        public Rectangle uiBox;
        public Rectangle playerInventoryBox;
        public Rectangle craftingBox;

        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputHandler();
                }
                return instance;
            }
        }

        private Dictionary<Keys, ICommand> keybinds = new Dictionary<Keys, ICommand>();

        public InputHandler()
        {

            pathfinder = new Astar();

            //Camera controls
            keybinds.Add(Keys.W, new MoveCommand(new Vector2(0, 8)));
            keybinds.Add(Keys.A, new MoveCommand(new Vector2(8, 0)));
            keybinds.Add(Keys.D, new MoveCommand(new Vector2(-8, 0)));
            keybinds.Add(Keys.S, new MoveCommand(new Vector2(0, -8)));
        }

        /// <summary>
        /// Used to check if you can target the cell and if you can, start the search algorithm
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (!MapCreator.DevMode)
            {
                
                Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
                foreach (Cell c in GameControl.Instance.playing.currentGrid)
                {

                    if (c.background.Intersects(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 10, 10)) && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased &&
                        c.IsWalkable && c.cellVector != player.currentCell.cellVector
                        && !uiBox.Contains(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 5, 5))
                        && !craftingBox.Contains(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 5, 5))
                        && !playerInventoryBox.Contains(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 5, 5)))
                    {
                        player.readyToMove = false;
                        player.step = 0;
                        mLeftReleased = false;
                        start = GameControl.Instance.playing.currentCells[player.currentCell.Position];
                        goal = c;

                        FindPath();
                        finalPath.Reverse();
                        GameControl.Instance.playing.selectedCell = null;
                        uiBox = Rectangle.Empty;
                        craftingBox = Rectangle.Empty;
                    }
                    else if (c.background.Intersects(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 10, 10)) && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased &&
                        !c.IsWalkable && c.cellVector != player.currentCell.cellVector
                        && !uiBox.Contains(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 5, 5))
                        && !craftingBox.Contains(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 5, 5))
                         && !playerInventoryBox.Contains(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 5, 5)))
                    {
                        GameControl.Instance.playing.selectedCell = c;
                        player.selectedCell = c;
                    }
                    if (mouseState.LeftButton == ButtonState.Released)
                    {
                        mLeftReleased = true;
                    }
                }

            }
            if (MapCreator.DevMode)
            {
                Camera cam = MapCreator.Instance.Camera;
                Execute(cam);
            }

        }

        /// <summary>
        /// Returns the result of the A* search algorithm and clears the list if it already exists.
        /// </summary>
        public void FindPath()
        {
            if (finalPath != null)
            {
                finalPath.Clear();
            }
            finalPath = pathfinder.FindPath(start.Position, goal.Position, GameControl.Instance.playing.grid.CreateNodes());
#if DEBUG
            ColorNodes();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player"></param>
        public void Execute(Player player)
        {
            mouseState = Mouse.GetState();
            Camera c = GameControl.Instance.camera;
            Execute(c);
        }

        public void Execute(Camera camera)
        {
            KeyboardState keyState = Keyboard.GetState();
            foreach (Keys key in keybinds.Keys)
            {
                if (keyState.IsKeyDown(key))
                {
                    keybinds[key].Execute(camera);
                }
            }

        }
        /// <summary>
        /// Colors nodes to display the checked tiles, is mainly a debug tool
        /// </summary>
        public void ColorNodes()
        {
            foreach (Cell item in GameControl.Instance.playing.currentGrid)
            {
                item.BackGroundColor = Color.White * 0.1f;
                if (pathfinder.Open.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.Blue * 0.5f;
                }
                if (pathfinder.Closed.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.Orange * 0.5f;
                }
                if (finalPath.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.GreenYellow * 0.5f;
                }
            }
        }
    }

}
