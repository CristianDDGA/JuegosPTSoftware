namespace Domain.ProblemaCaballo;

public class KnightBoard
{
    public int BoardSize { get; }
    private readonly int[,] _board;

    public static readonly int[] RowMoves = { -2, -1, 1, 2, 2, 1, -1, -2 };
    public static readonly int[] ColumnMoves = { 1, 2, 2, 1, -1, -2, -2, -1 };

    public KnightBoard(int boardSize)
    {
        BoardSize = boardSize;
        _board = new int[boardSize, boardSize];

        for (int row = 0; row < boardSize; row++)
            for (int column = 0; column < boardSize; column++)
                _board[row, column] = -1;
    }

    public int GetValue(int row, int column) => _board[row, column];

    public void SetValue(int row, int column, int value) => _board[row, column] = value;

    public bool IsValidMove(int row, int column)
    {
        return row >= 0 && row < BoardSize &&
               column >= 0 && column < BoardSize &&
               _board[row, column] == -1;
    }

    public int[,] GetBoard() => _board;
}
