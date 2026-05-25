using System.Collections.Generic;
using Domain.JarraAgua;

namespace Application.JarraAgua;

public interface IJarraAguaUseCase
{
    List<JarraAguaState> Resolver(int objetivo = 2);
}
