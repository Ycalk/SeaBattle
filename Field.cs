namespace SeaBattle
{
    internal class Field
    {
        private class Ship
        {
            public readonly int Size;
            public readonly ERotation Rotation;
            public bool IsDead { get; private set; }
            public readonly IntPoint Position;
            internal enum ERotation
            {
                Vertical,
                Horizontal
            }

            public void MarkAsDead() =>
                IsDead = true;


            public Ship(ERotation rotation, int size, IntPoint position)
            {
                if (size is < 1 or > 4)
                    throw new Exception("Incorrect ship size");

                Rotation = rotation;
                Size = size;
                Position = position;
            }
        }

        public const int FieldSize = 10;
        private static readonly Dictionary<int, int> ShipCount = new()
        {
            { 4, 1 },
            { 3, 2 },
            { 2, 3 },
            { 1, 4 }
        };

        public enum FieldElements
        {
            Empty,
            Ship,
            Miss,
            Hit,
            Dead
        }

        public readonly FieldElements[,] Elements;
        private readonly List<Ship> _ships = new();

        private Field()
        {
            Elements = new FieldElements[FieldSize, FieldSize];
        }

        public bool IsAllShipsDead() =>
            _ships.All(ship => ship.IsDead);

        public void MarkShipsAsDead(Field hitsResults)
        {
            foreach (var ship in _ships
                         .Where(ship => !ship.IsDead)
                         .Where(ship => CheckShip(ship, hitsResults)))
            {
                MarkShipAsDead(ship, hitsResults);
            }
        }

        private void MarkShipAsDead(Ship ship, Field hitsResults)
        {
            var toMark = new IntPoint[ship.Size];
            ship.MarkAsDead();
            for (var i = 0; i < ship.Size; i++)
            {
                var markPoint = ship.Rotation == Ship.ERotation.Vertical ?
                    ship.Position.WithY(ship.Position.Y + i) :
                    ship.Position.WithX(ship.Position.X + i);

                MarkPointNeighbors(hitsResults.Elements, markPoint, FieldElements.Miss);
                MarkPointNeighbors(Elements, markPoint, FieldElements.Miss);
                toMark[i] = markPoint;
            }
            foreach (var p in toMark)
            {
                Elements[p.X, p.Y] = FieldElements.Dead;
                hitsResults.Elements[p.X, p.Y] = FieldElements.Dead;
            }
        }

        private static void MarkPointNeighbors(FieldElements[,] field, IntPoint point, FieldElements marker)
        {
            MarkIfValid(point.X - 1, point.Y);
            MarkIfValid(point.X + 1, point.Y);
            MarkIfValid(point.X, point.Y - 1);
            MarkIfValid(point.X, point.Y + 1);
            MarkIfValid(point.X - 1, point.Y - 1);
            MarkIfValid(point.X + 1, point.Y + 1);
            MarkIfValid(point.X - 1, point.Y + 1);
            MarkIfValid(point.X + 1, point.Y - 1);
            return;

            void MarkIfValid(int x, int y)
            {
                if (x is >= 0 and < FieldSize && y is >= 0 and < FieldSize)
                    field[x, y] = marker;
            }
        }


        private static bool CheckShip(Ship ship, Field hitsResults)
        {
            for (var i = 0; i < ship.Size; i++)
            {
                var checkPoint = ship.Rotation == Ship.ERotation.Vertical ? 
                    ship.Position.WithY(ship.Position.Y + i) : 
                    ship.Position.WithX(ship.Position.X + i);
                if (hitsResults.Elements[checkPoint.X, checkPoint.Y] != FieldElements.Hit)
                    return false;
            }
            return true;
        }

        public static class FieldCreator
        {
            public static Field GetEmptyField () =>
                new();
            
            public static Field GetRandomField()
            {
                var field = new Field();
                foreach (var deckCount in ShipCount.Keys)
                    for (var i = 0; i < ShipCount[deckCount]; i++)
                    {
                        var ship = GetRandomShip(deckCount);
                        while (!CheckShipPosition(ship, field.Elements))
                            ship = GetRandomShip(deckCount);
                        PutShip(ship, field.Elements);
                        field._ships.Add(ship);
                    }
                return field;
            }

            private static Ship GetRandomShip(int size)
            {
                var random = new Random();
                var rotation = (Ship.ERotation)random.Next(2);
                var xMax = FieldSize;
                var yMax = FieldSize;
                if (rotation == Ship.ERotation.Vertical)
                    yMax -= size;
                else
                    xMax -= size;
                var position = new IntPoint(random.Next(xMax), random.Next(yMax));
                return new Ship(rotation, size, position);
            }

            private static void PutShip(Ship ship, FieldElements[,] field)
            {
                for (var j = 0; j < ship.Size; j++)
                {
                    if (ship.Rotation == Ship.ERotation.Vertical)
                        field[ship.Position.X, ship.Position.Y + j] = FieldElements.Ship;
                    else
                        field[ship.Position.X + j, ship.Position.Y] = FieldElements.Ship;
                }
            }

            private static bool CheckShipPosition(Ship ship, FieldElements[,] field)
            {
                for (var i = 0; i < ship.Size; i++)
                {
                    if (ship.Rotation == Ship.ERotation.Vertical)
                    {
                        if (CheckPoint(ship.Position.WithY(ship.Position.Y + i), field))
                            return false;
                    }
                    else
                    {
                        if (CheckPoint(ship.Position.WithX(ship.Position.X + i), field))
                            return false;
                    }
                }
                return true;
            }

            private static bool CheckPoint(IntPoint point, FieldElements[,] field)
            {
                return IsShipAtPoint(point.X, point.Y) ||
                       IsShipAtPoint(point.X - 1, point.Y) ||
                       IsShipAtPoint(point.X + 1, point.Y) ||
                       IsShipAtPoint(point.X, point.Y - 1) ||
                       IsShipAtPoint(point.X, point.Y + 1) ||
                       IsShipAtPoint(point.X - 1, point.Y - 1) ||
                       IsShipAtPoint(point.X + 1, point.Y + 1) ||
                       IsShipAtPoint(point.X - 1, point.Y + 1) ||
                       IsShipAtPoint(point.X + 1, point.Y - 1);

                bool IsShipAtPoint(int x, int y) =>
                    x is >= 0 and < FieldSize && y is >= 0 and < FieldSize && field[x, y] == FieldElements.Ship;
            }
        }
    }
}
