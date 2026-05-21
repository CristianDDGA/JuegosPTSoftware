using System.Collections.Generic;

namespace Application.Maze;

public interface IMazeUseCase
{
    List<(int row, int column)> FindPathBFS(Domain.Maze.MazeBoard board, (int row, int column) start, (int row, int column) end);
}
