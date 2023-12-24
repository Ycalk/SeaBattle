using static SeaBattle.Field.FieldCreator;

namespace SeaBattle
{
    internal class Game
    {
        private readonly Player[] _players = new Player[2];

        public Game(Field playerField)
        {
            _players[0] = new Player(Player.Type.Human, playerField, GetEmptyField());
            _players[1] = new Player(Player.Type.Computer, GetRandomField(), GetEmptyField());
        }

        public void Start()
        {
            var cursor = new IntPoint(0, 0);
            for (var i = 0; !GameEnded(); i++)
            {
                
                Painter.PaintPlayerFields(_players[0].Field.Elements, _players[0].HitsResults.Elements, cursor);
                var currentPlayer = _players[i % 2];
                var enemy = _players[(i + 1) % 2];

                i += Convert.ToInt32(currentPlayer.MakeMove(enemy, ref cursor));
            }
            GameEnd();
        }

        private void GameEnd()
        {
            Console.Clear();
            if (_players[1].Field.IsAllShipsDead())
                Painter.PaintCenteredMessage("You Win", ConsoleColor.Black, ConsoleColor.Green);
            else
                Painter.PaintCenteredMessage("You Lose", ConsoleColor.Black, ConsoleColor.Red);
            
        }

        private bool GameEnded() =>
            _players.Any(player => player.Field.IsAllShipsDead());

    }
}
