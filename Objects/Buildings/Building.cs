using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public abstract class Building
    {
        protected Cell parentCell;
        protected Cell[] otherCells;

        public Cell ParentCell { get => parentCell; set => parentCell = value; }
        public Cell[] OtherCells { get => otherCells; set => otherCells = value; }

        public Building(Cell parentCell)
        {
            ParentCell = parentCell;
        }

        public abstract void Occupy();
    }
}
