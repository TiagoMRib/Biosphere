# 🐾 Ecosystem Simulation (Very early development)

This project is a Unity-based **ecosystem simulator** where different animal species interact with their environment.  
The goal is to create emergent behaviors such as **hunting, eating, roaming, mating, and genetic inheritance**, leading to evolving populations over time.

---

## ✅ Current Features

### 🎮 Core Animal System
- `AnimalTraits` (ScriptableObject) defines species and sex-specific characteristics:
  - Health, strength, speed
  - Hunger rate, diet, comfort thresholds
  - Reproduction attributes (sex, fertility, gestation, penalties)
- Animals are instantiated with behaviors attached:
  - **FoodSearchBehavior** > Finds nearby food
  - **EatBehavior** > Consumes food to restore health
  - **RoamBehavior** > Random roaming when idle
  - **RestBehavior** > Temporary inactivity for recovery
  - **HuntingBehavior** > Predators fight and kill prey
  - **MatingBehavior** > Handles reproduction cycle (male/female/assexual)

### ⚔️ Hunting & Food
- Predators (e.g., foxes) can hunt prey (e.g., rabbits).
- Fighting system based on **strength rolls + randomness**.
- Prey leave behind **carcass food sources** with abundance values.
- Carcasses are edible by animals with "Meat" in their diet.
- **FoodSource system**:  
  Any object that animals can consume (e.g., grass patches, berry bushes, carcasses) is represented as a `FoodSource`.  
  Each `FoodSource` has:
  - A **tag** (e.g., "Grass", "Fruit", "Meat") checked against animal diets.
  - An **abundance value** representing how much food remains.
  - A **consumption rate** tied to the eater’s traits (`eatRate`).  
  This makes food a shared resource that can deplete and regenerate, creating competition in the ecosystem.

### 🍃 Roaming & Freezing
- Animals roam randomly when idle.
- Movement is frozen during fights, mating, or gestation.

### 🧬 Reproduction
- **Sexual reproduction**:
  - Females have fertility cycles and gestation periods.
  - Males seek fertile females in their search radius.
  - Gestating females suffer penalties (speed, strength, hunger).
- **Assexual reproduction**:
  - Animals can clone themselves after a cooldown.

### 🧬 GeneticCombiner
- Creates offspring traits by combining **numeric, categorical, and diet data**.
- Supports:
  - Numeric blending with mutation windows (e.g., speed, strength).
  - Diet inheritance with probability of merging lists.
  - Mutation likelihood higher within diet families (e.g., [Grass, Leafs] group).
  - Chance-based mutations for diversity.

---

## 🚀 Vision

The long-term vision is a **self-sustaining evolving ecosystem simulator** where animals adapt over time, forming unique populations through mutation and selection.

---

## 🔮 Future Work Checklist

- [ ] **Aging system**:  
  - Animals are born as babies with penalties (speed, strength).  
  - They mature into adults with peak traits.  
  - Old age brings penalties.  

- [ ] **Biome system**:  
  - Biome regions with temperature, vegetation, and camouflage effects.  
  - Animals adapt better/worse depending on their traits and colors.  
  - Climate affects food growth and animal survival.  

- [ ] **Seasonal cycles**:  
  - Changing temperature, day length, and food availability over time.  
  - Seasonal migrations as a possible emergent behavior.  

- [ ] **Egg-laying animals**:  
  - Alternative reproduction path where offspring hatch after incubation.
  - Egg as a food source that, if not destroyed spawns animals  

- [ ] **Camouflage & visibility**:  
  - Animal colors compared to biome colors for predator/prey detection difficulty.  

- [ ] **Child protection behavior**:  
  - Parents may defend or guide offspring.  
  - Herd/pack dynamics possible in social species.  
