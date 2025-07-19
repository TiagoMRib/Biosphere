using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFoodSource", menuName = "Environment/Food Source")]
public class FoodSourceData : ScriptableObject
{
    public string foodTag;            // E.g., "Grass", "Bug"

    public GameObject visualPrefab;   // Visual representation of the food source
    public float maxAbundance = 100f;
    public float regenRate = 5f;      // Abundance regenerated per second
    public float multiplicationChance = 0.1f; // Chance to spawn a neighbor
    public float neighborSpawnRadius = 5f;    // How close new patches can be
}

