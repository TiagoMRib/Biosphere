# GenoDrift Game Bible

## Species

A species is defined by a set of traits that determine its physical, behavioral, and reproductive characteristics. These traits influence how the animal survives, interacts, and evolves in the world:

- **Max Health:** The maximum health points the animal can have, usually tied with size.
- **Base Hunger Rate:** The rate at which health depletes due to hunger. Lower values mean the animal can go longer without food.
- **Eat Rate:** How quickly the animal regains health when eating.
- **Diet:** The types of food the animal can consume (Grass, Insects, Meat...).
- **Base Strength:** Influences fighting and hunting success.
- **Base Speed:** Determines movement speed.
- **Search Radius:** How far the animal can detect food, prey, or mates.
- **Comfortable Health Threshold:** The health percentage above which the animal is not considered hungry.
- **Boldness:** How much risk the animal takes (higher = more aggressive, lower = more cautious) - used to calculate if a predator will attack a prey close to its strength.
- **Fertility Cooldown:** Time between fertility cycles.
- **Fertility Duration:** How long the animal remains fertile during a cycle.
- **Gestation Duration:** How long pregnancy lasts (for females).
- **Gestation Strength Penalty:** Reduction in strength during gestation.
- **Gestation Speed Penalty:** Reduction in speed during gestation.
- **Gestation Hunger Multiplier:** Increase in hunger rate during gestation.

These traits are set in the animal's base configuration and can be modified by genetics, mutations, and environmental effects. 

- **Bunny**
    - Base Traits: 
        - Max Health: 100
        - Base Hunger Rate (health depletion): 5
        - Eat Rate (health replenish): 15
        - Diet: [Grass]
        - Base Strength: 1
        - Base Speed: 6
        - Search Radius: 8
        - Comfortable Health Threshold: 0.8
        - Boldness: 0.3
        - Fertility Cooldown: 50
        - Fertility Duration: 10
        - Gestation Duration: 20
        - Gestation Strength penalty: 0.3
        - Gestation Speed penalty: 0.3
        - Gestation Hunger Multiplier: 1.5

- **Male Fox**
    - Base Traits: 
        - Max Health: 180
        - Base Hunger Rate (health depletion): 2
        - Eat Rate (health replenish): 3
        - Diet: [Berries, Meat]
        - Base Strength: 4
        - Base Speed: 3
        - Search Radius: 16
        - Comfortable Health Threshold: 0.4
        - Boldness: 0.5
        - Fertility Cooldown: 50
        - Fertility Duration: 10
        - Gestation Duration: 30
        - Gestation Strength penalty: 0.3
        - Gestation Speed penalty: 0.3
        - Gestation Hunger Multiplier: 1.5

- **Female Fox**
    - Base Traits: 
        - Max Health: 180
        - Base Hunger Rate (health depletion): 2
        - Eat Rate (health replenish): 3
        - Diet: [Berries, Meat]
        - Base Strength: 3
        - Base Speed: 3
        - Search Radius: 16
        - Comfortable Health Threshold: 0.4
        - Boldness: 0.6
        - Fertility Cooldown: 50
        - Fertility Duration: 10
        - Gestation Duration: 30
        - Gestation Strength penalty: 0.3
        - Gestation Speed penalty: 0.3
        - Gestation Hunger Multiplier: 1.5
- **Lizard**
  - Base Traits: 
    - Max Health: 30
    - Base Hunger Rate (health depletion): 3
    - Eat Rate (health replenish): 10
    - Diet: [Insects]
    - Base Strength: 1
    - Base Speed: 8
    - Search Radius: 8
    - Comfortable Health Threshold: 0.4
    - Boldness: 0.9
    - Fertility Cooldown: 50
    - Fertility Duration: 10
    - Gestation Duration: 7
    - Gestation Strength penalty: 0.2
    - Gestation Speed penalty: 0.4
    - Gestation Hunger Multiplier: 1.5

---

## World Formulas & Probabilities

### GeneticCombiner
- **Stat Mutation:** +-10% for strength, speed (`mutationRate = 0.1`)
- **Hunger Boost:** `baseRate + (strength + speed - 1) * 0.02`
- **Diet Mutation:** 15% chance to add a new food type from same family (`DietMutation = 0.3`)
- **Rare Diet Mutation:** 1% chance to add a random food type from all possible (`rareMutationChance = 0.01`)
- **Combine Diets:** 60% chance to merge both parents' diets (`CombineDietsChance = 0.6`)
- **Sex Mutation:** 1% chance for sexual/assexual switch (`mutationChance = 0.01`)

### HuntingBehavior
- **Attack Range:** 1.5 units
- **Fight Duration:** `0.5 + (1 - strengthRatio) * 2` (strengthRatio = min(predator, prey) / max(predator, prey))
- **Kill Success:** `predatorRoll >= preyRoll` (roll = strength + random[0,2])
- **Carcass Bonus:** 10% of max health added to carcass abundance (healthier prey give more food)
- **Failed Hunt Penalty:** 10% of max health lost (lost energy)

### MatingBehavior
- **Fertility Cooldown:** (species trait)
- **Fertile Duration:** (species trait)
- **Gestation Duration:** (species trait)
- **Gestation Penalties:**
  - Strength: `* (1 - gestationStrengthPenalty)`
  - Speed: `* (1 - gestationSpeedPenalty)`
  - Hunger Rate: `* gestationHungerMultiplier`

### FoodSearchBehavior
- **Search Radius:** (species trait)
- **Prey Selection:**
  - Risk tolerance: `myStrength * (1 - 0.3 + boldness * 0.6 + hungerFactor * 0.4)`
  - Hunger factor: `1 - (currentHealth / maxHealth)`

---

