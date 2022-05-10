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
        private bool mLeftReleased = true;

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
        private Dictionary<KeyState, ICommand> keybinds = new Dictionary<KeyState, ICommand>();
        private Dictionary<ButtonState, ICommand> movement = new Dictionary<ButtonState, ICommand>();

        public InputHandler()
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            //instans af Astar, start og slut punkt (som så bliver spillerens position og hvor end man vil hen
            pathfinder = new Astar();
            //kører algoritmen, den flyttes også ind i input/movement senere men bare for at det virker
            
        }

        public void Update(GameTime gameTime)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            foreach (Cell c in GameWorld.Cells.Values)
            {

                if (c.background.Intersects(new Rectangle(mouseState.X - (int)GameWorld.Instance._camera.Position.X, mouseState.Y - (int)GameWorld.Instance._camera.Position.Y, 10, 10)) && mouseState.LeftButton == ButtonState.Pressed && mLeftReleased)
                {
                    player.step = 0;
                    mLeftReleased = false;
                    start = GameWorld.Cells[player.currentCell.Position];
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
            finalPath = pathfinder.FindPath(start.Position, goal.Position, GameWorld.Instance._grid.CreateNodes());
            ColorNodes();
        }
        public void Execute(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();


        }
        /// <summary>
        /// Colors nodes to display the checked tiles
        /// </summary>
        public void ColorNodes()
        {
            foreach (Cell item in GameWorld.Instance.grid)
            {
                if (pathfinder.Open.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.Blue;
                }
                if (pathfinder.Closed.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.Orange;
                }
                if (finalPath.Exists(x => x.Position == item.Position) && item.Position != start.Position && item.Position != goal.Position)
                {
                    item.BackGroundColor = Color.GreenYellow;
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
