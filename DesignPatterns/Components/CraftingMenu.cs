using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EksamensProjekt2022
{
    public class CraftingMenu : Component
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

        public CraftingMenu()
        {
            sprite = GameWorld.Instance.Content.Load<Texture2D>("MinerTest");
            redX = GameWorld.Instance.Content.Load<Texture2D>("redX");
        }
        public override void Update(GameTime gameTime)
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
                    displayedCrafting = false;
                    craftingMenu = false;
                    firstRecipeSlot = new Vector2(50, 200);
                }


            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
                spaceReleased = true;
        }

        public void AddRecipe(Recipe recipe)
        {
            Recipes.Add(recipe);
        }
        public void CheckCrafting()
        {
            //if (!addedRecipes)
            //{
            //    Recipes.Add(new ChestRecipe());
            //    Recipes.Add(new FermentedBreatMilkRecipe());
            //}
            //addedRecipes = true;
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


        public override void Draw(SpriteBatch spriteBatch)
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

}



