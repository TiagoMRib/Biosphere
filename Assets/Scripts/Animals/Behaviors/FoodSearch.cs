using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSearchBehavior : MonoBehaviour
{
    private Animal animal;

    public void Initialize(Animal animalRef)
    {
        animal = animalRef;
    }
    
    public FoodSource FindFood()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, animal.GetSearchRadius());

        foreach (var hit in hits)
        {
            Debug.Log($"Checking collider for food: {hit.name}");
            FoodSource food = hit.GetComponent<FoodSource>();
            Debug.Log($"Found food: {food?.name ?? "None"}");
            if (food != null && animal.GetDiet().Contains(food.GetTag()))
                return food;
        }

        return null;
    }

    public Animal FindPrey()
    {
        if (animal.GetDiet().Contains("Meat") == false)
        {
            return null;
        }

        Debug.Log($"{animal.name} is searching for prey...");
        
        Collider[] hits = Physics.OverlapSphere(transform.position, animal.GetSearchRadius());

        Animal bestCandidate = null;
        float bestScore = float.MinValue;

        foreach (var hit in hits)
        {
            Debug.Log($"Checking collider: {hit.name}");
            Animal other = hit.GetComponent<Animal>();

            if (other != null && other != animal)
            {

                // Don't hunt other predators or equals
                float preyStrength = other.traits.strength;
                float myStrength = animal.traits.strength;

                // Desperation based on hunger (0 if full, 1 if starving)
                float hungerFactor = 1f - (animal.currentHealth / animal.traits.maxHealth);

                // Boldness is how risky this animal behaves (0 = coward, 1 = reckless)
                float boldness = animal.traits.boldness;

                // Dynamic risk tolerance (higher if bold or desperate)
                float maxAcceptableStrength = myStrength * (1f - 0.3f + boldness * 0.6f + hungerFactor * 0.4f);

                if (preyStrength <= maxAcceptableStrength)
                {
                    // Prefer weaker prey that are easier to kill
                    float desirability = myStrength - preyStrength;

                    if (desirability > bestScore)
                    {
                        bestScore = desirability;
                        bestCandidate = other;
                    }
                }
            }
        }

        Debug.Log($"Found prey: {bestCandidate?.name ?? "None"} with score {bestScore}");
        return bestCandidate;
    }



}
