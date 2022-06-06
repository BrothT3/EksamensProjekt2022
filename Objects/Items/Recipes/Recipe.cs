using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EksamensProjekt2022
{
    public abstract class Recipe
    {
        public string Name { get; set; }
        public bool[] Available { get; set; }
        public bool AllAvailable { get; set; }
        public bool Click { get; set; }
        public List<Item> Ingredients { get; set; }
        public Button RecipeButton { get; set; }
        public abstract void Craft(object sender, EventArgs e);
        internal abstract EventHandler Craft();
        public Recipe()
        {

        }
    }
    public class FermentedBreastMilkRecipe : Recipe
    {


        public FermentedBreastMilkRecipe()
        {
            Name = "fish";
            Ingredients = new List<Item>();
            Ingredients.Add(new Stone(1));
            Available = new bool[1];
            AllAvailable = false;
            RecipeButton = new Button(Name);
        }
        public override void Craft(object sender, EventArgs e)
        {
            if (Click)
            {
                Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
                inv.RemoveItem(Ingredients[0], Ingredients[0].Quantity);
                inv.AddItem(new Fish(1));
                Click = false;
                GameControl.Instance.playing.userInterface.Updated = false;
            }

        }

        internal override EventHandler Craft()
        {
            Click = true;
            return Craft;
        }
    }
    public class ChestRecipe : Recipe
    {
        public ChestRecipe()
        {
            Name = "Chest";
            Ingredients = new List<Item>();
            Ingredients.Add(new Wood(3));
            Ingredients.Add(new Stone(1));
            Available = new bool[2];
            AllAvailable = false;
            RecipeButton = new Button(Name);

        }
        public override void Craft(object sender, EventArgs e)
        {
            if (Click)
            {
                Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
                inv.RemoveItem(Ingredients[0], Ingredients[0].Quantity);
                inv.RemoveItem(Ingredients[1], Ingredients[1].Quantity);
                inv.AddItem(new Wood(4));
                Click = false;
                GameControl.Instance.playing.userInterface.Updated = false;
            }

        }

        internal override EventHandler Craft()
        {
            Click = true;
            return Craft;
        }

    }
    public class CookedFishRecipe : Recipe
    {
        public bool toBeCooked;

        public CookedFishRecipe()
        {
            Name = "CookedFish";
            Ingredients = new List<Item>();
            Ingredients.Add(new Fish(1));
            Available = new bool[1];
            AllAvailable = false;
            RecipeButton = new Button(Name);

        }
        public override void Craft(object sender, EventArgs e)
        {
            if (Click)
            {
                Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
                GameObject campFire = GameControl.Instance.playing.currentGameObjects.First(x => x.Tag == "selectedBuilding");
                CampFire cf = campFire.GetComponent<CampFire>() as CampFire;
                Inventory cfinv = campFire.GetComponent<Inventory>() as Inventory;
                if (cfinv.items.Count + cf.CurrentlyCooking <= cfinv.InventoryMax)
                {
                    cf.Cook();
                    inv.RemoveItem(Ingredients[0], Ingredients[0].Quantity);

                }
                GameControl.Instance.playing.userInterface.Updated = false;

                Click = false;
            }

        }

        internal override EventHandler Craft()
        {
            Click = true;
            return Craft;
        }


    }
    public class EatFishRecipe : Recipe
    {
        public bool toBeCooked;

        public EatFishRecipe()
        {
            Name = "EatFish";
            Ingredients = new List<Item>();
            Ingredients.Add(new CookedFish(1));
            Available = new bool[1];
            AllAvailable = false;
            RecipeButton = new Button(Name);

        }
        public override void Craft(object sender, EventArgs e)
        {
            if (Click)
            {
                Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
                GameObject campFire = GameControl.Instance.playing.currentGameObjects.First(x => x.Tag == "selectedBuilding");
                CampFire cf = campFire.GetComponent<CampFire>() as CampFire;
                Inventory cfinv = campFire.GetComponent<Inventory>() as Inventory;
                inv.RemoveItem(Ingredients[0], Ingredients[0].Quantity);
                SurvivalAspect sa = player.GameObject.GetComponent<SurvivalAspect>() as SurvivalAspect;
                sa.CurrentHunger -= 10;
                sa.Heal(5);

                //player.hunger gå så op duJAJAJAJAJAJA
                // HUSK DEN HUNGER DER
                Click = false;
                GameControl.Instance.playing.userInterface.Updated = false;
            }

        }

        internal override EventHandler Craft()
        {
            Click = true;
            return Craft;
        }


    }
}

