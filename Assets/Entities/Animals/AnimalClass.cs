using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodSearchBehavior), typeof(EatBehavior))]
public class Animal : MonoBehaviour
{
    public AnimalTraits traits;
    public float currentHealth;
    private bool isFrozen = false;

    // Behaviors
    private FoodSearchBehavior foodSearch;
    private HuntingBehavior huntingBehavior;
    private EatBehavior eatBehavior;
    private RoamBehavior roamBehavior;
    private RestBehavior restBehavior;
    public MatingBehavior matingBehavior;


    // Visual representation
    private GameObject visualInstance;
    

    private void Awake()
    {
        foodSearch = GetComponent<FoodSearchBehavior>();
        huntingBehavior = GetComponent<HuntingBehavior>();
        eatBehavior = GetComponent<EatBehavior>();
        roamBehavior = GetComponent<RoamBehavior>();
        restBehavior = GetComponent<RestBehavior>();
        matingBehavior = GetComponent<MatingBehavior>();
    }

    public void Initialize(AnimalTraits traits)
    {
        this.traits = traits;
        currentHealth = traits.maxHealth;

        foodSearch.Initialize(this);
        huntingBehavior.Initialize(this);
        eatBehavior.Initialize(this);
        roamBehavior.Initialize(this);
        restBehavior.Initialize(this);
        matingBehavior.Initialize(this);

        SpawnVisual();
    }

    private void Update()
    {
        if (restBehavior.IsResting())
        {
            restBehavior.UpdateRest();
            return; // Fully pause behavior while resting
        }

        float healthPercent = currentHealth / traits.maxHealth;
        bool isHungry = healthPercent < traits.comfortableHealthThreshold;

        // 1. Prioritize eating if already eating (even if not hungry anymore)
        if (eatBehavior.IsEating())
        {
            eatBehavior.UpdateEating();

            FoodSource currentFood = eatBehavior.GetCurrentFood();
            if (currentFood != null && Vector3.Distance(transform.position, currentFood.transform.position) > 1f)
            {
                MoveTowards(currentFood.transform.position);
            }

            return;
        }
        HandleHunger();

        // 2. Find food
        if (isHungry)
        {
            // Try static food
            FoodSource food = foodSearch.FindFood();
            if (food != null)
            {
                roamBehavior.StopRoaming();

                if (Vector3.Distance(transform.position, food.transform.position) < 1f)
                {
                    eatBehavior.StartEating(food);
                }
                else
                {
                    MoveTowards(food.transform.position);
                }

                return;
            }

            // 3. if no easy meal, look for prey
        
            Animal prey = foodSearch.FindPrey();
            if (prey != null)
            {
                roamBehavior.StopRoaming();
                huntingBehavior.Hunt(prey);
            }

            // If actively hunting, continue updating that behavior
            if (huntingBehavior.IsHunting())
            {
                huntingBehavior.UpdateHunt();
                return;
            }

            
        }

        // 3. If not hungry, check for mates
        matingBehavior.UpdateReproduction();

        // 4. Roam if not hungry or no food/prey found
        if (!roamBehavior.IsRoaming() && !IsFrozen())
        {
            roamBehavior.PickNewRoamTarget();
        }
    }


    public string GetSpeciesTag()
    {
        return traits.speciesTag;
    }

    private void HandleHunger()
    {
        currentHealth -= traits.hungerRate * Time.deltaTime;
        if (currentHealth <= 0)
            Die();
    }

    public void ApplyFailedHuntPenalty()
    {
        float penalty = traits.maxHealth * 0.1f; // 10% health loss (can tune this)
        currentHealth -= penalty;
        currentHealth = Mathf.Max(currentHealth, 0f);

        restBehavior.StartRest();
    }


    public void FreezeMovement()
    {
        Debug.Log($"{name} is frozen and cannot move.");
        isFrozen = true;
    }

    public void UnfreezeMovement()
    {
        isFrozen = false;
    }

    public bool IsFrozen()
    {
        return isFrozen;
    }

    public void MoveTowards(Vector3 target)
    {
        if (isFrozen) return;
        transform.position = Vector3.MoveTowards(transform.position, target, traits.speed * Time.deltaTime);
    }

    private void SpawnVisual()
    {
        if (traits.visualPrefab != null)
        {
            visualInstance = Instantiate(traits.visualPrefab, transform);
            visualInstance.transform.localPosition = Vector3.zero;
            visualInstance.transform.localRotation = Quaternion.identity;
        }
    }   

    private void Die()
    {
        Destroy(gameObject);
    }

    public List<string> GetDiet() => traits.dietTags;
    public float GetSearchRadius() => traits.searchRadius;
}
