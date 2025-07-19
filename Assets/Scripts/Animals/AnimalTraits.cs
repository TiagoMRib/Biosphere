using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewAnimalTraits", menuName = "Animal/Traits")]
public class AnimalTraits : ScriptableObject
{
    public string speciesTag;
    public GameObject visualPrefab;
    public GameObject carcassPrefab;

    // Feeding
    public float maxHealth = 100f;
    public float hungerRate = 1f;
    public float eatRate = 10f; // Health gained per second of eating
    public List<string> dietTags;

    // Physical Attributes
    public float speed = 3f;
    public float strength = 0.5f;
    public float searchRadius = 8f;
    [Range(0f, 1f)]
    public float comfortableHealthThreshold = 0.8f;

    // Behavior
    [Range(0f, 1f)]
    public float boldness = 0.5f; // How much risk the animal takes
}
