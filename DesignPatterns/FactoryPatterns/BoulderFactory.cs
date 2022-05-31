using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EksamensProjekt2022
{
    public class BoulderFactory : Factory
    {

        private static BoulderFactory instance;

        public static BoulderFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BoulderFactory();
                }


                return instance;
            }
        }

        private BoulderFactory()
        {

        }

        public override GameObject CreateGameObject(Cell cell, int resourceAmount, bool isNew)
        {


            GameObject gameObject = new GameObject();
            gameObject.IsNew = isNew;
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            sr.SetSprite("AreaSprites/Rock");
            Random rand = new Random();
            if (rand.Next(0,2) > 0)
            {
                sr.SpriteEffect = SpriteEffects.FlipHorizontally;
            }
            

            Collider c = (Collider)gameObject.AddComponent(new Collider());
            gameObject.AddComponent(new Boulder(cell, resourceAmount));
            sr.OffSetY = GameControl.Instance.playing.CellSize/2;
            gameObject.Transform.Position = new Vector2(cell.cellVector.X, cell.cellVector.Y);
            cell.IsWalkable = false;
            gameObject.Tag = "Boulder";
            gameObject.Amount = resourceAmount;

            return gameObject;

        }
    }
}
