using Microsoft.Xna.Framework;

namespace EksamensProjekt2022
{
    public class MoveCommand : ICommand
    {
        private Vector2 target;


        public MoveCommand(Point target)
        {
            this.target = target.ToVector2();
        }



        public void Execute(Player player)
        {
            //player.Move(target);
        }
    }
}
