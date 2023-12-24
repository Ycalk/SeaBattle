using SeaBattle;

// Intro
Painter.PaintCenteredMessage("Sea Battle", ConsoleColor.Cyan, ConsoleColor.Black);
Console.ReadKey();
Console.Clear();

var messages = new[]
{
    "Press Enter to confirm field",
    "Press any other key to randomize field"
};

var playerField = Field.FieldCreator.GetRandomField();
Painter.PaintField(playerField.Elements, Console.WindowHeight / 2 - 10, Console.WindowWidth / 2 - 10, messages);

while (Console.ReadKey().Key != ConsoleKey.Enter)
{
    playerField = Field.FieldCreator.GetRandomField();
    Painter.PaintField(playerField.Elements, Console.WindowHeight / 2 - 10, Console.WindowWidth / 2 - 10, messages);
}
Console.Clear();

var game = new Game(playerField);
game.Start();
Console.ReadKey();