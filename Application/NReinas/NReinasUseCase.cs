using System;
using System.Collections.Generic;
using Domain.NReinas;

namespace Application.NReinas;
public class NReinasUseCase : INReinasUseCase
{
    private List<int[]> soluciones = new List<int[]>();

    public List<NReinasSolution> Resolver(int n)
    {
        soluciones = new List<int[]>();
        int[] tablero = new int[n];
        Backtracking(tablero, 0, n);
        var result = new List<NReinasSolution>(soluciones.Count);
        foreach (var solucion in soluciones)
        {
            result.Add(new NReinasSolution((int[])solucion.Clone()));
        }

        return result;
    }

    private void Backtracking(int[] tablero, int fila, int n)
    {
        if (fila == n)
        {
            soluciones.Add((int[])tablero.Clone());
            return;
        }

        for (int col = 0; col < n; col++)
        {
            if (EsSeguro(tablero, fila, col))
            {
                tablero[fila] = col;
                Backtracking(tablero, fila + 1, n);
            }
        }
    }

    private bool EsSeguro(int[] tablero, int fila, int col)
    {
        for (int i = 0; i < fila; i++)
        {
            int columnaOcupada = tablero[i];
            // Misma columna o diagonales
            if (columnaOcupada == col ||
                Math.Abs(columnaOcupada - col) == Math.Abs(i - fila))
            {
                return false;
            }
        }
        return true;
    }
}