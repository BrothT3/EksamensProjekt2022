﻿using Microsoft.Xna.Framework;

namespace EksamensProjekt2022
{
    public class MoveCommand : ICommand
    {
        private Vector2 velocity;


        public MoveCommand(Vector2 velocity)
        {
            this.velocity = velocity;
        }


        public void Execute(Camera camera)
        {
            camera.Move(velocity);         
        }
        public void Execute(Player player)
        {
            //player.Move(target);
        }
    }
}
