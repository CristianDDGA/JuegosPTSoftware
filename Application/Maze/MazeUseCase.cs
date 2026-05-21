using System.Collections.Generic;
using Domain.Maze;

namespace Application.Maze;

public class MazeUseCase : IMazeUseCase
{
    public List<(int row, int column)> FindPathBFS(MazeBoard board, (int row, int column) start, (int row, int column) end)
    {
        var queue = new Queue<(int row, int column)>();
        var parentMap = new Dictionary<(int row, int column), (int row, int column)>();
        var visited = new HashSet<(int row, int column)>();

        queue.Enqueue(start);
        visited.Add(start);

        int[] rowOffsets = { -1, 1, 0, 0 };
        int[] columnOffsets = { 0, 0, -1, 1 };

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == end)
            {
                var path = new List<(int row, int column)>();
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

            for (int directionIndex = 0; directionIndex < 4; directionIndex++)
            {
                int nextRow = current.row + rowOffsets[directionIndex];
                int nextColumn = current.column + columnOffsets[directionIndex];
                var nextPosition = (nextRow, nextColumn);

                if (board.IsValidMove(nextRow, nextColumn) && !visited.Contains(nextPosition))
                {
                    visited.Add(nextPosition);
                    queue.Enqueue(nextPosition);
                    parentMap[nextPosition] = current;
                }
            }
        }

        return new List<(int row, int column)>();
    }
}
