using System;
using System.Collections.Generic;
using System.Text;

namespace Application.NReinas;
public class NReinasUseCase : INReinasUseCase
{
    private List<int[]>?soluciones;

    public List<int[]> Resolver(int n)
    {
        soluciones = new List<int[]>();
        int[] tablero = new int[n];
        Backtracking(tablero, 0, n);
        return soluciones;
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