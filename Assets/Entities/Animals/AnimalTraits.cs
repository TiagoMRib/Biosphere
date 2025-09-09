using UnityEngine;
using System.Collections.Generic;
using static AnimalGeneticConstants;

[CreateAssetMenu(fileName = "NewAnimalTraits", menuName = "Animal/Traits")]


public class AnimalTraits : ScriptableObject
{
    public string speciesTag;
    public GameObject visualPrefab;
    public GameObject carcassPrefab;

    // Feeding
    public float maxHealth = 100f;
    public float baseHungerRate;
    [HideInInspector] public float hungerRate = 1f;
    public float eatRate = 10f; // Health gained per second of eating
    public List<string> dietTags;

    // Physical Attributes

    public float baseStrength;
    public float baseSpeed;

    [HideInInspector] public float speed = 3f;
    [HideInInspector] public float strength = 0.5f;
    public float searchRadius = 8f;
    [Range(0f, 1f)]
    public float comfortableHealthThreshold = 0.8f;

    // Behavior
    [Range(0f, 1f)]
    public float boldness = 0.5f; // How much risk the animal takes

    // Reproduction

    public Sex sex;
    public float fertilityCooldown = 50f;
    public float fertileDuration = 10f;
    public float gestationDuration = 20f;
    public float gestationStrengthPenalty = 0.3f;
    public float gestationSpeedPenalty = 0.3f;
    public float gestationHungerMultiplier = 1.5f;


    // On Validate
    private void OnValidate()
    {
        strength = baseStrength;
        speed = baseSpeed;
        hungerRate = baseHungerRate;
    }
}

