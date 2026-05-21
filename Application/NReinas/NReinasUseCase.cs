using System;
using System.Collections.Generic;
using Domain.NReinas;

namespace Application.NReinas;

public class NReinasUseCase : INReinasUseCase
{
    private List<int[]>? _solutions;

    public List<int[]> Resolver(int boardSize)
    {
        _solutions = new List<int[]>();
        var board = new NReinasBoard(boardSize);
        SolveBacktracking(board, 0, boardSize);
        return _solutions;
    }

    private void SolveBacktracking(NReinasBoard board, int currentRow, int boardSize)
    {
        if (currentRow == boardSize)
        {
            _solutions.Add(board.CloneBoard());
            return;
        }

        for (int column = 0; column < boardSize; column++)
        {
            if (board.IsSafe(currentRow, column))
            {
                board.PlaceQueen(currentRow, column);
                SolveBacktracking(board, currentRow + 1, boardSize);
            }
        }
    }
}
