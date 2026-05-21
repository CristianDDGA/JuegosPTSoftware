using System.Collections.Generic;

namespace Domain.ProblemaViajero;

public class ViajeroResult
{
    public List<int> RutaOptima { get; set; }
    public int DistanciaTotal { get; set; }
    public List<string> DesglosePasos { get; set; }

    public ViajeroResult()
    {
        RutaOptima = new List<int>();
        DesglosePasos = new List<string>();
        DistanciaTotal = int.MaxValue;
    }
}
