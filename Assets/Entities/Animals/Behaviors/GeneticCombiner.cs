using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GeneticCombiner
{
    public static AnimalTraits CombineTraits(AnimalTraits parentA, AnimalTraits parentB)
    {
        AnimalTraits child = ScriptableObject.CreateInstance<AnimalTraits>();

        child.speciesTag = parentA.speciesTag; // Assuming same species for now
        child.visualPrefab = parentA.visualPrefab; // For simplicity
        child.carcassPrefab = parentA.carcassPrefab;

        // Numeric Traits
        child.maxHealth = AverageWithMutation(parentA.maxHealth, parentB.maxHealth, 0.1f);
        child.baseStrength = AverageWithMutation(parentA.baseStrength, parentB.baseStrength, 0.1f);
        child.baseSpeed = AverageWithMutation(parentA.baseSpeed, parentB.baseSpeed, 0.1f);
        child.baseHungerRate = InheritHungerRate(child.baseStrength, child.baseSpeed);
        child.hungerRate = child.baseHungerRate;
        child.eatRate = AverageWithMutation(parentA.eatRate, parentB.eatRate, 0.1f);
        child.searchRadius = AverageWithMutation(parentA.searchRadius, parentB.searchRadius, 0.1f);
        child.comfortableHealthThreshold = Random.Range(0.75f, 0.95f);

        // Behavior
        child.boldness = AverageWithMutation(parentA.boldness, parentB.boldness, 0.1f);

        // Reproduction traits (from either parent)
        child.fertilityCooldown = AverageWithMutation(parentA.fertilityCooldown, parentB.fertilityCooldown, 0.1f);
        child.fertileDuration = AverageWithMutation(parentA.fertileDuration, parentB.fertileDuration, 0.1f);
        child.gestationDuration = AverageWithMutation(parentA.gestationDuration, parentB.gestationDuration, 0.1f);
        child.gestationStrengthPenalty = AverageWithMutation(parentA.gestationStrengthPenalty, parentB.gestationStrengthPenalty, 0.05f);
        child.gestationSpeedPenalty = AverageWithMutation(parentA.gestationSpeedPenalty, parentB.gestationSpeedPenalty, 0.05f);
        child.gestationHungerMultiplier = AverageWithMutation(parentA.gestationHungerMultiplier, parentB.gestationHungerMultiplier, 0.1f);

        // Sex inheritance with rare mutation
        bool parentAIsSexual = parentA.sex == Sex.Male || parentA.sex == Sex.Female;
        bool parentBIsSexual = parentB.sex == Sex.Male || parentB.sex == Sex.Female;
        bool bothSexual = parentAIsSexual && parentBIsSexual;
        bool bothAssexual = parentA.sex == Sex.Assexual && parentB.sex == Sex.Assexual;

        float mutationChance = 0.01f; // 1% chance for rare mutation
        if (bothSexual)
        {
            if (Random.value < mutationChance)
                child.sex = Sex.Assexual;
            else
                child.sex = Random.value < 0.5f ? Sex.Male : Sex.Female;
        }
        else if (bothAssexual)
        {
            if (Random.value < mutationChance)
                child.sex = Random.value < 0.5f ? Sex.Male : Sex.Female;
            else
                child.sex = Sex.Assexual;
        }
        else
        {
            // Mixed parents: random, but favor sexual
            child.sex = Random.value < 0.8f ? (Random.value < 0.5f ? Sex.Male : Sex.Female) : Sex.Assexual;
        }

        // Diet Tags
        child.dietTags = CombineDiets(parentA.dietTags, parentB.dietTags);

        return child;
    }

    private static float AverageWithMutation(float a, float b, float mutationRate)
    {
        float average = (a + b) / 2f;
        float mutation = Random.Range(-mutationRate, mutationRate) * average;
        return Mathf.Max(0.01f, average + mutation);
    }

    private static float InheritHungerRate(float strength, float speed)
    {
        float baseRate = 0.5f;
        float modifier = (strength + speed - 1f) * AnimalGeneticConstants.MutationChances.HungerBoost;
        return baseRate + modifier;
    }

    private static List<string> CombineDiets(List<string> dietA, List<string> dietB)
    {
        HashSet<string> combined = new HashSet<string>();

        if (Random.value < AnimalGeneticConstants.MutationChances.CombineDietsChance)
        {
            foreach (var diet in dietA) combined.Add(diet);
            foreach (var diet in dietB) combined.Add(diet);
        }
        else
        {
            combined = new HashSet<string>(Random.value < 0.5f ? dietA : dietB);
        }

        // Diet Mutation: add a nearby tag (from same family)
        if (Random.value < AnimalGeneticConstants.MutationChances.DietMutation)
        {
            string baseTag = combined.ElementAt(Random.Range(0, combined.Count));
            string familyKey = GetFamilyKey(baseTag);

            if (familyKey != null)
            {
                string[] family = AnimalGeneticConstants.DietFamilies[familyKey];
                string candidate = family[Random.Range(0, family.Length)];
                combined.Add(candidate);
            }
        }

        return combined.ToList();
    }

    private static string GetFamilyKey(string tag)
    {
        foreach (var pair in AnimalGeneticConstants.DietFamilies)
        {
            if (pair.Value.Contains(tag))
                return pair.Key;
        }
        return null;
    }
}
