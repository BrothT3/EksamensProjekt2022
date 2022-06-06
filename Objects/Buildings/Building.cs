using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace EksamensProjekt2022
{
    public abstract class Building : Component
    {
        protected string buildingTag;
        protected Cell parentCell;
        private List<Cell> cells = new List<Cell>();
        protected Point parentPoint;
        public Cell ParentCell { get => parentCell; set => parentCell = value; }

        public List<Cell> Cells { get => cells; set => cells = value; }

        public Building(Point _parentPoint)
        {
            parentPoint = _parentPoint;
            parentCell = GameControl.Instance.playing.currentGrid.Find(x => x.Position == parentPoint);
        }

        public abstract void Occupy();
        
        public bool IsSelected
        {
            get
            {
                if (GameControl.Instance.playing.selectedCell != null)
                {
                    if (Cells.Exists(x => (x.Position == GameControl.Instance.playing.selectedCell.Position)))   
                    {
                        this.buildingTag = "selectedBuilding";
                        return true;
                    }
                    else
                    {
                        buildingTag = "Building";
                        return false;
                    }
                }
                else
                    return false;
        
            }
        }

        
    }

}
