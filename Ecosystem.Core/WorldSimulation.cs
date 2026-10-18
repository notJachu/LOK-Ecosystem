namespace Ecosystem.Core;

public class WorldSimulation
{
    private Dictionary<Point, CellData> _worldA = new Dictionary<Point, CellData>();
    private Dictionary<Point, CellData> _worldB = new Dictionary<Point, CellData>();
    private HashSet<Point> _cellsToEvaluate = new HashSet<Point>();

    private int _worldSize { get; set; }
    private int _turnNumber { get; set; }
    private int _animalCount { get; set; }
    private int _plantCount { get; set; }
    private bool _isAutoplay { get; set; }
    private short _autoplaySpeed { get; set; }

    // WORLD SIMULATION RULES
    private short _maxAnimalAge { get; set; } // L
    private short _maxAnimalRange { get; set; } // M
    private short _newPlantReq { get; set; } // R
    private short _newAnimalReq { get; set; } // B
    private bool _isPlantReq { get; set; } // P
    private short _plantSurviveReq { get; set; } // W
    private short _animalSurviveReq { get; set; } // Z

    WorldSimulation()
    {
    }

    public void reset()
    {
    }

    public WorldSimulation loadFromFile(string filename)
    {
        
    }
    
    public void saveToFile(string filename)
    {}
    
}