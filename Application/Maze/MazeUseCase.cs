using System.Collections.Generic;
using Domain.Maze;

namespace Application.Maze;

public class MazeUseCase : IMazeUseCase
{
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
}
