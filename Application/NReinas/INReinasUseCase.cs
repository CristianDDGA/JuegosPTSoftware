using System;
using System.Collections.Generic;
using System.Text;

namespace Application.NReinas;
public interface INReinasUseCase
{
    List<int[]> Resolver(int n);
}
