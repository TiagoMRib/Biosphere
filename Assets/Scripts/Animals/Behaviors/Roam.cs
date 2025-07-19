using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoamBehavior : MonoBehaviour
{
    private Animal animal;
    private Vector3 roamTarget;
    private float roamRadius = 10f;
    private bool hasTarget = false;

    public void Initialize(Animal animalRef)
    {
        animal = animalRef;
    }

    private void Update()
    {
        if (!hasTarget) return;

        transform.position = Vector3.MoveTowards(transform.position, roamTarget, animal.traits.speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, roamTarget) < 0.5f)
        {
            hasTarget = false; // Reached destination
        }
    }

    public void PickNewRoamTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection.y = 0; // Stay level
        roamTarget = transform.position + randomDirection;
        hasTarget = true;
    }

    public bool IsRoaming()
    {
        return hasTarget;
    }

    public void StopRoaming()
    {
        hasTarget = false;
    }
}

