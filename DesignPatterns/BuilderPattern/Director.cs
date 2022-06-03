namespace EksamensProjekt2022
{
    public class Director
    {
        private PlayerBuilder builder;

        public Director(PlayerBuilder builder)
        {
            this.builder = builder;
        }

        public GameObject Construct(int health, int energy, int hunger, int x, int y, int time)
        {
            builder.BuildGameObject(health, energy, hunger, x, y, time);
            return builder.GetResult();
        }
    }
}
