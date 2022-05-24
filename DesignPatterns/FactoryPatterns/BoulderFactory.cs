using Microsoft.Xna.Framework;

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

        public override GameObject CreateGameObject(Cell cell, int resourceAmount)
        {


            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            sr.SetSprite("AreaSprites/Rock");
            Collider c = (Collider)gameObject.AddComponent(new Collider());

            gameObject.AddComponent(new Boulder(cell, resourceAmount));
            //with the offset the database can't find the cell
            gameObject.Transform.Position = new Vector2(cell.cellVector.X, cell.cellVector.Y);
            cell.IsWalkable = false;
            gameObject.Tag = "Boulder";
            gameObject.Amount = resourceAmount;

            return gameObject;

        }
    }
}
