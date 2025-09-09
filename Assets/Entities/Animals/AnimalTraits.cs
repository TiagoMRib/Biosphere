using UnityEngine;
using System.Collections.Generic;
using static AnimalGeneticConstants;

[CreateAssetMenu(fileName = "NewAnimalTraits", menuName = "Animal/Traits")]


public class AnimalTraits : ScriptableObject
{
    public string speciesTag; // Species name or identifier
    public GameObject visualPrefab; // Prefab for the animal's visual appearance
    public GameObject carcassPrefab; // Prefab for the animal's carcass after death

    // Feeding
    public float maxHealth = 100f; // Maximum health
    public float baseHungerRate; // Base rate at which health decreases due to hunger
    [HideInInspector] public float hungerRate = 1f; 
    public float eatRate = 10f; // Health gained per second of eating
    public List<string> dietTags; // List of diet types this animal can eat

    // Physical Attributes
    public float baseStrength; // Base strength (affects fighting and hunting)
    public float baseSpeed; // Base speed (affects movement)

    [HideInInspector] public float speed = 3f; 
    [HideInInspector] public float strength = 0.5f; 
    public float searchRadius = 8f; // How far the animal can search for food or mates
    [Range(0f, 1f)]
    public float comfortableHealthThreshold = 0.8f; // Health % above which animal is not hungry

    // Behavior
    [Range(0f, 1f)]
    public float boldness = 0.5f; // How much risk the animal takes (higher = more aggressive)

    // Reproduction
    public Sex sex; // Male, Female, or Assexual
    public float fertilityCooldown = 50f; // Time between fertility cycles
    public float fertileDuration = 10f; // Duration animal is fertile
    public float gestationDuration = 20f; // Pregnancy duration (for females)
    public float gestationStrengthPenalty = 0.3f; // Strength penalty during gestation
    public float gestationSpeedPenalty = 0.3f; // Speed penalty during gestation
    public float gestationHungerMultiplier = 1.5f; // Hunger rate multiplier during gestation


    // On Validate
    private void OnValidate()
    {
        strength = baseStrength;
        speed = baseSpeed;
        hungerRate = baseHungerRate;
    }
}

