using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
namespace EksamensProjekt2022
{
    public class Player : Component
    {
        private CurrentArea myArea;

        public Cell currentCell;
        public Cell nextCell;
        private Vector2 currentVector;
        public Vector2 nextVector;
        private Vector2 moveDir;
        private Animator animator;
        private Vector2 start = new Vector2(4, 9);
        private Vector2 end = new Vector2(3, 8);
        public int step = 0;
        public bool readyToMove = false;
        public CurrentArea MyArea { get => myArea; set => myArea = value; }

        public override void Awake()
        {

        }

        public override void Start()
        {
            myArea = CurrentArea.Camp;
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            // sr.SetSprite("Insert sprite path here");
            sr.SetSprite("MinerTest");
            currentCell = GameControl.Instance.playing.currentCells[start.ToPoint()];
            GameObject.Transform.Position = new Vector2(currentCell.cellVector.X, currentCell.cellVector.Y);



            animator = (Animator)GameObject.GetComponent<Animator>();

        }
        public override void Update(GameTime gameTime)
        {
            InputHandler.Instance.Execute(this);
            InputHandler.Instance.Update(gameTime);
            PlayerAnimate();
            FollowPath();

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
    }
}
