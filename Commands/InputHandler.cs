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


        }

        public void Update(GameTime gameTime)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            foreach (Cell c in GameWorld.Cells.Values)
            {

                if (c.background.Intersects(new Rectangle(mouseState.X, mouseState.Y, 10, 10)) && mouseState.LeftButton == ButtonState.Pressed)
                {

                    target = c.Position;
                    player.nextCell = c;


                }

            }

        }
        public void Execute(Player player)
        {
            KeyboardState keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();


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
