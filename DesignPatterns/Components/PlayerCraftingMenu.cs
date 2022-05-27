using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EksamensProjekt2022
{
    public class PlayerCraftingMenu
    {
        public bool addedRecipes = false;
        public bool displayedCrafting = false;
        public List<Button> playerCraftingButtons = new List<Button>();
        public List<Recipe> Recipes = new List<Recipe>();
        public bool craftingMenu = false;
        private bool spaceReleased;
        private Texture2D sprite;
        private Texture2D redX;
        private Vector2 firstRecipeSlot = new Vector2(50, 200);

        public PlayerCraftingMenu()
        {
            sprite = GameWorld.Instance.Content.Load<Texture2D>("MinerTest");
            redX = GameWorld.Instance.Content.Load<Texture2D>("redX");
        }
        public void Update(GameTime gameTime)
        {
            if (!craftingMenu)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && spaceReleased)
                {
                    spaceReleased = false;
                    craftingMenu = true;
                }
            }
            else
            {        
                AddRecipes();
                CheckCrafting();
                DisplayCrafting();

                foreach (Button button in playerCraftingButtons)
                {
                    button.Update(gameTime);
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space) && spaceReleased)
                {
                    spaceReleased = false;
                    playerCraftingButtons.Clear();
                    Recipes.Clear();
                    addedRecipes = false;
                    displayedCrafting = false;
                    craftingMenu = false;
                    firstRecipeSlot = new Vector2(50, 200);
                }


            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                spaceReleased = true;
        }

        public void AddRecipes()
        {
            if (!addedRecipes)
            {
                Recipes.Add(new ChestRecipe());
                Recipes.Add(new FermentedBreatMilkRecipe());
            }
            addedRecipes = true;
        }
        public void CheckCrafting()
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
            foreach (Recipe recipe in Recipes)
            {
                int i = 0;
                foreach (Item item in recipe.Ingredients)
                {

                    if (item.Quantity <= inv.GetItemCount<Item>(item))
                    {
                        recipe.Available[i] = true;
                    }
                    else
                    {
                        recipe.Available[i] = false;
                    }
                    i++;
                }

                if (recipe.Available.All(x => x == true))
                    recipe.RecipeButton.IsAvailable = true;
                else
                    recipe.RecipeButton.IsAvailable = false;
            } 
            addedRecipes = true;
        }
        public void DisplayCrafting()
        {
            if (!displayedCrafting)
            foreach (Recipe recipe in Recipes)
            {
                
                recipe.RecipeButton.OnClicking += recipe.Craft();
                recipe.RecipeButton.Rectangle = new Rectangle((int)firstRecipeSlot.X, (int)firstRecipeSlot.Y, 60, 35);
                playerCraftingButtons.Add(recipe.RecipeButton);
                firstRecipeSlot.Y += 40;
            }
            displayedCrafting = true;
        }


        public void Draw(SpriteBatch spriteBatch)
        {


            foreach (Button button in playerCraftingButtons)
            {
                
                button.Draw(spriteBatch);
                if (!button.IsAvailable)
                {
                    spriteBatch.Draw(redX, button.Rectangle, Color.White * 0.4f);
                }

            }
            if (craftingMenu)
                spriteBatch.Draw(sprite, new Vector2(400, 400), Color.White);

        }
    }

    public abstract class Recipe
    {
        public string Name { get; set; }
        public bool[] Available { get; set; }
        public bool AllAvailable { get; set; }
        public List<Item> Ingredients { get; set; }
        public Button RecipeButton { get; set; }
        public abstract void Craft(object sender, EventArgs e);
        internal abstract EventHandler Craft();

        public Recipe()
        {

        }
    }

    public class FermentedBreatMilkRecipe : Recipe
    {

        public FermentedBreatMilkRecipe()
        {
            Name = "fermented breast milk";
            Ingredients = new List<Item>();
            Ingredients.Add(new Stone(1));
            Available = new bool[1];
            AllAvailable = false;
            RecipeButton = new Button(Name);
        }
        public override void Craft(object sender, EventArgs e)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
            inv.RemoveItem(Ingredients[0], Ingredients[0].Quantity);
            inv.AddItem(new Fish(1));
        }

        internal override EventHandler Craft()
        {
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

            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
            inv.RemoveItem(Ingredients[0], Ingredients[0].Quantity);
            inv.RemoveItem(Ingredients[1], Ingredients[1].Quantity);
            
            inv.AddItem(new Wood(4));
        }

        internal override EventHandler Craft()
        {
            return Craft;   
        }

    }


}
