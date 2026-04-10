using Domain.ProblemaViajero;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ProblemaViajero;
// La implementación concreta de la interfaz
public class ViajeroUseCase : IViajeroUseCase
{
    private int _n;
    private int[,] _distancias = new int[0, 0];
    private int _mejorDistancia;
    private List<int> _mejorRuta = new List<int>();

    public ViajeroResult CalcularMejorRuta(int[,] matrizDistancias)
    {
        _distancias = matrizDistancias;
        _n = matrizDistancias.GetLength(0);
        _mejorDistancia = int.MaxValue;
        _mejorRuta = new List<int>();

        var rutaInicial = new List<int> { 0 };
        ExplorarRutas(0, 1, rutaInicial, 0);

        var pasos = new List<string>();
        for (int i = 0; i < _mejorRuta.Count - 1; i++)
        {
            int origen = _mejorRuta[i];
            int destino = _mejorRuta[i + 1];
            pasos.Add($"- Tramo {i + 1}: De {(char)('A' + origen)} a {(char)('A' + destino)} = {_distancias[origen, destino]} unid.");
        }

        return new ViajeroResult
        {
            RutaOptima = _mejorRuta,
            DistanciaTotal = _mejorDistancia,
            DesglosePasos = pasos
        };
    }

    private void ExplorarRutas(int ciudadActual, int ciudadesVisitadas, List<int> rutaActual, int distanciaActual)
    {
        // PODA
        if (distanciaActual >= _mejorDistancia) return;

        // CASO BASE
        if (ciudadesVisitadas == _n)
        {
            _mejorDistancia = distanciaActual;
            _mejorRuta = new List<int>(rutaActual);
            return;
        }

        // RAMIFICACIÓN
        for (int siguienteCiudad = 0; siguienteCiudad < _n; siguienteCiudad++)
        {
            if (!rutaActual.Contains(siguienteCiudad))
            {
                rutaActual.Add(siguienteCiudad);

                ExplorarRutas(
                    siguienteCiudad,
                    ciudadesVisitadas + 1,
                    rutaActual,
                    distanciaActual + _distancias[ciudadActual, siguienteCiudad]
                );

                rutaActual.RemoveAt(rutaActual.Count - 1);
            }
        }
    }
}