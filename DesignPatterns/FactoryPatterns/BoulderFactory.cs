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

            gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            sr.SetSprite("AreaSprites/Rock");
        //    gameObject.AddComponent(new Collider());

            gameObject.AddComponent(new Boulder(cell, resourceAmount));
            gameObject.Transform.Position = new Vector2(cell.cellVector.X, cell.cellVector.Y + (GameWorld.Instance.CellSize / 2));
            cell.IsWalkable=false;

            return gameObject;

        }
    }
}
