namespace Ecosystem.Core;

public struct CellData
{
    public bool HasPlant;
    public bool HasAnimal;
    public short AnimalAge;

    public CellData GetNextCellState(int plantNeighbours, int animalNeighbours, in SimulationRules simulationRules)
    {
        CellData nextCellState = new CellData();

        if (HasAnimal)
        {
            if (AnimalAge < simulationRules.MaxAnimalAge &&
                SimulationRules.IsRuleSatisfied(simulationRules.AnimalSurvivalMask, animalNeighbours))
            {
                nextCellState.HasAnimal = true;
                nextCellState.AnimalAge = (short)(AnimalAge + 1);
            }
            
        }
        else if (HasPlant)
        {
            if (SimulationRules.IsRuleSatisfied(simulationRules.PlantSurvivalMask, plantNeighbours))
            {
                nextCellState.HasPlant = true;
            }
            
        }
        else
        {
            if (SimulationRules.IsRuleSatisfied(simulationRules.AnimalBirthMask, animalNeighbours) &&
                SimulationRules.IsRuleSatisfied(simulationRules.AnimalBirthPlantMask, Convert.ToInt32(HasPlant)))
            {
                nextCellState.HasAnimal = true;
                nextCellState.AnimalAge = 0;
            }
            else if (SimulationRules.IsRuleSatisfied(simulationRules.PlantBirthMask, plantNeighbours))
            {
                nextCellState.HasPlant = true;
            }
        }
        
        return nextCellState;
    }
}
