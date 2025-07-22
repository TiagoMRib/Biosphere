using System.Collections;
using UnityEngine;

public class MatingBehavior : MonoBehaviour
{
    private Animal animal;
    private float fertilityTimer = 0f;
    private float fertileTimeRemaining = 0f;
    private bool isGestating = false;
    private float gestationTimer = 0f;

    public void Initialize(Animal animalRef)
    {
        animal = animalRef;
    }

    public void UpdateReproduction()
    {
        switch (animal.traits.sex)
        {
            case Sex.Female:
                UpdateFemaleCycle();
                break;
            case Sex.Male:
                SearchForMate();
                break;
            case Sex.Assexual:
                UpdateAssexualCycle();
                break;
        }
    }

    private void UpdateFemaleCycle()
    {
        if (isGestating)
        {
            gestationTimer -= Time.deltaTime;
            if (gestationTimer <= 0f)
            {
                isGestating = false;
                ResetGestationPenalties();
                Debug.Log($"{animal.name} gave birth!");
                fertilityTimer = animal.traits.fertilityCooldown;
            }
        }
        else if (fertileTimeRemaining > 0f)
        {
            fertileTimeRemaining -= Time.deltaTime;
        }
        else
        {
            fertilityTimer -= Time.deltaTime;
            if (fertilityTimer <= 0f)
            {
                fertileTimeRemaining = animal.traits.fertileDuration;
                Debug.Log($"{animal.name} is now FERTILE");
            }
        }
    }

    private void SearchForMate()
    {
        // Males look for fertile females
        Collider[] hits = Physics.OverlapSphere(transform.position, animal.GetSearchRadius());

        foreach (var hit in hits)
        {
            Animal other = hit.GetComponent<Animal>();
            if (other == null || other == animal) continue;

            if (animal.GetSpeciesTag() == other.GetSpeciesTag() &&
                other.traits.sex == Sex.Female &&
                other.matingBehavior.IsFertile() &&
                Vector3.Distance(transform.position, other.transform.position) < 1.5f)
            {
                other.matingBehavior.BeginGestation();
                Debug.Log($"{animal.name} mated with {other.name}");
                break;
            }
        }
    }

    private void UpdateAssexualCycle()
    {
        fertilityTimer -= Time.deltaTime;
        if (fertilityTimer <= 0f)
        {
            fertilityTimer = animal.traits.fertilityCooldown;
            Debug.Log($"{animal.name} cloned itself (assexual reproduction)");
            // Clone instantiation later
        }
    }

    public void BeginGestation()
    {
        isGestating = true;
        gestationTimer = animal.traits.gestationDuration;

        // Apply penalties
        animal.traits.strength *= 1 - animal.traits.gestationStrengthPenalty;
        animal.traits.speed *= 1 - animal.traits.gestationSpeedPenalty;
        animal.traits.hungerRate *= animal.traits.gestationHungerMultiplier;
    }

    private void ResetGestationPenalties()
    {
        // For now just reset to trait base; we can improve by storing original values
        animal.traits.strength = animal.traits.baseStrength;
        animal.traits.speed = animal.traits.baseSpeed;
        animal.traits.hungerRate = animal.traits.baseHungerRate;
    }

    public bool IsFertile() => fertileTimeRemaining > 0f && !isGestating;
}
