using UnityEngine;
using System.Collections.Generic;

public class AnimalSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpeciesSpawnInfo
    {
        public AnimalTraits baseMaleTraits;
        public AnimalTraits baseFemaleTraits;
        public int count = 5;
        public Vector3 spawnAreaCenter;
        public Vector3 spawnAreaSize = new Vector3(10, 0, 10);
    }

    public List<SpeciesSpawnInfo> speciesToSpawn;

    private void Start()
    {
        foreach (var info in speciesToSpawn)
        {
            for (int i = 0; i < info.count; i++)
            {
                // Randomly choose male or female traits
                AnimalTraits parentA = Random.value < 0.5f ? info.baseMaleTraits : info.baseFemaleTraits;
                AnimalTraits parentB = Random.value < 0.5f ? info.baseMaleTraits : info.baseFemaleTraits;
                AnimalTraits childTraits = GeneticCombiner.CombineTraits(parentA, parentB);

                // Random position in area
                Vector3 randomPos = info.spawnAreaCenter + new Vector3(
                    Random.Range(-info.spawnAreaSize.x/2, info.spawnAreaSize.x/2),
                    0,
                    Random.Range(-info.spawnAreaSize.z/2, info.spawnAreaSize.z/2)
                );

                AnimalFactory.CreateAnimal(childTraits, randomPos, Quaternion.identity, this.transform);
            }
        }
    }
}
