using Microsoft.Xna.Framework;

namespace EksamensProjekt2022
{
    public class Player : Component
    {
        private float speed;
        private bool canShoot = true;
        private Cell currentCell;
        public Cell nextCell;
        private Vector2 currentVector;
        public Vector2 nextVector;
        private Vector2 moveDir;
        private Animator animator;
        private Vector2 start = new Vector2(4, 9);
        private Vector2 end = new Vector2(3, 8);
        public void Move(Vector2 _velocity)
        {
            if (_velocity != Vector2.Zero)
            {
                _velocity.Normalize();
            }

            _velocity *= speed;

            GameObject.Transform.Translate(_velocity * GameWorld.DeltaTime);
        }

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
            GameObject.Transform.Position = new Vector2(currentCell.Position.X * 35 - 19, currentCell.Position.Y * 35 - 37);


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

            if (currentCell != null)
                currentVector = new Vector2(currentCell.cellVector.X - 19, currentCell.cellVector.Y - 37);
            if (nextCell != null)
                nextVector = new Vector2(nextCell.cellVector.X - 19, nextCell.cellVector.Y - 37);
            moveDir = nextVector - currentVector;
            moveDir.Normalize();
            GameObject.Transform.Position += moveDir;



        }
    }
}
