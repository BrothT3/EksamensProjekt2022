using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EksamensProjekt2022
{
    public class Chest : Building
    {
        private Color Color = Color.White;
        private SpriteFont font;
        public Chest(Point parentPoint) : base(parentPoint)
        {
            font = GameWorld.Instance.Content.Load<SpriteFont>("Font");
            
        }
        public override void Start()
        {
            GameObject.Transform.Position = new Vector2(parentCell.cellVector.X, parentCell.cellVector.Y);
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            sr.OffSet += 18; 
            Occupy();
        }
        public override void Update(GameTime gameTime)
        {
            if (IsSelected)
            {
                Color = Color.Red;
            }
            else
            {
                Color = Color.Green;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Inventory inv = GameObject.GetComponent<Inventory>() as Inventory;
            if (IsSelected)
            {
                
                Vector2 firstItemSlot = new Vector2(GameObject.Transform.Position.X, GameObject.Transform.Position.Y - 36);
                if (inv != null)
                    foreach (Item item in inv.items)
                    {
                        spriteBatch.Draw(item.Sprite, firstItemSlot, Color.White);
                        spriteBatch.DrawString(font, $"{item.Quantity}", new Vector2(firstItemSlot.X, firstItemSlot.Y), Color);
                        firstItemSlot.Y -= 36;
                    }
            }
            
        }


        public override void Occupy()
        {
            Cells.Add((GameControl.Instance.playing.currentCells[new Point(parentPoint.X, parentPoint.Y)]));
            Cells.Add((GameControl.Instance.playing.currentCells[new Point(parentPoint.X + 1, parentPoint.Y)]));
            foreach (Cell cell in Cells)
            {
                if (GameControl.Instance.playing.currentGrid.Exists(x => (x.Position == cell.Position)))
                {
                    Cell _cell = GameControl.Instance.playing.currentGrid.First(x => x.Position == cell.Position);
                    _cell.IsWalkable = false;
                }
            }
            //2 forskellige objekter på 2 forskellige lister, find ud af hvordan de ka referer til det samme på 2 lister
        }
    }
}
