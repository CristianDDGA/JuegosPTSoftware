using System.Collections.Generic;

namespace Application.Maze;

public interface IMazeUseCase
{
    List<(int, int)> FindPathBFS(Domain.Maze.MazeBoard board, (int, int) start, (int, int) end);
}
