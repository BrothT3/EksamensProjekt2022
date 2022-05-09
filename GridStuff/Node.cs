using Microsoft.Xna.Framework;
using System;

namespace EksamensProjekt2022
{
    


    public class Node : IComparable<Node>
    {
        //værdier brugt til at udregne "cost" for at bevæge sig
        private int f;
        private int g;
        private int h;


        private Point position;
        private Node parent;
        private Cell cellParent;
        private Vector2 nodeVector;

        public int G {  get; set; }
        public Point Position { get => position; set => position = value; }
        public Node Parent { get => parent; set => parent = value; }
        public Vector2 NodeVector { get => nodeVector; set => nodeVector = value; }
        public Cell CellParent { get => cellParent; set => cellParent = value; }

        public Node(Point position)
        {
            this.position = position;
            this.NodeVector = new Vector2(position.X*35, position.Y*35);
        }


        public void CalculateValues(Node parentNode, Node goalNode, int cost)
        {
            parent = parentNode;
            g = parent.G + cost;

            h = ((Math.Abs(position.X - goalNode.position.X) + Math.Abs(goalNode.position.Y - position.Y)) * 10);

            f = h + g;
        }

        public int CompareTo(Node other)
        {
            if (f > other.f)
            {
                return 1;
            }
            else if (f < other.f)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
