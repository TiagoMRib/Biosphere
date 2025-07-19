using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatBehavior : MonoBehaviour
{
    private Animal animal;
    private FoodSource currentFood;

    public void Initialize(Animal animalRef)
    {
        animal = animalRef;
    }

    public void StartEating(FoodSource food)
    {
        if (food == null) return;
        currentFood = food;
        Debug.Log($"{animal.name} started eating {food.name}");
    }

    public void StopEating()
    {
        if (currentFood != null)
            Debug.Log($"{animal.name} stopped eating {currentFood.name}");
        currentFood = null;
    }

    public void UpdateEating()
    {
        if (currentFood == null) return;

        float missingHealth = animal.traits.maxHealth - animal.currentHealth;
        if (missingHealth <= 0f)
        {
            StopEating(); // Already full
            return;
        }

        float amountToConsume = animal.traits.eatRate * Time.deltaTime;
        float healthGained = currentFood.Consume(amountToConsume); // defined in FoodSource
        animal.currentHealth += healthGained;
        animal.currentHealth = Mathf.Min(animal.traits.maxHealth, animal.currentHealth);

        if (currentFood == null) StopEating(); // If destroyed
    }

    public bool IsEating()
    {
        return currentFood != null;
    }

    public FoodSource GetCurrentFood()
    {
        return currentFood;
    }
}
