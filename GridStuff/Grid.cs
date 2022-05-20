using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace EksamensProjekt2022
{
    public class Grid
    {


        private int cellCount;
        private int cellSizeX;
        private int cellSizeY;


        public Grid(int cellcount, int cellsizex, int cellsizey)
        {
            this.cellCount = cellcount;
            this.cellSizeX = cellsizex;
            this.cellSizeY = cellsizey;
        }

        public Dictionary<Point, Cell> CreateCells()
        {
            Dictionary<Point, Cell> grid = new Dictionary<Point, Cell>();
            for (int y = 0; y < cellCount; y++)
            {
                for (int x = 0; x < cellCount; x++)
                {
                    grid.Add(new Point(x, y), new Cell(new Point(x, y), (int)cellSizeX, cellSizeY));
                }
            }

            return grid;
        }

        /// <summary>
        /// Creates and returns a grid with the dimensions set in the Grid class constructor
        /// </summary>
        /// <returns></returns>
        public List<Cell> CreateGrid()
        {
            List<Cell> grid = new List<Cell>();
            grid.Clear();

            for (int x = 0; x < cellCount; x++)
            {
                for (int y = 0; y < cellCount; y++)
                {
                    grid.Add(new Cell(new Point(x, y), (int)cellSizeX, (int)cellSizeY));
                }

            }


            return grid;
        }

        /// <summary>
        /// Creates and returns a list of Nodes used for A* pathfinding
        /// Only takes walkable nodes
        /// </summary>
        /// <returns></returns>
        public List<Node> CreateNodes()
        {
            List<Node> allNodes = new List<Node>();

            foreach (Cell cell in GameControl.Instance.playing.currentGrid)
            {
                if (cell.IsWalkable)
                {
                    cell.MyNode = new Node(cell.Position);
                    cell.MyNode.CellParent = cell;
                    allNodes.Add(cell.MyNode);
                }
            }
            return allNodes;
        }
    }
}
