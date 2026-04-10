using System.Collections.Generic;
using Domain.Maze;

namespace Application.Maze;

public class MazeUseCase : IMazeUseCase
{
    public (MazeBoard board, (int row, int col) start, (int row, int col) end) GeneratePlayableMaze(int rows, int cols)
    {
        if (rows % 2 == 0) rows++;
        if (cols % 2 == 0) cols++;

        int[,] grid = new int[rows, cols];
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                grid[r, c] = 1;

        var rnd = new Random();
        CarvePassagesFrom(1, 1, grid, rnd);

        int wallsToRemove = (rows * cols) / 20;
        while (wallsToRemove > 0)
        {
            int r = rnd.Next(1, rows - 1);
            int c = rnd.Next(1, cols - 1);
            if (grid[r, c] == 1 && r > 1 && r < rows - 2 && c > 1 && c < cols - 2)
            {
                grid[r, c] = 0;
                wallsToRemove--;
            }
        }

        var start = (row: 1, col: 1);
        var end = (row: rows - 2, col: cols - 2);
        grid[end.row, end.col] = 0;

        return (new MazeBoard(grid), start, end);
    }

    public List<(int, int)> FindPathBFS(MazeBoard board, (int, int) start, (int, int) end)
    {
        var queue = new Queue<(int r, int c)>();
        var parentMap = new Dictionary<(int, int), (int, int)>();
        var visited = new HashSet<(int, int)>();

        queue.Enqueue(start);
        visited.Add(start);

        int[] dRow = { -1, 1, 0, 0 };
        int[] dCol = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == end)
            {
                var path = new List<(int, int)>();
                var step = end;

                while (step != start)
                {
                    path.Add(step);
                    step = parentMap[step];
                }
                path.Add(start);
                path.Reverse();

                return path;
            }

            for (int i = 0; i < 4; i++)
            {
                int nextRow = current.r + dRow[i];
                int nextCol = current.c + dCol[i];
                var nextPos = (nextRow, nextCol);

                if (board.IsValidMove(nextRow, nextCol) && !visited.Contains(nextPos))
                {
                    visited.Add(nextPos);
                    queue.Enqueue(nextPos);
                    parentMap[nextPos] = current;
                }
            }
        }

        return new List<(int, int)>(); // Path not found
    }

    private static void CarvePassagesFrom(int r, int c, int[,] grid, Random rnd)
    {
        grid[r, c] = 0;

        int[] dr = { -2, 2, 0, 0 };
        int[] dc = { 0, 0, -2, 2 };
        int[] dirs = { 0, 1, 2, 3 };

        for (int i = dirs.Length - 1; i > 0; i--)
        {
            int j = rnd.Next(i + 1);
            (dirs[i], dirs[j]) = (dirs[j], dirs[i]);
        }

        foreach (int dir in dirs)
        {
            int nr = r + dr[dir];
            int nc = c + dc[dir];
            if (nr > 0 && nr < grid.GetLength(0) - 1 && nc > 0 && nc < grid.GetLength(1) - 1 && grid[nr, nc] == 1)
            {
                grid[r + (dr[dir] / 2), c + (dc[dir] / 2)] = 0;
                CarvePassagesFrom(nr, nc, grid, rnd);
            }
        }
    }
}
