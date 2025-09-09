using System.Collections;
using UnityEngine;

public class MatingBehavior : MonoBehaviour
{
    private Animal animal;

    private float matingRange = 1.5f;
    private bool isMating = false;

    // Male only
    private Animal targetMate;

    // Female only
    private float fertilityTimer = 0f;
    private float fertileTimeRemaining = 0f;
    private bool isGestating = false;
    private float gestationTimer = 0f;

    // Store child's traits during gestation
    private AnimalTraits pendingChildTraits;

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
                // Instantiate offspring using stored traits
                if (pendingChildTraits != null)
                {
                    AnimalFactory.CreateAnimal(pendingChildTraits, animal.transform.position, Quaternion.identity, animal.transform.parent);
                    pendingChildTraits = null;
                }
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
        if (isMating) return;

        // If already pursuing a mate
        if (targetMate != null)
        {
            if (targetMate == null || targetMate.gameObject == null)
            {
                targetMate = null;
                return;
            }
            animal.MoveTowards(targetMate.transform.position);

            if (Vector3.Distance(animal.transform.position, targetMate.transform.position) <= matingRange)
            {
                StartCoroutine(MatingRoutine(targetMate));
            }
            return;
        }

        // Search for a fertile female of same species
        Collider[] hits = Physics.OverlapSphere(animal.transform.position, animal.GetSearchRadius());
        foreach (var hit in hits)
        {
            Animal other = hit.GetComponent<Animal>();
            if (other == null || other == animal || other.gameObject == null) continue;

            if (animal.GetSpeciesTag() == other.GetSpeciesTag() &&
                other.traits.sex == Sex.Female &&
                other.matingBehavior.IsFertile())
            {
                targetMate = other;
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
            AnimalFactory.CreateClone(animal.traits, animal.transform.position, Quaternion.identity, animal.transform.parent);
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

    private IEnumerator MatingRoutine(Animal female)
    {
        isMating = true;
        animal.FreezeMovement();
        female.FreezeMovement();

        Debug.Log($"{animal.name} is mating with {female.name}...");
        yield return new WaitForSeconds(2f); // simulate time spent mating

    // Generate child traits at conception
    AnimalTraits childTraits = GeneticCombiner.CombineTraits(animal.traits, female.traits);
    female.matingBehavior.pendingChildTraits = childTraits;
    female.matingBehavior.BeginGestation();
    Debug.Log($"{female.name} is now pregnant!");

        // Unfreeze both
        animal.UnfreezeMovement();
        female.UnfreezeMovement();

        // Reset state
        targetMate = null;
        isMating = false;
    }

}
