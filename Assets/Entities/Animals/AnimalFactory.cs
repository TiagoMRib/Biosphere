using UnityEngine;

public static class AnimalFactory
{
    // Instantiate animal from traits
    // Reference to the base Animal prefab (should be set from a manager or inspector)
    public static GameObject BaseAnimalPrefab;

    public static Animal CreateAnimal(AnimalTraits traits, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (traits == null || traits.visualPrefab == null || BaseAnimalPrefab == null)
        {
            Debug.LogError("Traits, visualPrefab, or BaseAnimalPrefab missing!");
            Debug.Log("BaseAnimalPrefab: " + (BaseAnimalPrefab == null ? "null" : "set"));
            Debug.Log("Traits: " + (traits == null ? "null" : "set"));
            Debug.Log("VisualPrefab: " + (traits.visualPrefab == null ? "null" : "set"));
            return null;
        }

        // Instantiate the base animal prefab
        GameObject go = Object.Instantiate(BaseAnimalPrefab, position, rotation, parent);
        Animal animal = go.GetComponent<Animal>();
        if (animal == null)
        {
            Debug.LogError("Base prefab does not have an Animal component!");
            Object.Destroy(go);
            return null;
        }
        animal.Initialize(traits);
        return animal;
    }

    // Instantiate animal from two parents (reproduction)
    public static Animal CreateOffspring(AnimalTraits parentA, AnimalTraits parentB, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        AnimalTraits childTraits = GeneticCombiner.CombineTraits(parentA, parentB);
        return CreateAnimal(childTraits, position, rotation, parent);
    }

    // Instantiate animal from a single parent (assexual reproduction)
    public static Animal CreateClone(AnimalTraits parentTraits, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        // Optionally mutate traits for clone
        AnimalTraits cloneTraits = GeneticCombiner.CombineTraits(parentTraits, parentTraits);
        return CreateAnimal(cloneTraits, position, rotation, parent);
    }
}
