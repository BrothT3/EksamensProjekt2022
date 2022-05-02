using System;

namespace EksamensProjekt2022
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = GameWorld.Instance)
                game.Run();
        }
    }
}
