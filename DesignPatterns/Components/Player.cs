using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace EksamensProjekt2022
{
    public class Player : Component
    {
        private Area myArea;
        public Cell currentCell;
        public Cell nextCell;
        public Vector2 nextVector;
        private Vector2 moveDir;
        public Cell selectedCell;
        private Animator animator;
        private SurvivalAspect survivalAspect;
        public int step = 0;
        public bool readyToMove = false;
        private float countDown = 2;
        public Area MyArea { get => myArea; set => myArea = value; }


        public override void Start()
        {
            myArea = Area.Camp;
            survivalAspect = GameObject.GetComponent<SurvivalAspect>() as SurvivalAspect;
            animator = (Animator)GameObject.GetComponent<Animator>();
            survivalAspect.DeathEvent += OnDeathEvent;


        }
        public override void Update(GameTime gameTime)
        {
            InputHandler.Instance.Execute(this);
            InputHandler.Instance.Update(gameTime);
            PlayerAnimate();
            FollowPath();

            if (selectedCell != null && selectedCell.myResource is ResourceDepot
                && readyToMove)
            {
                ResourceGather();
            }

            if (!currentCell.IsWalkable)
            {
                GameObject.Transform.Position = GameControl.Instance.playing.currentCells[new Point(15, 10)].cellVector;
                currentCell = GameControl.Instance.playing.currentGrid.Find(x => x.cellVector == GameObject.Transform.Position);
            }
        }

        private void OnDeathEvent(object sender, EventArgs e)
        {
            GameControl.Instance.playing.Destroy(this.GameObject);
        }

        public void FollowPath()
        {

            if (InputHandler.Instance.finalPath != null)
            {
                if (Vector2.Distance(GameObject.Transform.Position, currentCell.cellVector) > 8 && readyToMove == false)
                {
                    moveDir = currentCell.cellVector - GameObject.Transform.Position;
                    moveDir.Normalize();
                    moveDir *= 4;
                    GameObject.Transform.Position += moveDir;
                }
                else
                {
                    readyToMove = true;
                }
                if (readyToMove)
                {
                    if (step + 1 < InputHandler.Instance.finalPath.Count)
                    {
                        currentCell = InputHandler.Instance.finalPath[step].CellParent;
                        nextCell = InputHandler.Instance.finalPath[step + 1].CellParent;
                        moveDir = (nextCell.cellVector) - (currentCell.cellVector);
                        moveDir.Normalize();
                        moveDir *= 4;
                        GameObject.Transform.Position += moveDir;
                    }

                    if (step + 1 >= InputHandler.Instance.finalPath.Count)
                    {

                        currentCell = nextCell;
                        if (Vector2.Distance(GameObject.Transform.Position, currentCell.cellVector) > 4)
                        {
                            moveDir = currentCell.cellVector - GameObject.Transform.Position;
                            moveDir.Normalize();
                            GameObject.Transform.Position += moveDir;
                        }

                        InputHandler.Instance.finalPath.Clear();

                    }

                    if (Vector2.Distance(GameObject.Transform.Position, nextCell.cellVector) < 8)
                    {
                        step++;
                    }

                }
                if (InputHandler.Instance.finalPath == null)
                {


                }
            }

        }
        public void PlayerAnimate()
        {
            if (currentCell == nextCell || nextCell == null)
                animator.PlayAnimation("playerIdle");

            if (nextCell != null)
            {
                if (nextCell.Position.Y > currentCell.Position.Y)
                    animator.PlayAnimation("playerWalkDown");
                else if (nextCell.Position.Y < currentCell.Position.Y)
                    animator.PlayAnimation("playerWalkUp");
                else
                {
                    if (nextCell.Position.X > currentCell.Position.X)
                        animator.PlayAnimation("playerWalkRight");

                    else if (nextCell.Position.X < currentCell.Position.X)
                        animator.PlayAnimation("playerWalkLeft");
                }
            }

        }

   
        private void ResourceGather()
        {
            countDown -= GameWorld.DeltaTime;
            Inventory inv = GameObject.GetComponent<Inventory>() as Inventory;

            if (Vector2.Distance(selectedCell.cellVector, GameObject.Transform.Position) < 64)
            {
                if (selectedCell.myResource is Tree && countDown <= 0 && selectedCell.myResource.Amount > 0)
                {

                    GameWorld.Instance.woodChop.Play(0.5f, 0, 0);
                    inv.AddItem(new Wood(2));
                    selectedCell.myResource.Amount -= 2;
                    countDown = 2;
                }
                else if (selectedCell.myResource is Boulder && countDown <= 0 && selectedCell.myResource.Amount > 0)
                {
                    GameWorld.Instance.rockHit.Play();

                    inv.AddItem(new Stone(2));
                    selectedCell.myResource.Amount -= 2;
                    countDown = 2;
                }
            }

        }

    


    }
}
