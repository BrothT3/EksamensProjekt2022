namespace EksamensProjekt2022
{
    public class Director
    {
        private PlayerBuilder builder;

        public Director(PlayerBuilder builder)
        {
            this.builder = builder;
        }

        public GameObject Construct()
        {
            builder.BuildGameObject();
            return builder.GetResult();
        }
    }
}
