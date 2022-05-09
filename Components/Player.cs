using Microsoft.Xna.Framework;

namespace EksamensProjekt2022
{
    public class Player : Component
    {
        private float speed;
        private bool canShoot = true;
        public Cell currentCell;
        public Cell nextCell;
        private Vector2 currentVector;
        public Vector2 nextVector;
        private Vector2 moveDir;
        private Animator animator;
        private Vector2 start = new Vector2(4, 9);
        private Vector2 end = new Vector2(3, 8);
        public int step = 1;
        //public void Move(Vector2 _velocity)
        //{
        //    if (_velocity != Vector2.Zero)
        //    {
        //        _velocity.Normalize();
        //    }

        //    _velocity *= speed;

        //    GameObject.Transform.Translate(_velocity * GameWorld.DeltaTime);
        //}

        public override void Awake()
        {
            speed = 200;
        }

        public override void Start()
        {
            SpriteRenderer sr = GameObject.GetComponent<SpriteRenderer>() as SpriteRenderer;
            // sr.SetSprite("Insert sprite path here");
            sr.SetSprite("MinerTest");
            currentCell = GameWorld.Cells[start.ToPoint()];
            GameObject.Transform.Position = new Vector2(currentCell.cellVector.X - 19, currentCell.cellVector.Y + 37);



            animator = (Animator)GameObject.GetComponent<Animator>();

        }
        //currentCell = GameWorld.Cells[GameObject.Transform.Position.ToPoint()];
        public override void Update(GameTime gameTime)
        {
            InputHandler.Instance.Execute(this);
            InputHandler.Instance.Update(gameTime);
            //  currentCell = GameWorld.Cells[start.ToPoint()];
            //  nextCell = GameWorld.Cells[end.ToPoint()];
            //currentVector = new Vector2(currentCell.Position.X * 35 - 19, currentCell.Position.Y * 35 - 37);
            //nextVector = new Vector2(nextCell.Position.X * 35 - 19, nextCell.Position.Y * 35 - 37);
            FollowPath();
 
            //if (InputHandler.Instance.finalPath != null)
            //{
               
            //    InputHandler.Instance.finalPath.Reverse();
                
            //    int iMax = InputHandler.Instance.finalPath.Count;

            //    nextVector = new Vector2(InputHandler.Instance.finalPath[step].NodeVector.X, InputHandler.Instance.finalPath[step].NodeVector.Y);
            //    //currentVector = new Vector2(InputHandler.Instance.finalPath[i].NodeVector.X, InputHandler.Instance.finalPath[i].NodeVector.Y);

            //    //currentVector = new Vector2(currentCell.cellVector.X, currentCell.cellVector.Y);
            //    //if (currentCell != null)
            //    //    currentVector = new Vector2(currentCell.MyNode.NodeVector.X - 19, currentCell.MyNode.NodeVector.Y - 37);

            //    moveDir = nextVector - GameObject.Transform.Position;
            //    moveDir.Normalize();
            //    GameObject.Transform.Position += moveDir;

            //    if (Vector2.Distance(GameObject.Transform.Position, nextVector) < 20)
            //    {
            //        if (step < iMax)
            //        {

            //            step++;
            //            currentCell = InputHandler.Instance.finalPath[step - 1].CellParent;
            //        }
            //        else if (step >= iMax)
            //        {
            //            step = 0;
            //            InputHandler.Instance.finalPath.Clear();
            //        }
            //    }


            //}





        }

        public void FollowPath()
        {
            if (InputHandler.Instance.finalPath != null)
            {
                InputHandler.Instance.finalPath.Reverse();
                currentCell = InputHandler.Instance.finalPath[step].CellParent;
                nextCell = InputHandler.Instance.finalPath[step+1].CellParent;
                moveDir = nextCell.cellVector - currentCell.cellVector;
                moveDir.Normalize();
                GameObject.Transform.Position += moveDir;
                if (Vector2.Distance(GameObject.Transform.Position, nextVector) < 10)
                {
                    step++;
                }
            }
        }
        //public void FollowPath()
        //{
        //    if (InputHandler.Instance.finalPath != null)
        //    {

        //        for (int i = 0; i < InputHandler.Instance.finalPath.Count; i++)
        //        {
        //            currentVector = InputHandler.Instance.finalPath[i].NodeVector;
        //            nextVector = InputHandler.Instance.finalPath[i + 1].NodeVector;
        //            if (nextVector == currentVector)
        //            {
        //                i++;
        //            }
        //        }

        //    }
        //}
    }
}
