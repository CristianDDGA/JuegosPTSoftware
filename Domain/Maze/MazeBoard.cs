namespace Domain.Maze;

public class MazeBoard
{
    private readonly int[,] _grid;

    public MazeBoard(int[,] grid)
    {
        _grid = grid;
    }

    public int Rows => _grid.GetLength(0);
    public int Cols => _grid.GetLength(1);

    public int GetValue(int row, int col) => _grid[row, col];

    public bool IsValidMove(int row, int col)
    {
        return row >= 0 && row < Rows && col >= 0 && col < Cols && _grid[row, col] == 0;
    }
}
