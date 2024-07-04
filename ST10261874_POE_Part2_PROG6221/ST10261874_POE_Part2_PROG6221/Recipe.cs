using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10261874_POE_Part2_PROG6221
{
    // Class representing a Recipe
    public class Recipe
    {
        // Delegate to handle the event when recipe calories exceed a certain limit
        public delegate void RecipeCaloriesExceededHandler(string recipeName, int totalCalories);

        // Event triggered when the total calories of a recipe exceed a predefined limit
        public event RecipeCaloriesExceededHandler RecipeCaloriesExceeded;

        // Name of the recipe
        public string Name { get; set; }

        // List of ingredients in the recipe
        public List<Ingredients> Ingredients { get; set; }

        // List of steps to prepare the recipe
        public List<Steps> Steps { get; set; }

        // List of original ingredients (used for resetting quantities)
        private List<Ingredients> OriginalIngredients { get; set; }

        // Constructor to initialize the recipe with empty lists
        public Recipe()
        {
            Ingredients = new List<Ingredients>();
            Steps = new List<Steps>();
            OriginalIngredients = new List<Ingredients>();
        }

        // Method to calculate the total calories of the recipe
        public int CalculateTotalCalories()
        {
            // Sum up the calories of all ingredients
            return Ingredients.Sum(ingredient => ingredient.Calories);
        }

        // Method to trigger the RecipeCaloriesExceeded event
        public void OnRecipeCaloriesExceeded(string recipeName, int totalCalories)
        {
            // Invoke the event if there are any subscribers
            RecipeCaloriesExceeded?.Invoke(recipeName, totalCalories);
        }

        // Method to save the original quantities of ingredients
        public void SaveOriginalQuantities()
        {
            // Create a deep copy of the ingredients list and store it
            OriginalIngredients = Ingredients.Select(i => new Ingredients
            {
                Name = i.Name,
                Quantity = i.Quantity,
                Unit = i.Unit,
                Calories = i.Calories,
                FoodGroup = i.FoodGroup
            }).ToList();
        }

        // Method to reset the quantities of ingredients to their original values
        public void ResetQuantities()
        {
            // Restore the ingredients list to the original quantities
            Ingredients = OriginalIngredients.Select(i => new Ingredients
            {
                Name = i.Name,
                Quantity = i.Quantity,
                Unit = i.Unit,
                Calories = i.Calories,
                FoodGroup = i.FoodGroup
            }).ToList();
        }
    }
}