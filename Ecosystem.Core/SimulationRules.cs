namespace Ecosystem.Core;
using System.Collections.Generic;

public readonly struct SimulationRules
{
    public readonly int MaxAnimalAge; // L
    public readonly int MaxAnimalRange; // M
    
    public readonly int PlantBirthMask; // R
    public readonly int PlantSurvivalMask; // W
    public readonly int AnimalBirthMask; // B
    public readonly int AnimalBirthPlantMask; // P
    public readonly int AnimalSurvivalMask; // Z

    public SimulationRules(int maxAnimalAge, int maxAnimalRange, 
        int plantBirthMask, int plantSurvivalMask,
        int animalBirthMask, int animalBirthPlantMask, int animalSurvivalMask)
    {
        MaxAnimalAge = maxAnimalAge;
        MaxAnimalRange = maxAnimalRange;
        PlantBirthMask = plantBirthMask;
        PlantSurvivalMask = plantSurvivalMask;
        AnimalBirthMask = animalBirthMask;
        AnimalBirthPlantMask = animalBirthPlantMask;
        AnimalSurvivalMask = animalSurvivalMask;
    }

    public SimulationRules(int maxAnimalAge, int maxAnimalRange, 
        IEnumerable<int> plantBirthRules,
        IEnumerable<int> plantSurvivalRules,
        IEnumerable<int> animalBirthRules, 
        IEnumerable<int> animalBirthPlantRules, 
        IEnumerable<int> animalSurvivalRules)
    {
        MaxAnimalAge = maxAnimalAge;
        MaxAnimalRange = maxAnimalRange;
        PlantBirthMask = ToMask(plantBirthRules);
        PlantSurvivalMask = ToMask(plantSurvivalRules);
        AnimalBirthMask = ToMask(animalBirthRules);
        AnimalBirthPlantMask = ToMask(animalBirthPlantRules);
        AnimalSurvivalMask = ToMask(animalSurvivalRules);
    }

    private static int ToMask(IEnumerable<int> rules)
    {
        int mask = 0;
        if (rules != null)
        {
            foreach (int rule in rules)
            {
                mask |= (1 << rule); 
            }
        }
        return mask;
    }

    public static bool IsRuleSatisfied(int ruleMask, int state) => 
        ((1 << state) & ruleMask) != 0;
    
}