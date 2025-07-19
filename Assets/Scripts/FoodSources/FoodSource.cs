using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSource : MonoBehaviour
{
    public FoodSourceData data;
    public float currentAbundance;
    private GameObject visualInstance;


    // Control Variables
    private float multiplyTimer = 0f;

    protected virtual void Start()
    {
        currentAbundance = data.maxAbundance;
        SpawnVisual();
    }

    protected virtual void Update()
    {
        Regenerate();
        

        multiplyTimer += Time.deltaTime;
        if (multiplyTimer >= 10f)
        {
            TryMultiply();
            multiplyTimer = 0f;
        }
    }

    private void Regenerate()
    {
        if (currentAbundance < data.maxAbundance)
        {
            currentAbundance += data.regenRate * Time.deltaTime;
            currentAbundance = Mathf.Min(currentAbundance, data.maxAbundance);
        }
    }

    private void TryMultiply()
    {
        if (Random.value < data.multiplicationChance)
        {
            int attempts = 5;
            float checkRadius = 0.5f; // Minimum distance between sources

            for (int i = 0; i < attempts; i++)
            {
                Vector3 offset = Random.insideUnitSphere * data.neighborSpawnRadius;
                offset.y = 0f;
                Vector3 spawnPosition = transform.position + offset;

                // Check for existing food sources nearby
                Collider[] nearby = Physics.OverlapSphere(spawnPosition, checkRadius);
                bool tooClose = false;

                foreach (var col in nearby)
                {
                    if (col.GetComponent<FoodSource>() != null)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    Instantiate(gameObject, spawnPosition, Quaternion.identity);
                    break;
                }
            }
        }
    }


    public string GetTag()
    {
        return data.foodTag;
    }

    public float GetAbundancePercent()
    {
        return currentAbundance / data.maxAbundance;
    }

    private void SpawnVisual()
    {
        if (data.visualPrefab != null)
        {
            visualInstance = Instantiate(data.visualPrefab, transform);
            visualInstance.transform.localPosition = Vector3.zero;
            visualInstance.transform.localRotation = Quaternion.identity;
        }
    }

    public float Consume(float requestedAmount)
    {
        float amountAvailable = Mathf.Min(requestedAmount, currentAbundance);
        currentAbundance -= amountAvailable;

        if (currentAbundance <= 0f)
        {
            Destroy(gameObject);
        }

        return amountAvailable;
    }
}
