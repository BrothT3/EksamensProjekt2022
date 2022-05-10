using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    class Astar
    {
        private List<Node> closed;

        private List<Node> open;

        public List<Node> Closed { get => closed; set => closed = value; }
        public List<Node> Open { get => open; set => open = value; }


        public Astar()
        {
            closed = new List<Node>();
            open = new List<Node>();
        }


        public List<Node> FindPath(Point myStart, Point myGoal, List<Node> nodes)
        {
            Point start = myStart;
            Point goal = myGoal;

            List<Node> finalPath = new List<Node>();
            finalPath.Clear();
            closed.Clear();
            open.Clear();

            Node currentNode = nodes.Find(x => x.Position == start);

            open.Add(currentNode);

            while (true)
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if (x != 0 || y != 0)
                        {
                            Node neighbor = nodes.Find(node => node.Position.X == currentNode.Position.X - x && node.Position.Y == currentNode.Position.Y - y);

                            if (neighbor != null)
                            {
                                int gCost = 0;

                                if (Math.Abs(x - y) % 2 == 1)
                                {
                                    gCost = 10;
                                }
                                else
                                {
                                    gCost = 14;
                                }

                                if (open.Exists(n => n == neighbor))
                                {
                                    if (currentNode.G + gCost < neighbor.G)
                                    {
                                        neighbor.CalculateValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                    }
                                }
                                else if (!closed.Exists(n => n == neighbor))
                                {
                                    if (gCost == 14)
                                    {
                                        if (currentNode.Position.X < neighbor.Position.X && currentNode.Position.Y > neighbor.Position.Y - 1) //TopRight
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X, currentNode.Position.Y - 1)
                                            && nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X + 1, currentNode.Position.Y))))
                                            {
                                                open.Add(neighbor);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbor.CalculateValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                        else if (currentNode.Position.X > neighbor.Position.X && currentNode.Position.Y < neighbor.Position.Y) //BottomLeft
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X, currentNode.Position.Y + 1) &&
                                            nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X - 1, currentNode.Position.Y))))
                                            {
                                                open.Add(neighbor);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbor.CalculateValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                        else if (currentNode.Position.X > neighbor.Position.X && currentNode.Position.Y > neighbor.Position.Y) //TopLeft
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X, currentNode.Position.Y - 1)
                                            && nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X - 1, currentNode.Position.Y))))
                                            {
                                                open.Add(neighbor);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbor.CalculateValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                        else if (currentNode.Position.X < neighbor.Position.X && currentNode.Position.Y < neighbor.Position.Y) // bottom right
                                        {
                                            if (nodes.Exists(node => node.Position == new Point(currentNode.Position.X + 1, currentNode.Position.Y)
                                            && nodes.Exists(node2 => node2.Position == new Point(currentNode.Position.X, currentNode.Position.Y + 1))))
                                            {
                                                open.Add(neighbor);
                                                open.Remove(currentNode);
                                                closed.Add(currentNode);
                                                neighbor.CalculateValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        open.Add(neighbor);
                                        open.Remove(currentNode);
                                        closed.Add(currentNode);
                                        neighbor.CalculateValues(currentNode, nodes.Find(node => node.Position == goal), gCost);
                                    }
                                }
                            }
                        }
                    }
                }

                if (open.Count == 0)
                {
                    break;
                }

                open.Sort();

                currentNode = open[0];
                open.Remove(currentNode);
                closed.Add(currentNode);

                if (currentNode.Position == goal)
                {
                    closed.Add(currentNode);
                    break;
                }
            }

            Node tmpNode = closed.Find(x => x.Position == goal);

            if (tmpNode != null)
            {
                while (!finalPath.Exists(x => x.Position == start))
                {
                    finalPath.Add(tmpNode);
                    tmpNode = tmpNode.Parent;
                }
            }

            return finalPath;
        }
    }

}
