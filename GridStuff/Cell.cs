using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjekt2022
{
    public enum CellType { Empty, Resource, Water }
    public class Cell
    {
        private Vector2 _pos;
        private Point position;
        private Texture2D sprite;
        private Color spriteColor = Color.White;
        private Color edgeColor = Color.Black;
        private bool isWalkable;

        private int height;
        private int width;

        private Node myNode;
        private CellType myType = CellType.Empty;

        public Point Position { get => position; set => position = value; }
        public Node MyNode { get => myNode; set => myNode = value; }
        public Color SpriteColor { get => spriteColor; set => spriteColor = value; }
        public CellType MyType { get { return myType; } }
        public bool IsWalkable { get => isWalkable; set => isWalkable = value; }

        #region rectangles
        private Rectangle topLine;

        private Rectangle bottomLine;

        private Rectangle rightLine;

        private Rectangle leftLine;

        private Rectangle background;
        #endregion


        public Cell(Point position, int width, int height)
        {
            this.position = position;
            this.width = width;
            this.height = height;

            IsWalkable = true;
        }


        public void LoadContent()
        {
            sprite = GameWorld.Instance.Content.Load<Texture2D>("Pixel");

            topLine = new Rectangle(Position.X * width, Position.Y * height, width, 1);

            bottomLine = new Rectangle(Position.X * width, (Position.Y * height) + height, width, 1);

            rightLine = new Rectangle((Position.X * width) + width, Position.Y * height, 1, height);

            leftLine = new Rectangle(Position.X * width, Position.Y * height, 1, height);

           // background = new Rectangle(Position.X * width, Position.Y * height, width, height);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, background, spriteColor);
            spriteBatch.Draw(sprite, topLine, edgeColor);
            spriteBatch.Draw(sprite, bottomLine, edgeColor);
            spriteBatch.Draw(sprite, rightLine, edgeColor);
            spriteBatch.Draw(sprite, leftLine, edgeColor);

        }
    }
}
