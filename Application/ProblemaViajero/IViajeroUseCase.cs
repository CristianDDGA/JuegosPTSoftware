using Domain.ProblemaViajero;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.ProblemaViajero;

public interface IViajeroUseCase
{
    // El contrato que obliga a implementar este método
    ViajeroResult CalcularMejorRuta(int[,] matrizDistancias);
}

