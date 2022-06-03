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
        private bool isNew;
        private int height;
        private int width;
        public ResourceDepot myResource;

        private Node myNode;
        private CellType myType = CellType.Empty;

        private bool isHovering;

        public Vector2 cellVector;    

        public Point Position { get => position; set => position = value; }
        public Node MyNode { get => myNode; set => myNode = value; }
        public Color BackGroundColor { get; set; } = Color.White * 0.1f;
        public CellType MyType { get { return myType; } }
        public bool IsWalkable { get; set; } 
        public bool IsNew { get; set; }

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
                || background.Contains(new Point(mstate.X - (int)MapCreator.Instance.Camera.Position.X, mstate.Y - (int)MapCreator.Instance.Camera.Position.Y)) && MapCreator.DevMode)
            {
                isHovering = true;
                
            }
            if (Sprite == null && !MapCreator.DevMode)
            {
                IsWalkable = false;
            }

        }

        public void LoadContent()
        {
           // lineSprite = GameWorld.Instance.pixel;

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

            spriteBatch.Draw(GameWorld.Instance.pixel, background, color);

#if DEBUG
            //path
            spriteBatch.Draw(GameWorld.Instance.pixel, background, BackGroundColor);
            spriteBatch.Draw(GameWorld.Instance.pixel, topLine, edgeColor);
            spriteBatch.Draw(GameWorld.Instance.pixel, bottomLine, edgeColor);
            spriteBatch.Draw(GameWorld.Instance.pixel, rightLine, edgeColor);
            spriteBatch.Draw(GameWorld.Instance.pixel, leftLine, edgeColor);          
#endif

        }
    }
}
