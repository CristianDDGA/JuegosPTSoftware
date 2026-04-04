namespace Domain.TicTacToe;

public class TicTacToeBoard
{

    // Matriz 3x3 que representa el tablero
    public Player[,] Grid { get; private set; }

    public TicTacToeBoard()
    {
        Grid = new Player[3, 3];
        // En C#, los enums se inicializan por defecto en su primer valor (Player.None)
    }

    // Valida si las coordenadas están dentro del tablero y la casilla está vacía
    public bool IsValidMove(int row, int col)
    {
        return row >= 0 && row < 3 && col >= 0 && col < 3 && Grid[row, col] == Player.None;
    }

    // Coloca la 'X' o la 'O' en el tablero
    public void MakeMove(int row, int col, Player player)
    {
        if (IsValidMove(row, col))
        {
            Grid[row, col] = player;
        }
    }

    // Deshace un movimiento (Esto será CRUCIAL para el algoritmo Minimax)
    public void UndoMove(int row, int col)
    {
        Grid[row, col] = Player.None;
    }

    // Evalúa si alguien ganó, si hay empate o si el juego continúa
    public GameStatus CheckStatus()
    {
        // 1. Revisar filas y columnas
        for (int i = 0; i < 3; i++)
        {
            if (Grid[i, 0] != Player.None && Grid[i, 0] == Grid[i, 1] && Grid[i, 1] == Grid[i, 2])
                return Grid[i, 0] == Player.X ? GameStatus.XWins : GameStatus.OWins;

            if (Grid[0, i] != Player.None && Grid[0, i] == Grid[1, i] && Grid[1, i] == Grid[2, i])
                return Grid[0, i] == Player.X ? GameStatus.XWins : GameStatus.OWins;
        }

        // 2. Revisar diagonales
        if (Grid[0, 0] != Player.None && Grid[0, 0] == Grid[1, 1] && Grid[1, 1] == Grid[2, 2])
            return Grid[0, 0] == Player.X ? GameStatus.XWins : GameStatus.OWins;

        if (Grid[0, 2] != Player.None && Grid[0, 2] == Grid[1, 1] && Grid[1, 1] == Grid[2, 0])
            return Grid[0, 2] == Player.X ? GameStatus.XWins : GameStatus.OWins;

        // 3. Revisar si hay empate (tablero lleno)
        foreach (var cell in Grid)
        {
            if (cell == Player.None)
                return GameStatus.InProgress;
        }

        return GameStatus.Draw;
    }

}
