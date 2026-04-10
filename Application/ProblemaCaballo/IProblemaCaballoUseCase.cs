using System;
using System.Collections.Generic;
using Domain.ProblemaCaballo;

namespace Application.ProblemaCaballo;

public interface IProblemaCaballoUseCase
{
    int[,] Resolver(int n, int startRow, int startCol);
    List<KnightMove> ObtenerMovimientosValidos(int n, int[,] estadoTablero, int row, int col);
}
