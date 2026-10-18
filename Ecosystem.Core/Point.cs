namespace Ecosystem.Core;

public readonly struct Point : IEquatable<Point>
{
    public readonly int X;
    public readonly int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public void AddNeighborsTo(HashSet<Point> candidateSet, int boardWidth, int boardHeight)
    {
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = X + dx;
                int ny = Y + dy;

                if (nx >= 0 && nx < boardWidth && ny >= 0 && ny < boardHeight)
                {
                    candidateSet.Add(new Point(nx, ny));
                }
            }
        }
    }

    public bool Equals(Point other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Point other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}
