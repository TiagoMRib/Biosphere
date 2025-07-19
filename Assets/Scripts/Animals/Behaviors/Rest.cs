using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestBehavior : MonoBehaviour
{
    private Animal animal;
    private bool isResting = false;
    private float restTimer = 0f;
    public float restDuration = 5f; 

    public void Initialize(Animal animalRef)
    {
        animal = animalRef;
    }

    public void StartRest()
    {
        isResting = true;
        restTimer = restDuration;
    }

    public void UpdateRest()
    {
        if (!isResting) return;

        restTimer -= Time.deltaTime;
        if (restTimer <= 0f)
        {
            isResting = false;
        }
    }

    public bool IsResting()
    {
        return isResting;
    }
}
