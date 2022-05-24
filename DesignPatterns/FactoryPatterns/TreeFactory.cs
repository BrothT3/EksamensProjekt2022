﻿using Microsoft.Xna.Framework;

namespace EksamensProjekt2022
{
    public class TreeFactory : Factory
    {
        private static TreeFactory instance;

        public static TreeFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TreeFactory();
                }
                return instance;
            }

        }

        private TreeFactory()
        {


        }
        public override GameObject CreateGameObject(Cell cell, int resourceAmount)
        {
            GameObject gameObject = new GameObject();
            SpriteRenderer sr = (SpriteRenderer)gameObject.AddComponent(new SpriteRenderer());
            sr.SetSprite("AreaSprites/Tree");
            Collider c = (Collider)gameObject.AddComponent(new Collider());

            gameObject.AddComponent(new Tree(cell, resourceAmount));
            gameObject.Transform.Position = new Vector2(cell.cellVector.X ,
                cell.cellVector.Y );
            cell.IsWalkable = false;
            gameObject.Tag = "Tree";
            gameObject.Amount = resourceAmount;

            return gameObject;
        }
    }
}
