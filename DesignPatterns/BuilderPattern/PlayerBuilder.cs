using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjekt2022
{
    public class PlayerBuilder : IBuilder
    {
        private GameObject gameObject;
        public void BuildGameObject()
        {
            gameObject = new GameObject();
            gameObject.Tag = "Player";
            BuildComponents();

        }

        private void BuildComponents()
        {
            Player p = (Player)gameObject.AddComponent(new Player());
            p.MyArea = Area.Camp;

            gameObject.AddComponent(new SpriteRenderer());

            Collider c = (Collider)gameObject.AddComponent(new Collider());
     
            Animator a = (gameObject).AddComponent(new Animator()) as Animator;
            a.AddAnimation(BuildAnimation("playerWalkUp", "playerWalkUp"));
            a.AddAnimation(BuildAnimation("playerWalkDown", "playerWalkDown"));
            a.AddAnimation(BuildAnimation("playerWalkLeft", "playerWalkLeft"));
            a.AddAnimation(BuildAnimation("playerWalkRight", "playerWalkRight"));
            a.AddAnimation(BuildAnimation("playerIdle", "playerIdle"));

            Inventory inv = (Inventory)gameObject.AddComponent(new Inventory(5)) as Inventory;
            inv.items.Add(new Stone(3));
        }
        private Animation BuildAnimation(string animationName, string spriteName)
        {
            Texture2D sprite = GameWorld.Instance.Content.Load<Texture2D>($"{spriteName}");

            Animation animation = new Animation(animationName, sprite, 3, 10);

            return animation;
        }
        public GameObject GetResult()
        {
            return gameObject;
        }
    }
}
