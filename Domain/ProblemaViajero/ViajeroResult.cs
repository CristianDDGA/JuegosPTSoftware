using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.ProblemaViajero;
// Esta clase solo representa el resultado final
public class ViajeroResult
{
    public List<int> RutaOptima { get; set; }
    public int DistanciaTotal { get; set; }
    public List<string> DesglosePasos { get; set; }

    // NUEVO: Aquí guardaremos el string de la matriz ya dibujada
    public string MapaVisual { get; set; }

    public ViajeroResult()
    {
        RutaOptima = new List<int>();
        DesglosePasos = new List<string>();
        DistanciaTotal = int.MaxValue;
        MapaVisual = string.Empty;
    }
}


