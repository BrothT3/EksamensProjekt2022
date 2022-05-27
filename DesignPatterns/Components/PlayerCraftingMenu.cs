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
        public List<Button> playerCraftingButtons = new List<Button>();
        public List<Recipe> Recipes = new List<Recipe>();
        public bool craftingMenu = false;
        private bool spaceReleased;
        private Vector2 firstRecipeSlot = new Vector2(10, 36);

        public void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && spaceReleased == true)
            {
                spaceReleased = false;
                craftingMenu = true;
            }
            spaceReleased = true;
            if (craftingMenu)
            {
                CheckCrafting();
                foreach (Button button in playerCraftingButtons)
                {  
                        button.Update(gameTime);
                }
            }

        }

        public void CheckCrafting()
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();

            Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
            if (!addedRecipes)
            {
                Recipes.Add(new ChestRecipe());
                
            }
            addedRecipes = true;
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
                {
                    recipe.RecipeButton.IsAvailable = true;
                }
                playerCraftingButtons.Add(recipe.RecipeButton);

                
            }
            
        }
        
        
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Button button in playerCraftingButtons)
            {
                button.Draw(spriteBatch);
            }
        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public bool[] Available { get; set; }
        public bool AllAvailable { get; set; }
        public List<Item> Ingredients { get; set; }
        public Button RecipeButton { get; set; }
        public Recipe()
        {

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
        private void CraftChest(object sender, EventArgs e)
        {
            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
            Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
            Wood wood = new Wood(3);
            inv.AddItem(wood);
        }
    }


}
