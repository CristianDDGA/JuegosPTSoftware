using System;
using System.Collections.Generic;
using Domain.ProblemaCaballo;

namespace Application.ProblemaCaballo;

public class ProblemaCaballoUseCase : IProblemaCaballoUseCase
{
    public int[,] Resolver(int boardSize, int startRow, int startColumn)
    {
        var board = new KnightBoard(boardSize);
        board.SetValue(startRow, startColumn, 0);

        if (SolveBacktracking(board, startRow, startColumn, 1, boardSize))
            return board.GetBoard();

        return null!;
    }

    private bool SolveBacktracking(KnightBoard board, int currentRow, int currentColumn, int moveIndex, int boardSize)
    {
        if (moveIndex == boardSize * boardSize)
            return true;

        var moves = new List<(int nextRow, int nextColumn, int degree)>();
        for (int directionIndex = 0; directionIndex < 8; directionIndex++)
        {
            int nextRow = currentRow + KnightBoard.RowMoves[directionIndex];
            int nextColumn = currentColumn + KnightBoard.ColumnMoves[directionIndex];

            if (board.IsValidMove(nextRow, nextColumn))
            {
                int degree = CountOnwardMoves(board, nextRow, nextColumn, boardSize);
                moves.Add((nextRow, nextColumn, degree));
            }
        }

        moves.Sort((first, second) => first.degree.CompareTo(second.degree));

        foreach (var move in moves)
        {
            board.SetValue(move.nextRow, move.nextColumn, moveIndex);
            if (SolveBacktracking(board, move.nextRow, move.nextColumn, moveIndex + 1, boardSize))
                return true;
            board.SetValue(move.nextRow, move.nextColumn, -1);
        }

        return false;
    }

    private int CountOnwardMoves(KnightBoard board, int row, int column, int boardSize)
    {
        int count = 0;
        for (int directionIndex = 0; directionIndex < 8; directionIndex++)
        {
            int nextRow = row + KnightBoard.RowMoves[directionIndex];
            int nextColumn = column + KnightBoard.ColumnMoves[directionIndex];
            if (board.IsValidMove(nextRow, nextColumn))
                count++;
        }
        return count;
    }
}
