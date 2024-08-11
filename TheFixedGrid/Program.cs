using TheFixedGrid;

var grid = new FixedGrid(0, 0, 80, 80, 10, 3);
var pointList = new List<Point> {
    new(2, 74), new(2, 73), new(24, 71),
    new(48, 76), new(75, 72), new(2, 58),
    new(26, 59), new(48, 56), new(77, 57),
    new(2, 22), new(3, 23), new(25, 22),
    new(48, 24), new(76, 22), new(2, 3),
    new(24, 4), new(48, 3), new(75, 4),
    new(12, 54), new(32, 53), new(24, 41),
    new(48, 36), new(25, 32), new(12, 58),
    new(26, 49), new(18, 56), new(17, 57),
    new(52, 22), new(53, 23), new(15, 22),
    new(68, 24), new(66, 22), new(12, 3),
    new(24, 14), new(48, 33), new(75, 64),
    new(16, 51), new(49, 33), new(41, 37)
};
pointList.ForEach(grid.Insert);

// Point query
var p1 = new Point(48, 36);
var p2 = new Point(19, 19); // not exist
Console.WriteLine(grid.Query(p1) is not null ? $"Point({p1.X},{p1.Y}) Found" : $"Point({p1.X},{p1.Y}) Not Found");
Console.WriteLine(grid.Query(p2) is not null ? $"Point({p2.X},{p2.Y}) Found" : $"Point({p2.X},{p2.Y}) Not Found");

// Window query
var pList = grid.WindowQuery(new(10, 20), new(50, 60));
Console.WriteLine($"Window Result: {string.Join(",", pList.Select(p => $"({p.X},{p.Y})"))}");