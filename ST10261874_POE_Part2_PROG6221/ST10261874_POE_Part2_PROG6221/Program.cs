using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ST10261874_POE_Part2_PROG6221
{
    // Delegate for validating recipe names
    public delegate bool RecipeNameValidationDelegate(string recipeName);

    // Delegate for logging messages
    public delegate void LogDelegate(string message);

    // Delegate for handling errors
    public delegate void ErrorHandlerDelegate(Exception ex);

    public class Program
    {
        static List<Recipe> recipes = new List<Recipe>(); // List to store recipes
        static List<string> logs = new List<string>(); // List to store logs

        static void Main(string[] args)
        {
            // Set console background color to dark grey
            Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();

            // Display a loading message with a countdown
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Loading........");
            for (int i = 5; i > 0; i--)
            {
                Console.Write(i + "\t");
                Thread.Sleep(1000); // Sleep for 1 second
            }
            Console.Clear(); // Clear the loading screen

            // Define the ASCII art for the welcome message
            string welcomeMessage = @"
     
 __    __     _                            _                                          _                                
/ / /\ \ \___| | ___ ___  _ __ ___   ___  | |_ ___    _ __ ___  _   _   _ __ ___  ___(_)_ __   ___    __ _ _ __  _ __  
\ \/  \/ / _ \ |/ __/ _ \| '_ ` _ \ / _ \ | __/ _ \  | '_ ` _ \| | | | | '__/ _ \/ __| | '_ \ / _ \  / _` | '_ \| '_ \ 
 \  /\  /  __/ | (_| (_) | | | | | |  __/ | || (_) | | | | | | | |_| | | | |  __/ (__| | |_) |  __/ | (_| | |_) | |_) |
  \/  \/ \___|_|\___\___/|_| |_| |_|\___|  \__\___/  |_| |_| |_|\__, | |_|  \___|\___|_| .__/ \___|  \__,_| .__/| .__/ 
                                                                |___/                  |_|                |_|   |_|    

┌────────────────────────────────────────────────────┐
│░█▄█░█▀█░█▀▄░█▀▀░░░█▀▄░█░█░░░█▀▄░▀█▀░█▀█░█▀█░█▀▀░█▀█│
│░█░█░█▀█░█░█░█▀▀░░░█▀▄░░█░░░░█░█░░█░░█▀█░█░█░█░░░█▀█│
│░▀░▀░▀░▀░▀▀░░▀▀▀░░░▀▀░░░▀░░░░▀▀░░▀▀▀░▀░▀░▀░▀░▀▀▀░▀░▀│
└────────────────────────────────────────────────────┘
";
            // Display the ASCII art
            Console.WriteLine(welcomeMessage);
            Thread.Sleep(2000); // Wait for 2 seconds

            bool exit = false;

            while (!exit)
            {
                // Display menu options
                Console.ForegroundColor = ConsoleColor.Cyan; // Cyan menu options
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1 - Enter Recipe Details");
                Console.WriteLine("2 - Display Recipe List");
                Console.WriteLine("3 - Choose Recipe to Display");
                Console.WriteLine("4 - Scale Recipe");
                Console.WriteLine("5 - Reset scaled Recipe");
                Console.WriteLine("6 - Clear Recipes");
                Console.WriteLine("7 - View Logs");
                Console.WriteLine("8 - Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();
                Console.Clear(); // Clear the screen before displaying the next menu

                switch (choice)
                {
                    case "1":
                        EnterRecipeDetails(Log, ValidateRecipeName);
                        break;
                    case "2":
                        DisplayRecipeList();
                        break;
                    case "3":
                        ChooseRecipe();
                        break;
                    case "4":
                        ScaleRecipe();
                        break;
                    case "5":
                        ResetRecipe();
                        break;
                    case "6":
                        ClearRecipes();
                        break;
                    case "7":
                        ViewLogs(Log);
                        break;
                    case "8":
                        exit = true;
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red; // Red error message
                        Console.WriteLine("Invalid choice! Please try again.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        // Method to enter recipe details
        static void EnterRecipeDetails(LogDelegate log, RecipeNameValidationDelegate validateRecipeName)
        {
            try
            {
             
                Console.ForegroundColor = ConsoleColor.Blue; // Blue text for entering recipe details
                Console.WriteLine("Enter Recipe Details:\n");
                Recipe recipe = new Recipe();
                recipe.RecipeCaloriesExceeded += HandleRecipeCaloriesExceeded;

                // Get recipe name from user
                Console.Write("Enter Recipe Name: ");
                string recipeName = Console.ReadLine();

                // Validate recipe name
                if (!IsRecipeNameValid(recipeName))
                {
                    Console.ForegroundColor = ConsoleColor.Red; // Red error message
                    Console.WriteLine("Invalid recipe name! Recipe names cannot contain special symbols or numbers.");
                    Console.ResetColor();

                    return;
                }

                recipe.Name = recipeName;

                // Get and validate ingredients
                Console.WriteLine("\nEnter Ingredients:");
                Console.WriteLine("-----------------");
                Console.Write("Enter Number of Ingredients: ");
                int numIngredients = int.Parse(Console.ReadLine());

                for (int i = 0; i < numIngredients; i++)
                {
                    Ingredients ingredient = new Ingredients();

                    Console.WriteLine($"\nIngredient {i + 1}:");
                    Console.Write("Name: ");
                    string name = Console.ReadLine();

                    // Validate ingredient name
                    if (!IsIngredientNameValid(name))
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Red error message
                        Console.WriteLine("Invalid ingredient name! Ingredient names cannot contain special symbols.");
                        Console.ResetColor();

                        // Log the invalid ingredient name
                        Log($"Invalid ingredient name entered: {name}");

                        return;
                    }

                    ingredient.Name = name;

                    Console.Write("Quantity: ");
                    ingredient.Quantity = double.Parse(Console.ReadLine());

                    // Select unit
                    Console.WriteLine("Select Unit:");
                    Console.WriteLine("1 - Cup");
                    Console.WriteLine("2 - Liter");
                    Console.WriteLine("3 - Milliliter");
                    Console.WriteLine("4 - Teaspoon");
                    Console.WriteLine("5 - Tablespoon");
                    Console.Write("Enter Unit Selection: ");
                    int unitChoice = int.Parse(Console.ReadLine());

                    switch (unitChoice)
                    {
                        case 1:
                            ingredient.Unit = "Cup";
                            break;
                        case 2:
                            ingredient.Unit = "Liter";
                            break;
                        case 3:
                            ingredient.Unit = "Milliliter";
                            break;
                        case 4:
                            ingredient.Unit = "Teaspoon";
                            break;
                        case 5:
                            ingredient.Unit = "Tablespoon";
                            break;
                        default:
                            Console.ForegroundColor = ConsoleColor.Red; // Red error message
                            Console.WriteLine("Invalid unit selection! Please try again.");
                            Console.ResetColor();
                            return;
                    }

                    Console.Write("Calories: ");
                    ingredient.Calories = int.Parse(Console.ReadLine());

                    Console.Write("Food Group: ");
                    ingredient.FoodGroup = Console.ReadLine();

                    recipe.Ingredients.Add(ingredient);
                }

                recipe.SaveOriginalQuantities(); // Save original quantities

                Console.WriteLine("\nEnter Steps:");
                Console.WriteLine("------------");
                Console.Write("Enter Number of Steps: ");
                int numSteps = int.Parse(Console.ReadLine());

                for (int i = 0; i < numSteps; i++)
                {
                    Steps step = new Steps();

                    Console.WriteLine($"\nStep {i + 1}:");
                    Console.Write("Description: ");
                    step.Description = Console.ReadLine();

                    recipe.Steps.Add(step);
                }

                recipes.Add(recipe);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("\nRecipe added successfully!\n");
                Console.ResetColor();

                // Log the successful addition of the recipe
                Log($"Recipe '{recipeName}' added successfully.");
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                HandleError(ex, log);
            }
        }
        // Method to view logs
        static void ViewLogs(LogDelegate log)
        {
            log("User viewed logs.");

            Console.ForegroundColor = ConsoleColor.Magenta; // Magenta text for logs
            Console.WriteLine("Logs:");
            Console.WriteLine("-----");

            // Display all logs with timestamps
            foreach (string logEntry in logs)
            {
                Console.WriteLine(logEntry);
            }

            if (logs.Count == 0)
            {
                Console.WriteLine("No logs available.");
            }
        }
        //Method to display list of recipes
        static void DisplayRecipeList()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed; // Pink text for display recipe list
            Console.WriteLine("Recipe List:");
            Console.WriteLine("------------");

            // Display recipe names in alphabetical order
            foreach (Recipe recipe in recipes.OrderBy(r => r.Name))
            {
                Console.WriteLine(recipe.Name);
                Console.WriteLine();
            }
        }

        // Method to choose and display a specific recipe
        static void ChooseRecipe()
        {
            Console.ForegroundColor = ConsoleColor.Yellow; // Yellow text for choose recipe
            Console.WriteLine("Choose a Recipe to Display:");
            Console.WriteLine("---------------------------");
            DisplayRecipeList();

            // Get the recipe name from user
            Console.Write("Enter Recipe Name: ");
            string recipeName = Console.ReadLine();

            // Find the recipe and display its details
            Recipe recipe = recipes.FirstOrDefault(r => r.Name == recipeName);
       
            if (recipe != null)
            {
                DisplayRecipeDetails(recipe);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; // Red error message
                Console.WriteLine("Recipe not found!");
                Console.ResetColor();
            }
        }

        // Method to display details of a recipe
        static void DisplayRecipeDetails(Recipe recipe)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow; // Dark yellow text for display recipe details
            Console.WriteLine($"\nRecipe: {recipe.Name}");
            Console.WriteLine("Ingredients:");
            Console.WriteLine("-------------");

            // Display each ingredient's details
            foreach (Ingredients ingredient in recipe.Ingredients)
            {
                Console.WriteLine($"- {ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}");
                Console.WriteLine($"  Calories: {ingredient.Calories}, Food Group: {ingredient.FoodGroup}");
            }

            // Calculate and display total calories
            int totalCalories = recipe.CalculateTotalCalories();
            Console.WriteLine($"\nTotal Calories: {totalCalories}");

            Console.WriteLine("\nSteps:");
            Console.WriteLine("-------");
            for (int i = 0; i < recipe.Steps.Count; i++)
            {
                Console.WriteLine($"Step {i + 1}: {recipe.Steps[i].Description}");
            }

            // Display warning if total calories exceed 300
            if (totalCalories > 300)
            {
                Console.ForegroundColor = ConsoleColor.Red; // Red warning message
                Console.WriteLine("\nWARNING: Total calories exceed 300!\n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green; // Green success message
                Console.WriteLine("\nTotal calories within limit.\n");
                Console.ResetColor();
            }
        }

        //Method to scale ingredients
        static void ScaleRecipe()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Choose a Recipe to Scale:");
            Console.WriteLine("-------------------------");
            DisplayRecipeList();

            Console.Write("Enter Recipe Name: ");
            string recipeName = Console.ReadLine();

            Recipe recipe = recipes.FirstOrDefault(r => r.Name == recipeName);
            if (recipe != null)
            {
                Console.Write("Enter Scale Factor (e.g., 0.5 to halve, 2 to double, or 3 to triple): ");
                double scaleFactor = double.Parse(Console.ReadLine());

                ScaleRecipeIngredients(recipe, scaleFactor);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("\nRecipe scaled successfully!\n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Recipe not found!");
                Console.ResetColor();
            }
        }

        static void ScaleRecipeIngredients(Recipe recipe, double scaleFactor)
        {
            foreach (Ingredients ingredient in recipe.Ingredients)
            {
                ingredient.Quantity *= scaleFactor;
                ingredient.Calories = (int)Math.Round(ingredient.Calories * scaleFactor);
            }
        }

        static void ResetRecipe()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Choose a Recipe to Reset:");
            Console.WriteLine("-------------------------");
            DisplayRecipeList();

            Console.Write("Enter Recipe Name: ");
            string recipeName = Console.ReadLine();

            Recipe recipe = recipes.FirstOrDefault(r => r.Name == recipeName);
            if (recipe != null)
            {
                recipe.ResetQuantities();
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Green;
                Console.WriteLine("\nRecipe reset successfully!\n");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Recipe not found!");
                Console.ResetColor();
            }
        }

        // Method to clear recipes (all or selected)
        static void ClearRecipes()
        {
            Console.ForegroundColor = ConsoleColor.Magenta; // Magenta text for clear recipes option
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1 - Clear All Recipes");
            Console.WriteLine("2 - Clear a Selected Recipe");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();
            Console.Clear(); // Clear the screen before displaying the next menu

            switch (choice)
            {
                case "1":
                    // Clear all recipes
                    recipes.Clear();
                    Console.ForegroundColor = ConsoleColor.White; // White text for success message
                    Console.BackgroundColor = ConsoleColor.Green; // Green background for success message
                    Console.WriteLine("All recipes have been cleared successfully!\n");
                    Console.ResetColor();
                    break;
                case "2":
                    // Clear a selected recipe
                    Console.ForegroundColor = ConsoleColor.Yellow; // Yellow text for choose recipe to clear
                    Console.WriteLine("Choose a Recipe to Clear:");
                    Console.WriteLine("-------------------------");
                    DisplayRecipeList();

                    Console.Write("Enter Recipe Name: ");
                    string recipeName = Console.ReadLine();
                    Recipe recipeToRemove = recipes.FirstOrDefault(r => r.Name == recipeName);
                    if (recipeToRemove != null)
                    {
                        recipes.Remove(recipeToRemove);
                        Console.ForegroundColor = ConsoleColor.White; // White text for success message
                        Console.BackgroundColor = ConsoleColor.Green; // Green background for success message
                        Console.WriteLine($"\nRecipe '{recipeName}' has been cleared successfully!\n");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Red error message
                        Console.WriteLine("Recipe not found!");
                        Console.ResetColor();
                    }
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red; // Red error message
                    Console.WriteLine("Invalid choice! Please try again.");
                    Console.ResetColor();
                    break;
            }
        }
        // Method to handle the event
        static void HandleRecipeCaloriesExceeded(string recipeName, int totalCalories)
        {
            Console.ForegroundColor = ConsoleColor.Red; // Red warning message
            Console.WriteLine($"\nWARNING: The recipe '{recipeName}' has {totalCalories} total calories, exceeding 300!\n");
            Console.ResetColor();
        }

        // Method to validate recipe name
        static bool IsRecipeNameValid(string recipeName)
        {
             return !string.IsNullOrWhiteSpace(recipeName) && recipeName.All(char.IsLetterOrDigit);
           // return !string.IsNullOrWhiteSpace(recipeName);
        }

        // Method to validate ingredient name
        static bool IsIngredientNameValid(string ingredientName)
        {
            
             return !string.IsNullOrWhiteSpace(ingredientName) && ingredientName.All(char.IsLetter);
            //return !string.IsNullOrWhiteSpace(ingredientName);
        }

        // Method to log messages
        // Method to log messages
        static void Log(string message)
        {
            string logEntry = $"{DateTime.Now}: {message}";
            logs.Add(logEntry); // Add log entry to the list
            Console.WriteLine($"Log: {message}");
        }

        // Method to handle errors
        static void HandleError(Exception ex, LogDelegate log)
        {
            string errorMessage = $"Error: {ex.Message}";
            Console.WriteLine(errorMessage);
            log(errorMessage);
        }
        // Method to validate recipe name
        static bool ValidateRecipeName(string recipeName)
        {
            return !string.IsNullOrWhiteSpace(recipeName) && recipeName.All(char.IsLetterOrDigit);
        }
    }
}