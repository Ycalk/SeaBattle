using static SeaBattle.Field;
namespace SeaBattle
{
    internal static class Painter
    {
        public static void PaintBackground(char back)
        {
            for (var i = 0; i < Console.WindowWidth; i++)
                for (var j = 0; j < Console.WindowHeight; j++)
                    Console.Write(back);
        }

        public static void PaintCenteredMessage(string message, ConsoleColor background, ConsoleColor foreground)
        {
            Console.BackgroundColor = background;
            PaintBackground(' ');
            Console.ForegroundColor = foreground;
            Console.SetCursorPosition(
                Console.WindowWidth / 2 - message.Length / 2,
                Console.WindowHeight / 2);
            Console.Write(message);
            Console.ResetColor();
        }

        public static void PaintPlayerFields(FieldElements[,] field, FieldElements[,] hitsResults, IntPoint dedicated)
        {
            var messages = new[]
            {
                "Press Space to confirm cell",
                "Press Arrows to move cursor",
            };
            PaintField(hitsResults, Console.WindowHeight / 2 - 10, Console.WindowWidth / 2 - 10 - 20, messages, dedicated);
            PaintField(field, Console.WindowHeight / 2 - 10, Console.WindowWidth / 2 - 10 + 20, Array.Empty<string>());
        }

        public static void PaintField(FieldElements[,] field, int topIndent,
            int leftIndent, string[] messages, IntPoint dedicated = new())
        {
            for (var i = 0; i < 10; i++)
            for (var j = 0; j < 10; j++)
            {
                if (i == dedicated.X && j == dedicated.Y && dedicated.NotEmpty)
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                if (field[i, j] == FieldElements.Ship)
                {
                    Console.SetCursorPosition(leftIndent + i * 2,
                        topIndent + j);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("*" + " ");
                }
                else if (field[i, j] == FieldElements.Miss)
                {
                    Console.SetCursorPosition(leftIndent + i * 2,
                        topIndent + j);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write("~" + " ");
                }

                else if (field[i, j] == FieldElements.Hit)
                {
                    Console.SetCursorPosition(leftIndent + i * 2,
                        topIndent + j);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("X" + " ");
                }
                else if (field[i, j] == FieldElements.Dead)
                {
                    Console.SetCursorPosition(leftIndent + i * 2,
                                               topIndent + j);
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.Write(" " + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else if (field[i, j] == FieldElements.Empty)
                {
                    Console.SetCursorPosition(leftIndent + i * 2,
                        topIndent + j);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("·" + " ");
                }
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.WriteLine();
            Console.ResetColor();
            for (var i = 0; i < messages.Length; i++)
            {
                Console.SetCursorPosition(leftIndent - messages[i].Length / 2 + 10, topIndent + 10 + 2 + i);
                Console.Write(messages[i]);
            }
        }
    }
    

}
