namespace EksamensProjekt2022
{
    public interface IBuilder
    {
        void BuildGameObject(int health, int energy, int hunger, int x, int y, int time);
        GameObject GetResult();
    }
}
