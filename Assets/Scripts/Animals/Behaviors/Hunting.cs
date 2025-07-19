using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingBehavior : MonoBehaviour
{
    private Animal animal;
    private Animal targetPrey;
    private float attackRange = 1.5f;

    public void Initialize(Animal animalRef)
    {
        animal = animalRef;
    }

    public void Hunt(Animal prey)
    {
        targetPrey = prey;
        Debug.Log($"{animal.name} is now hunting {prey.name}.");
    }

    public void StopHunt()
    {
        targetPrey = null;
    }

    public bool IsHunting()
    {
        return targetPrey != null;
    }

    public void UpdateHunt()
    {
        if (targetPrey == null) return;

        animal.MoveTowards(targetPrey.transform.position);

        if (Vector3.Distance(animal.transform.position, targetPrey.transform.position) < attackRange)
        {
            StartCoroutine(FightRoutine(animal, targetPrey));
            StopHunt();
        }
    }

    private IEnumerator FightRoutine(Animal predator, Animal prey)
    {
        Debug.Log($"{predator.name} is fighting {prey.name}...");

        predator.FreezeMovement();
        prey.FreezeMovement();

        float predatorStrength = predator.traits.strength;
        float preyStrength = prey.traits.strength;

        float baseTime = 0.5f;
        float maxExtraTime = 2f;

        // Closer match -> longer fight
        float strengthRatio = Mathf.Min(predatorStrength, preyStrength) / Mathf.Max(predatorStrength, preyStrength);
        float fightDuration = baseTime + (1f - strengthRatio) * maxExtraTime;

        Debug.Log($"Fight duration: {fightDuration:F2}s");
        yield return new WaitForSeconds(fightDuration);

        float predatorRoll = predatorStrength + Random.Range(0f, 2f);
        float preyRoll = preyStrength + Random.Range(0f, 2f);

        if (predatorRoll >= preyRoll)
        {
            Debug.Log($"{predator.name} killed {prey.name}!");

            if (prey.traits.carcassPrefab != null)
            {
                GameObject carcass = Instantiate(prey.traits.carcassPrefab, prey.transform.position, Quaternion.identity);
                CarcassFoodSource carcassSource = carcass.GetComponent<CarcassFoodSource>();

                if (carcassSource != null)
                {
                    float leftoverHealth = prey.currentHealth;
                    float bonus = prey.traits.maxHealth * 0.1f;
                    carcassSource.currentAbundance = leftoverHealth + bonus;
                }
            }
            else
            {
                Debug.LogWarning($"{prey.name} has no assigned carcass prefab!");
            }

            Object.Destroy(prey.gameObject);
        }
        else
        {
            Debug.Log($"{predator.name} failed to kill {prey.name}!");
            prey.UnfreezeMovement();
            predator.ApplyFailedHuntPenalty();
        }

        predator.UnfreezeMovement();
    }


}
