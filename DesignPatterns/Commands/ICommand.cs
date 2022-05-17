namespace EksamensProjekt2022
{
    public interface ICommand
    {
        void Execute(Player player);
        void Execute(Camera camera);
    }
}
