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
    
    public static (int plantCount, int animalCount) CountNeighbours(Point p, Dictionary<Point, CellData> currentWorld)
    {
        int plantCount = 0;
        int animalCount = 0;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                Point neighbourPoint = new Point(p.X + dx, p.Y + dy);

                if (currentWorld.TryGetValue(neighbourPoint, out CellData neighbourData))
                {
                    if (neighbourData.HasPlant) plantCount++;
                    if (neighbourData.HasAnimal) animalCount++;
                }
            }
        }

        return (plantCount, animalCount);
    }
}
