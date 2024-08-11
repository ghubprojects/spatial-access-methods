namespace TheFixedGrid;

public class FixedGrid {
    // real space
    private readonly int sX0;
    private readonly int sY0;
    private readonly int sXSize;
    private readonly int sYSize;
    private readonly int cellSize;
    private readonly int cellCapacity;

    // directory
    private readonly int nx;
    private readonly int ny;
    private readonly List<Point>[,] pages;
    private readonly Dictionary<(int, int), List<Point>> overflowPages;

    public FixedGrid(int sX0, int sY0, int sXmax, int sYmax, int cellSize, int cellCapacity) {
        this.sX0 = sX0;
        this.sY0 = sY0;
        sXSize = sXmax - sX0;
        sYSize = sYmax - sY0;
        this.cellSize = cellSize;
        this.cellCapacity = cellCapacity;

        nx = sXSize / cellSize;
        ny = sYSize / cellSize;

        // init directory
        pages = new List<Point>[nx, ny];
        overflowPages = [];
        Enumerable.Range(0, nx).ToList().ForEach(i =>
        Enumerable.Range(0, ny).ToList().ForEach(j =>
        pages[i, j] = []));
    }

    public void Insert(Point p) {
        var (i, j) = GetCellIndex(p);
        if (pages[i, j].Count < cellCapacity) {
            pages[i, j].Add(p);
        } else {
            if (!overflowPages.ContainsKey((i, j))) {
                overflowPages[(i, j)] = [];
            }
            Console.WriteLine($"Overflow at cell: {i},{j}");
            overflowPages[(i, j)].Add(p);
        }
    }

    private (int, int) GetCellIndex(Point p) {
        int i = (p.X - sX0) / cellSize;
        int j = (p.Y - sY0) / cellSize;
        return (i, j);
    }

    public Point? Query(Point p) {
        var (i, j) = GetCellIndex(p);
        foreach (var point in pages[i, j]) {
            if (point.Equals(p))
                return point;
        }
        // Check in overflow page
        if (overflowPages.ContainsKey((i, j))) {
            foreach (var point in overflowPages[(i, j)]) {
                if (point.Equals(p))
                    return point;
            }
        }
        return null;
    }

    public List<Point> WindowQuery(Point p1, Point p2) {
        List<Point> result = [];
        var (i1, j1) = GetCellIndex(p1);
        var (i2, j2) = GetCellIndex(p2);

        for (int i = i1; i <= i2; i++)
            for (int j = j1; j <= j2; j++) {
                result.AddRange(pages[i, j]);
                if (overflowPages.ContainsKey((i, j)))
                    result.AddRange(overflowPages[(i, j)]);
            }
        return result;
    }
}
