using UnityEngine;
using System.Collections.Generic;

public class FoodSourceSpawner : MonoBehaviour
{
	[System.Serializable]
	public class FoodSpawnInfo
	{
		public GameObject foodPrefab;
		public int count = 5;
		public Vector3 spawnAreaCenter;
		public Vector3 spawnAreaSize = new Vector3(10, 0, 10);
	}

	public List<FoodSpawnInfo> foodSourcesToSpawn;

	private void Start()
	{
		foreach (var info in foodSourcesToSpawn)
		{
			for (int i = 0; i < info.count; i++)
			{
				Vector3 randomPos = info.spawnAreaCenter + new Vector3(
					Random.Range(-info.spawnAreaSize.x/2, info.spawnAreaSize.x/2),
					0,
					Random.Range(-info.spawnAreaSize.z/2, info.spawnAreaSize.z/2)
				);

				GameObject food = Instantiate(info.foodPrefab, randomPos, Quaternion.identity, this.transform);
				if (food != null)
				{
					food.name = info.foodPrefab.name;
				}
			}
		}
	}
}
