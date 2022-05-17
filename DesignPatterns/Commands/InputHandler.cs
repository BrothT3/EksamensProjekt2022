using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    public class InputHandler
    {
        private MouseState mouseState;
        private Point target;
        private static InputHandler instance;
        private Astar pathfinder;
        private Cell start, goal;
        public List<Node> finalPath;
        private bool mLeftReleased = false;


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
        //kan bruges til at styre interface måske?
        private Dictionary<Keys, ICommand> keybinds = new Dictionary<Keys, ICommand>();
        private Dictionary<ButtonState, ICommand> movement = new Dictionary<ButtonState, ICommand>();

        public InputHandler()
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            //instans af Astar, start og slut punkt (som så bliver spillerens position og hvor end man vil hen
            pathfinder = new Astar();

            keybinds.Add(Keys.W, new MoveCommand(new Vector2(0, 8)));
            keybinds.Add(Keys.A, new MoveCommand(new Vector2(8, 0)));
            keybinds.Add(Keys.D, new MoveCommand(new Vector2(-8, 0)));
            keybinds.Add(Keys.S, new MoveCommand(new Vector2(0, -8)));
            //kører algoritmen, den flyttes også ind i input/movement senere men bare for at det virker

        }

        public void Update(GameTime gameTime)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            foreach (Cell c in GameControl.Instance.currentGrid)
            {

                if (c.background.Intersects(new Rectangle(mouseState.X - (int)GameControl.Instance.camera.Position.X, mouseState.Y - (int)GameControl.Instance.camera.Position.Y, 10, 10)) && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased &&
                    c.IsWalkable && c.cellVector != player.currentCell.cellVector)
                {
                    player.readyToMove = false;
                    player.step = 0;
                    mLeftReleased = false;
                    start = GameControl.Instance.currentCells[player.currentCell.Position];
                    goal = c;

                    FindPath();
                    finalPath.Reverse();
                }
                if (mouseState.LeftButton == ButtonState.Released)
                {
                    mLeftReleased = true;
                }
            }

         

        }

        public void FindPath()
        {
            if (finalPath != null)
            {
                finalPath.Clear();
            }
            finalPath = pathfinder.FindPath(start.Position, goal.Position, GameControl.Instance.grid.CreateNodes());
            ColorNodes();
        }
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
        /// Colors nodes to display the checked tiles
        /// </summary>
        public void ColorNodes()
        {
            foreach (Cell item in GameControl.Instance.currentGrid)
            {
                item.BackGroundColor = Color.White * 0.1f;
                if (pathfinder.Open.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.Blue *0.5f;
                }
                if (pathfinder.Closed.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.Orange *0.5f;
                }
                if (finalPath.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.GreenYellow *0.5f;
                }
            }
        }
    }


    /// <summary>
    /// KeyInfo can be used for buttonevent controls
    /// </summary>
    public class KeyInfo
    {
        public bool IsDown { get; set; }
        public Keys Key { get; set; }

        public KeyInfo(Keys key)
        {
            this.Key = key;
        }
    }
}
