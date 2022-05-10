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
            GameObject.Transform.Position = new Vector2(currentCell.cellVector.X, currentCell.cellVector.Y);



            animator = (Animator)GameObject.GetComponent<Animator>();

        }
        public override void Update(GameTime gameTime)
        {
            InputHandler.Instance.Execute(this);
            InputHandler.Instance.Update(gameTime);
            
            FollowPath();

        }

        public void FollowPath()
        {

            if (InputHandler.Instance.finalPath != null)
            {
                if (step + 1 < InputHandler.Instance.finalPath.Count)
                {
                    currentCell = InputHandler.Instance.finalPath[step].CellParent;
                    nextCell = InputHandler.Instance.finalPath[step + 1].CellParent;
                    moveDir = (nextCell.cellVector) - (currentCell.cellVector);
                    moveDir.Normalize();
                    GameObject.Transform.Position += moveDir;
                }
                if (step + 1 >= InputHandler.Instance.finalPath.Count)
                {

                    currentCell = InputHandler.Instance.finalPath[step].CellParent;
                    //moveDir = currentCell.cellVector - GameObject.Transform.Position;
                    //moveDir.Normalize();
                    //GameObject.Transform.Position += moveDir;
                    InputHandler.Instance.finalPath.Clear();
                }               
                
                if (Vector2.Distance(GameObject.Transform.Position, nextCell.cellVector) < 4 )
                {
                    step++;
                }

            }
            if (InputHandler.Instance.finalPath == null)
            {
                

            }
            
        }

    }
}
