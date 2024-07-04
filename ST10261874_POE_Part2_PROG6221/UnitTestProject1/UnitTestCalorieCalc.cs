using Microsoft.VisualStudio.TestTools.UnitTesting;
using ST10261874_POE_Part2_PROG6221;
using System;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTestCalorieCalc
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Arrange - creating test cases
            var recipe1 = new Recipe();
            var recipe2 = new Recipe();
            var recipe3 = new Recipe();

            // Test case 1: Normal scenario with positive calories
            recipe1.Ingredients.Add(new Ingredients { Calories = 100 });
            recipe1.Ingredients.Add(new Ingredients { Calories = 200 });
            recipe1.Ingredients.Add(new Ingredients { Calories = 150 });

            // Test case 2: Scenario with no ingredients
            // No ingredients added to recipe2

            // Test case 3: Scenario with negative calorie values
            recipe3.Ingredients.Add(new Ingredients { Calories = -50 });
            recipe3.Ingredients.Add(new Ingredients { Calories = 100 });

            // Act - calculating total calories
            int totalCalories1 = recipe1.CalculateTotalCalories();
            int totalCalories2 = recipe2.CalculateTotalCalories();
            int totalCalories3 = recipe3.CalculateTotalCalories();

            // Assert - verifying the results
            Assert.AreEqual(450, totalCalories1, "Total calories should be the sum of ingredient calories (normal scenario).");
            Assert.AreEqual(0, totalCalories2, "Total calories should be 0 when there are no ingredients.");
            Assert.AreEqual(50, totalCalories3, "Total calories should correctly sum negative and positive values.");
        }
    }
}