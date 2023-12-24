using System;
using static SeaBattle.Field;

namespace SeaBattle
{
    internal class Player
    {
        public enum Type
        {
            Human,
            Computer
        }

        public readonly Type PlayerType;
        public readonly Field Field;
        public readonly Field HitsResults;

        public Player(Type playerType, Field field, Field hitsResults)
        {
            PlayerType = playerType;
            Field = field;
            HitsResults = hitsResults;
        }

        public bool MakeMove(Player enemy, ref IntPoint cursor)
        {

            var coordinates = PlayerType == Type.Human ? 
                GetPointFromConsole(ref cursor) : GetRandomPoint();


            if (enemy.Field.Elements[coordinates.X, coordinates.Y] == FieldElements.Ship)
            {
                enemy.Field.Elements[coordinates.X, coordinates.Y] = FieldElements.Hit;
                HitsResults.Elements[coordinates.X, coordinates.Y] = FieldElements.Hit;
                enemy.Field.MarkShipsAsDead(HitsResults);
                return true;
            }
            enemy.Field.Elements[coordinates.X, coordinates.Y] = FieldElements.Miss;
            HitsResults.Elements[coordinates.X, coordinates.Y] = FieldElements.Miss;
            return false;
        }

        private IntPoint GetRandomPoint()
        {
            var random = new Random();
            var point = new IntPoint(random.Next(FieldSize), random.Next(FieldSize));

            while (HitsResults.Elements[point.X, point.Y] != FieldElements.Empty)
                point = new IntPoint(random.Next(FieldSize), random.Next(FieldSize));

            return point;
        }

        private IntPoint GetClosestEmptyPoint(IntPoint start, IntPoint offset)
        {
            var result = start;
            while (result.X + offset.X is >= 0 and < FieldSize &&
                   result.Y + offset.Y is >= 0 and < FieldSize)
            {
                result += offset;
                if (HitsResults.Elements[result.X, result.Y] == FieldElements.Empty)
                    return result;
            }

            return start;
        }

        private IntPoint GetPointFromConsole(ref IntPoint coordinates)
        {
            var key = Console.ReadKey().Key;
            while (key != ConsoleKey.Spacebar ||
                   HitsResults.Elements[coordinates.X, coordinates.Y] != FieldElements.Empty)
            {
                coordinates = key switch
                {
                    ConsoleKey.UpArrow => GetClosestEmptyPoint (coordinates, IntPoint.EmptyX(-1)),
                    ConsoleKey.DownArrow => GetClosestEmptyPoint(coordinates, IntPoint.EmptyX(1)),
                    ConsoleKey.LeftArrow => GetClosestEmptyPoint (coordinates, IntPoint.EmptyY(-1)),
                    ConsoleKey.RightArrow => GetClosestEmptyPoint(coordinates, IntPoint.EmptyY(1)),
                    _ => coordinates
                };
                Painter.PaintField(HitsResults.Elements, 
                    Console.WindowHeight / 2 - 10, Console.WindowWidth / 2 - 10 - 20, Array.Empty<string>(), coordinates);
                key = Console.ReadKey().Key;
            }

            return coordinates;
        }
    }
}
