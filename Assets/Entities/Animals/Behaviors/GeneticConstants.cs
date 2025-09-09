
using System.Collections.Generic;
public enum Sex
{
    Male,
    Female,
    Assexual
}

public static class AnimalGeneticConstants
{
    public static readonly List<Sex> PossibleSexes = new List<Sex>
    {
        Sex.Male,
        Sex.Female,
        Sex.Assexual
    };

    public static readonly List<string> AllDietTags = new List<string>
    {
        "Grass",
        "Leaves",
        "Berries",
        "Fruit",
        "Meat",
        "Insects",
        "Mushrooms",
        "Seeds"
    };

    public static readonly Dictionary<string, string[]> DietFamilies = new Dictionary<string, string[]>
    {
        { "Foliage", new[] { "Grass", "Leaves" } },
        { "Fruit", new[] { "Berries", "Fruit" } },
        { "Meat", new[] { "Meat", "Insects", "Fish" } },
    };

    // Example: Mutation probability per trait type 
    public static class MutationChances
    {
        public const float StatMutation = 0.1f;      // 10% for strength, speed
        public const float HungerBoost = 0.02f;      // Added if faster/stronger
        public const float DietMutation = 0.3f;      // 30% chance of new food type
        public const float CombineDietsChance = 0.6f;
    }
}
