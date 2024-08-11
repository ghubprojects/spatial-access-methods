namespace TheFixedGrid;

public class Point(int x, int y) {
    public int X { get; set; } = x;
    public int Y { get; set; } = y;

    // Override Equals and GetHashCode for correct comparison in lists
    public override bool Equals(object obj) {
        return obj is Point point && X == point.X && Y == point.Y;
    }

    public override int GetHashCode() {
        return HashCode.Combine(X, Y);
    }
}
