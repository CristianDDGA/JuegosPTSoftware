namespace Domain.NReinas;

public class NReinasBoard
{
    public int BoardSize { get; }
    private readonly int[] _board;

    public NReinasBoard(int boardSize)
    {
        BoardSize = boardSize;
        _board = new int[boardSize];
    }

    public int GetQueenColumn(int row) => _board[row];

    public void PlaceQueen(int row, int column) => _board[row] = column;

    public bool IsSafe(int row, int column)
    {
        for (int previousRow = 0; previousRow < row; previousRow++)
        {
            int occupiedColumn = _board[previousRow];

            if (occupiedColumn == column ||
                Math.Abs(occupiedColumn - column) == Math.Abs(previousRow - row))
            {
                return false;
            }
        }
        return true;
    }

    public int[] CloneBoard() => (int[])_board.Clone();
}
