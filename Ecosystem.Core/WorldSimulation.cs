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

    public void reset()
    {
    }

    public WorldSimulation loadFromFile(string filename)
    {
    }

    public void saveToFile(string filename)
    {
    }

    public void NextStep()
    {
        var currentWorld = _useWorldA ? _worldA : _worldB;
        var nextWorld = _useWorldA ? _worldB : _worldA;

        _cellsToEvaluate.Clear();
        nextWorld.Clear();

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
            }
        }
    }
}