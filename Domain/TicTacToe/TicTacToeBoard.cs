namespace Domain.TicTacToe;

public class TicTacToeBoard
{
    public Player[,] Grid { get; private set; }

    public TicTacToeBoard()
    {
        Grid = new Player[3, 3];
    }

    public bool IsValidMove(int row, int col)
    {
        return row >= 0 && row < 3 && col >= 0 && col < 3 && Grid[row, col] == Player.None;
    }

    public void MakeMove(int row, int col, Player player)
    {
        if (IsValidMove(row, col))
        {
            Grid[row, col] = player;
        }
    }

    public void UndoMove(int row, int col)
    {
        Grid[row, col] = Player.None;
    }

    public GameStatus CheckStatus()
    {
        for (int lineIndex = 0; lineIndex < 3; lineIndex++)
        {
            if (Grid[lineIndex, 0] != Player.None && Grid[lineIndex, 0] == Grid[lineIndex, 1] && Grid[lineIndex, 1] == Grid[lineIndex, 2])
                return Grid[lineIndex, 0] == Player.X ? GameStatus.XWins : GameStatus.OWins;

            if (Grid[0, lineIndex] != Player.None && Grid[0, lineIndex] == Grid[1, lineIndex] && Grid[1, lineIndex] == Grid[2, lineIndex])
                return Grid[0, lineIndex] == Player.X ? GameStatus.XWins : GameStatus.OWins;
        }

        if (Grid[0, 0] != Player.None && Grid[0, 0] == Grid[1, 1] && Grid[1, 1] == Grid[2, 2])
            return Grid[0, 0] == Player.X ? GameStatus.XWins : GameStatus.OWins;

        if (Grid[0, 2] != Player.None && Grid[0, 2] == Grid[1, 1] && Grid[1, 1] == Grid[2, 0])
            return Grid[0, 2] == Player.X ? GameStatus.XWins : GameStatus.OWins;

        foreach (var cell in Grid)
        {
            if (cell == Player.None)
                return GameStatus.InProgress;
        }

        return GameStatus.Draw;
    }
}
