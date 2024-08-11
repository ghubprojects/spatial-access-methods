namespace TheGridFile.PointIndexing;

using System;
using System.Collections.Generic;
using System.Linq;

class GridFile {
    private readonly int capacity;
    private List<Point>[,] directory;
    private readonly List<double> Sx;
    private readonly List<double> Sy;

    public GridFile(int initialCapacity) {
        capacity = initialCapacity;
        directory = new List<Point>[1, 1];
        directory[0, 0] = [];
        Sx = [];
        Sy = [];
    }

    public void Insert(Point point) {
        int cellX = FindCellIndex(Sx, point.X);
        int cellY = FindCellIndex(Sy, point.Y);

        if (directory[cellX, cellY].Count >= capacity) {
            SplitCell(cellX, cellY);
            Insert(point); // Re-insert after split
        } else {
            directory[cellX, cellY].Add(point);
        }
    }

    public bool PointQuery(Point point) {
        int cellX = FindCellIndex(Sx, point.X);
        int cellY = FindCellIndex(Sy, point.Y);

        return directory[cellX, cellY].Contains(point);
    }

    public List<Point> WindowQuery(double minX, double minY, double maxX, double maxY) {
        List<Point> result = [];
        int minCellX = FindCellIndex(Sx, minX);
        int maxCellX = FindCellIndex(Sx, maxX);
        int minCellY = FindCellIndex(Sy, minY);
        int maxCellY = FindCellIndex(Sy, maxY);

        for (int i = minCellX; i <= maxCellX; i++) {
            for (int j = minCellY; j <= maxCellY; j++) {
                result.AddRange(directory[i, j].Where(p => p.X >= minX && p.X <= maxX && p.Y >= minY && p.Y <= maxY));
            }
        }

        return result;
    }

    private static int FindCellIndex(List<double> scale, double value) {
        int index = 0;
        while (index < scale.Count && value > scale[index])
            index++;
        return index;
    }

    private void SplitCell(int cellX, int cellY) {
        bool splitX = directory.GetLength(0) <= directory.GetLength(1);

        if (splitX) {
            double midX = CalculateMidPoint(Sx, cellX);
            Sx.Insert(cellX, midX);
            ExpandDirectory(true);
        } else {
            double midY = CalculateMidPoint(Sy, cellY);
            Sy.Insert(cellY, midY);
            ExpandDirectory(false);
        }
    }

    private static double CalculateMidPoint(List<double> scale, int index) {
        double lower = index > 0 ? scale[index - 1] : 0;
        double upper = index < scale.Count ? scale[index] : 1;
        return (lower + upper) / 2;
    }

    private void ExpandDirectory(bool splitX) {
        int newRows = splitX ? directory.GetLength(0) + 1 : directory.GetLength(0);
        int newCols = splitX ? directory.GetLength(1) : directory.GetLength(1) + 1;
        var newDirectory = new List<Point>[newRows, newCols];

        for (int i = 0; i < directory.GetLength(0); i++) {
            for (int j = 0; j < directory.GetLength(1); j++) {
                newDirectory[i, j] = directory[i, j];
                if (splitX) {
                    newDirectory[i + 1, j] = new List<Point>();
                } else {
                    newDirectory[i, j + 1] = new List<Point>();
                }
            }
        }

        directory = newDirectory;
    }
}

class Point(double x, double y) {
    public double X { get; } = x;
    public double Y { get; } = y;

    // Override Equals and GetHashCode for correct comparison in lists
    public override bool Equals(object obj) {
        return obj is Point point &&
               X == point.X &&
               Y == point.Y;
    }

    public override int GetHashCode() {
        return HashCode.Combine(X, Y);
    }
}

class Program {
    static void Main() {
        GridFile gridFile = new(4);

        gridFile.Insert(new Point(0.1, 0.2));
        gridFile.Insert(new Point(0.4, 0.5));
        gridFile.Insert(new Point(0.7, 0.8));
        gridFile.Insert(new Point(0.9, 0.6));
        gridFile.Insert(new Point(0.2, 0.3));

        Console.WriteLine("Point Query (0.4, 0.5): " + gridFile.PointQuery(new Point(0.4, 0.5)));

        var windowQueryResult = gridFile.WindowQuery(0.1, 0.1, 0.5, 0.5);
        Console.WriteLine("Window Query (0.1, 0.1, 0.5, 0.5):");
        foreach (var point in windowQueryResult) {
            Console.WriteLine($"({point.X}, {point.Y})");
        }
    }
}
