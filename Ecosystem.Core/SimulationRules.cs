namespace Ecosystem.Core;
public readonly struct SimulationRules
{
    public readonly int MaxAnimalAge; // L
    public readonly int MaxAnimalRange; // M
    public readonly HashSet<int> PlantBirthRules; // R
    public readonly HashSet<int> PlantSurvivalRules; // W
    public readonly HashSet<int> AnimalBirthRules; // B
    public readonly HashSet<int> AnimalBirthPlantRules; // P
    public readonly HashSet<int> AnimalSurvivalRules; // Z

    SimulationRules(int maxAnimalAge, int maxAnimalRange, HashSet<int> plantBirthRules,
        HashSet<int> plantSurvivalRules,
        HashSet<int> animalBirthRules, HashSet<int> animalBirthPlantRules, HashSet<int> animalSurvivalRules)
    {
        MaxAnimalRange = maxAnimalRange;
        PlantBirthRules = plantBirthRules;
        PlantSurvivalRules = plantSurvivalRules;
        MaxAnimalAge = maxAnimalAge;
        AnimalBirthRules = animalBirthRules;
        AnimalSurvivalRules = animalSurvivalRules;
        AnimalBirthPlantRules = animalBirthPlantRules;
    }
}