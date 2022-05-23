using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EksamensProjekt2022
{
    public enum CellType { Empty, Resource, Water }
    public class Cell
    {
        private Point position;
        public Texture2D Sprite { get; set; }
        private Texture2D lineSprite; 
        private Color spriteColor = Color.White;
        private Color edgeColor = Color.Black;
        public MouseState mstate { get; set; }
      
        private bool isWalkable;

        private int height;
        private int width;

        private Node myNode;
        private CellType myType = CellType.Empty;

        private bool isHovering;

        public Vector2 cellVector;    

        public Point Position { get => position; set => position = value; }
        public Node MyNode { get => myNode; set => myNode = value; }
        public Color BackGroundColor { get; set; } = Color.White * 0.1f;
        public CellType MyType { get { return myType; } }
        public bool IsWalkable { get => isWalkable; set => isWalkable = value; }

        #region rectangles
        private Rectangle topLine;

        private Rectangle bottomLine;

        private Rectangle rightLine;

        private Rectangle leftLine;

        public Rectangle background
        {
            get
            {
                return new Rectangle(Position.X * width, Position.Y * height, width, height);
            }
        }
        #endregion


        public Cell(Point position, int width, int height)
        {
            this.position = position;
            this.width = width;
            this.height = height;
            IsWalkable = true;
            cellVector = new Vector2((position.X+1) * width - (width / 2), (position.Y+1) * height - (height));
        }
        public void Update(GameTime gameTime)
        {
            mstate = Mouse.GetState();
            isHovering = false;
            if (background.Contains(new Point(mstate.X - (int)GameControl.Instance.camera.Position.X, mstate.Y - (int)GameControl.Instance.camera.Position.Y)) && !MapCreator.DevMode
                || background.Contains(new Point(mstate.X - (int)MapCreator.Instance.camera.Position.X, mstate.Y - (int)MapCreator.Instance.camera.Position.Y)) && MapCreator.DevMode)
            {
                isHovering = true;

            }


        }

        public void LoadContent()
        {
            lineSprite = GameWorld.Instance.Content.Load<Texture2D>("Pixel");

#if DEBUG
            topLine = new Rectangle(Position.X * width, Position.Y * height, width, 1);
            bottomLine = new Rectangle(Position.X * width, (Position.Y * height) + height, width, 1);
            rightLine = new Rectangle((Position.X * width) + width, Position.Y * height, 1, height);
            leftLine = new Rectangle(Position.X * width, Position.Y * height, 1, height);
#endif
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            var color = Color.White * 0.1f;
            if (isHovering)
            {
                color = Color.Green * 0.5f;
            }
            //bare for at at man kan se hvor man har musen og selve pathen, men pathen er også bare et debug tool
            //mouseover
            if (Sprite != null)
                spriteBatch.Draw(Sprite, new Vector2(background.X, background.Y), Color.White);

            spriteBatch.Draw(lineSprite, background, color);

#if DEBUG
            //path
            spriteBatch.Draw(lineSprite, background, BackGroundColor);
            spriteBatch.Draw(lineSprite, topLine, edgeColor);
            spriteBatch.Draw(lineSprite, bottomLine, edgeColor);
            spriteBatch.Draw(lineSprite, rightLine, edgeColor);
            spriteBatch.Draw(lineSprite, leftLine, edgeColor);          
#endif

        }
    }
}
