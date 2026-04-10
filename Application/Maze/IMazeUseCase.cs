using System.Collections.Generic;
using Domain.Maze;

namespace Application.Maze;

public interface IMazeUseCase
{
    (MazeBoard board, (int row, int col) start, (int row, int col) end) GeneratePlayableMaze(int rows, int cols);
    List<(int, int)> FindPathBFS(MazeBoard board, (int, int) start, (int, int) end);
}
