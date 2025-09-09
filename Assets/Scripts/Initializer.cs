using UnityEngine;

public class Initializer : MonoBehaviour
{
    public GameObject baseAnimalPrefab; // Assign in inspector

    private static Initializer _instance;
    public static Initializer Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);

        AnimalFactory.BaseAnimalPrefab = baseAnimalPrefab;
    }

}