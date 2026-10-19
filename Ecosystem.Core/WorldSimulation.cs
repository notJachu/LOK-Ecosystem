namespace Ecosystem.Core;

public class WorldSimulation
{
    private Dictionary<Point, CellData> _worldA = new Dictionary<Point, CellData>();
    private Dictionary<Point, CellData> _worldB = new Dictionary<Point, CellData>();
    private HashSet<Point> _cellsToEvaluate = new HashSet<Point>();

    private bool _useWorldA = true;

    private int _worldSize { get; set; }
    private int _turnNumber { get; set; }
    private int _animalCount { get; set; }
    private int _plantCount { get; set; }
    private bool _isAutoplay { get; set; }
    private short _autoplaySpeed { get; set; }

    private SimulationRules _simulationRules { get; set; }

    WorldSimulation()
    {
        _worldSize = 50;
        _turnNumber = 0;
        _animalCount = 0;
        _plantCount = 0;
        _isAutoplay = false;
        _autoplaySpeed = 500;

        _simulationRules = new SimulationRules(
            10, 2,
            new[] { 1, 2, 3, 4 },
            new[] { 1, 2, 3, 4, 5, 6 },
            new[] { 2, 3 },
            new[] { 0 },
            new[] { 0, 1, 2, 3 });
    }

    public WorldSimulation(
        int worldSize,
        int turnNumber,
        int animalCount,
        int plantCount,
        bool isAutoplay,
        short autoplaySpeed,
        SimulationRules simulationRules)
    {
        _worldSize = worldSize;
        _turnNumber = turnNumber;
        _animalCount = animalCount;
        _plantCount = plantCount;
        _isAutoplay = isAutoplay;
        _autoplaySpeed = autoplaySpeed;
        _simulationRules = simulationRules;
    }

    public void Reset()
    {
        _turnNumber = 0;
        _animalCount = 0;
        _plantCount = 0;
        _isAutoplay = false;
        _autoplaySpeed = 500;

        _worldA = new Dictionary<Point, CellData>();
        _worldB = new Dictionary<Point, CellData>();
        _cellsToEvaluate.Clear();
    }

    public WorldSimulation LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException("File not found", filename);

        using (StreamReader reader = new StreamReader(filename))
        {
            string[] stateParts = reader.ReadLine().Split(';');
            _worldSize = int.Parse(stateParts[0]);
            _turnNumber = int.Parse(stateParts[1]);
            _autoplaySpeed = short.Parse(stateParts[2]);

            string[] ruleParts = reader.ReadLine().Split(';');
            _simulationRules = new SimulationRules(
                maxAnimalAge: int.Parse(ruleParts[0]),
                maxAnimalRange: int.Parse(ruleParts[1]),
                plantBirthMask: int.Parse(ruleParts[2]),
                plantSurvivalMask: int.Parse(ruleParts[3]),
                animalBirthMask: int.Parse(ruleParts[4]),
                animalBirthPlantMask: int.Parse(ruleParts[5]),
                animalSurvivalMask: int.Parse(ruleParts[6])
            );

            _worldA.Clear();
            _worldB.Clear();
            _cellsToEvaluate.Clear();
            _useWorldA = true;
            _plantCount = 0;
            _animalCount = 0;

            string line;
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] cellParts = line.Split(';');
                Point p = new Point(int.Parse(cellParts[0]), int.Parse(cellParts[1]));

                CellData cell = new CellData
                {
                    HasPlant = bool.Parse(cellParts[2]),
                    HasAnimal = bool.Parse(cellParts[3]),
                    AnimalAge = short.Parse(cellParts[4])
                };

                _worldA[p] = cell;

                if (cell.HasPlant) _plantCount++;
                if (cell.HasAnimal) _animalCount++;
            }
        }
        
        return this;
    }

    public void SaveToFile(string filename)
    {
        var currentWorld = _useWorldA ? _worldA : _worldB;

        using (StreamWriter writer = new StreamWriter(filename))
        {
            writer.WriteLine($"{_worldSize};{_turnNumber};{_autoplaySpeed}");

            writer.WriteLine(
                $"{_simulationRules.MaxAnimalAge};{_simulationRules.MaxAnimalRange};{_simulationRules.PlantBirthMask};{_simulationRules.PlantSurvivalMask};{_simulationRules.AnimalBirthMask};{_simulationRules.AnimalBirthPlantMask};{_simulationRules.AnimalSurvivalMask}");

            foreach (var item in currentWorld)
            {
                Point p = item.Key;
                CellData cell = item.Value;

                writer.WriteLine($"{p.X};{p.Y};{cell.HasPlant};{cell.HasAnimal};{cell.AnimalAge}");
            }
        }
    }

    public void NextStep()
    {
        var currentWorld = _useWorldA ? _worldA : _worldB;
        var nextWorld = _useWorldA ? _worldB : _worldA;

        _cellsToEvaluate.Clear();
        nextWorld.Clear();
        _plantCount = 0;
        _animalCount = 0;
        
        foreach (var point in currentWorld.Keys)
        {
            _cellsToEvaluate.Add(point);
            point.AddNeighboursTo(_cellsToEvaluate, _worldSize, _worldSize);
        }

        foreach (var point in _cellsToEvaluate)
        {
            var (plantNeighbourCount, animalNeighbourCount) = CellData.CountNeighbours(point, currentWorld);
            currentWorld.TryGetValue(point, out CellData currentCell);

            CellData nextCell =
                currentCell.GetNextCellState(plantNeighbourCount, animalNeighbourCount, _simulationRules);

            // TODO: move animals
            if (nextCell.HasPlant || nextCell.HasAnimal)
            {
                nextWorld[point] = nextCell;
                _plantCount += nextCell.HasPlant ? 1 : 0;
                _animalCount += nextCell.HasAnimal ? 1 : 0;
            }
        }
    }
}