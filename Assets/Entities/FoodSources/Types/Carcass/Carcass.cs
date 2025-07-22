using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarcassFoodSource : FoodSource
{
    [Header("Bug Cloud Spawning")]
    public GameObject bugCloudPrefab;
    public float bugCloudSpawnInterval = 30f;
    public float bugCloudSpawnRadius = 1.5f;

    private float bugCloudTimer = 0f;

    protected override void Start()
    {
        // dont do nothing
    }

    protected override void Update()
    {
        base.Update(); // Let base handle decay (via negative regen)

        // Handle bug cloud spawning
        bugCloudTimer += Time.deltaTime;
        if (bugCloudTimer >= bugCloudSpawnInterval)
        {
            TrySpawnBugCloud();
            bugCloudTimer = 0f;
        }
    }

    private void TrySpawnBugCloud()
    {
        Debug.Log($"Attempting to spawn bug cloud for carcass {name}");
        if (bugCloudPrefab == null)
        {
            Debug.LogWarning("No BugCloud prefab assigned to carcass.");
            return;
        }

        const int attempts = 5;
        const float minDistance = 0.3f;

        for (int i = 0; i < attempts; i++)
        {
            Vector3 offset = Random.insideUnitSphere * bugCloudSpawnRadius;
            offset.y = 0f; // Keep on ground
            Vector3 spawnPos = transform.position + offset;

            Collider[] colliders = Physics.OverlapSphere(spawnPos, minDistance);
            bool blocked = false;
            foreach (var col in colliders)
            {
                if (col.GetComponent<FoodSource>() != null)
                {
                    blocked = true;
                    break;
                }
            }

            if (!blocked)
            {
                Debug.Log($"Spawning bug cloud");
                Instantiate(bugCloudPrefab, spawnPos, Quaternion.identity);
                break;
            }
        }
    }
}


