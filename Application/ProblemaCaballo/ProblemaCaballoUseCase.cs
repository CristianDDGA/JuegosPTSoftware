using System;
using System.Collections.Generic;
using Domain.ProblemaCaballo;

namespace Application.ProblemaCaballo;

public class ProblemaCaballoUseCase : IProblemaCaballoUseCase
{
    private readonly int[] dr = new int[] { -2, -1, 1, 2, 2, 1, -1, -2 };
    private readonly int[] dc = new int[] { 1, 2, 2, 1, -1, -2, -2, -1 };

    public int[,] Resolver(int n, int startRow, int startCol)
    {
        int[,] board = new int[n, n];
        for (int r = 0; r < n; r++)
            for (int c = 0; c < n; c++)
                board[r, c] = -1;

        board[startRow, startCol] = 0;

        if (Backtrack(board, startRow, startCol, 1, n))
            return board;

        return null!; // no solution
    }

    public List<KnightMove> ObtenerMovimientosValidos(int n, int[,] estadoTablero, int row, int col)
    {
        var moves = new List<KnightMove>();
        for (int k = 0; k < 8; k++)
        {
            int nr = row + dr[k];
            int nc = col + dc[k];
            if (nr >= 0 && nr < n && nc >= 0 && nc < n && estadoTablero[nr, nc] < 0)
            {
                moves.Add(new KnightMove(nr, nc));
            }
        }

        return moves;
    }

    private bool IsValid(int r, int c, int n, int[,] board)
    {
        return r >= 0 && r < n && c >= 0 && c < n && board[r, c] == -1;
    }

    private bool Backtrack(int[,] board, int r, int c, int moveIndex, int n)
    {
        if (moveIndex == n * n)
            return true;

        // Warnsdorff heuristic: sort moves by number of onward moves
        var moves = new List<(int nr, int nc, int degree)>();
        for (int k = 0; k < 8; k++)
        {
            int nr = r + dr[k];
            int nc = c + dc[k];
            if (IsValid(nr, nc, n, board))
            {
                int deg = CountOnwardMoves(nr, nc, n, board);
                moves.Add((nr, nc, deg));
            }
        }

        moves.Sort((a, b) => a.degree.CompareTo(b.degree));

        foreach (var m in moves)
        {
            board[m.nr, m.nc] = moveIndex;
            if (Backtrack(board, m.nr, m.nc, moveIndex + 1, n))
                return true;
            board[m.nr, m.nc] = -1;
        }

        return false;
    }

    private int CountOnwardMoves(int r, int c, int n, int[,] board)
    {
        int count = 0;
        for (int k = 0; k < 8; k++)
        {
            int nr = r + dr[k];
            int nc = c + dc[k];
            if (IsValid(nr, nc, n, board)) count++;
        }
        return count;
    }
}
