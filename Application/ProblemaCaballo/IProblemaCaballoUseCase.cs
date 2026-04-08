using System;

namespace Application.ProblemaCaballo;

public interface IProblemaCaballoUseCase
{
    int[,] Resolver(int n, int startRow, int startCol);
}
