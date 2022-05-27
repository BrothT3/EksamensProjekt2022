using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//namespace EksamensProjekt2022
////{
//    //public class lortePlayerCraftingMenu
//    {
//        public List<Button> playerCraftingButtons = new List<Button>();
//        public List<Recipe> Recipes = new List<Recipe>();
//        public bool addedRecipes = false;
//        public int defaultposX = 200;
//        public int defaultposY = 200;
//        public void Update(GameTime gameTime)
//        {
//            CheckCrafting();
//            foreach (Button button in playerCraftingButtons)
//            {

//                if (button.IsAvailable)
//                {
//                    button.Update(gameTime);
//                }

//            }
//        }

//        public void CheckCrafting()
//        {
//            if (!addedRecipes)
//            {
//                Recipes.Add(new Recipe("Chest", "Basic item-storage", new Wood(1), 3));
//                Button ChestButton = new Button("Chest");
//                ChestButton.OnClicking += CraftChest;
//                playerCraftingButtons.Add(ChestButton);
//            }
//            addedRecipes = true;
            
//            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
//            if (player != null)
//            {
//                Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;

//                foreach (Recipe recipe in Recipes)
//                {
//                    if (recipe.Amount >= inv.GetItemCount<Item>(recipe.Ingredient))
//                    {
//                        Button button = playerCraftingButtons.Find(x => (x.ButtonText == recipe.Name));
//                        button.IsAvailable = true;
//                    }
//                }
//            }


//        }

//        private void CraftChest(object sender, EventArgs e)
//        {
//            Player player = (Player)GameWorld.Instance.FindObjectOfType<Player>();
//            Inventory inv = player.GameObject.GetComponent<Inventory>() as Inventory;
//            Wood wood = new Wood(3);
//            inv.AddItem(wood);
//        }
//        public void Draw(SpriteBatch spriteBatch)
//        {
//            foreach (Button button in playerCraftingButtons)
//            {

//                if (button.IsAvailable)
//                {
//                    button.Rectangle = new Rectangle(defaultposX, defaultposY, 100, 100);
//                    button.Draw(spriteBatch);
//                }
//                defaultposY += 120;
//            }
//        }
//    }

//    public class Recipe
//    {
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public int Amount { get; set; }
//        public Item Ingredient { get; set; }
//        public bool Available { get; set; }
//        public Button RecipeButton { get; set; }

//        public Recipe(string name, string description, Item ingredient, int amount)
//        {
//            Name = name;
//            Description = description;
//            Ingredient = ingredient;
//            Amount = amount;
//            Available = false;
//        }
//    }

//    internal static class RecipeList
//    {

//        internal const int MAX_WOOD_QUANTITY = 10;
//        internal const int MAX_FISH_QUANTITY = 8;
//        internal const int MAX_VEGETABLE_QUANTITY = 10;
//    }

//}
