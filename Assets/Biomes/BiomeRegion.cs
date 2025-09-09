using UnityEngine;

public enum BiomeType { Grassland, Forest, Desert, Tundra}

public class BiomeRegion : MonoBehaviour
{
    public BiomeType biomeType;
    public Color biomeColor = Color.green;
    public float temperature = 20f; // °C baseline
    
}
