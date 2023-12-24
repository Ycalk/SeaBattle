namespace SeaBattle
{
    internal readonly struct IntPoint
    {
        public readonly int X;
        public readonly int Y;
        public readonly bool NotEmpty;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
            NotEmpty = true;
        }
        public static IntPoint EmptyX(int y) => new(0, y);
        public static IntPoint EmptyY(int x) => new(x, 0);
        public IntPoint WithX(int x) => new(x, Y);
        public IntPoint WithY(int y) => new(X, y);
        public override string ToString() => "(" + X + ", " + Y + ")";
        public static IntPoint operator -(IntPoint a, IntPoint b) => new(a.X - b.X, a.Y - b.Y);
        public static IntPoint operator +(IntPoint a, IntPoint b) => new(a.X + b.X, a.Y + b.Y);
        public static bool operator ==(IntPoint a, IntPoint b) => a.X == b.X && a.Y == b.Y;
        public static bool operator !=(IntPoint a, IntPoint b) => a.X != b.X && a.Y != b.Y;
    }
}
