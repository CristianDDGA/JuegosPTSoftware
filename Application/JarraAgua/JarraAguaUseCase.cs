using System.Collections.Generic;
using Domain.JarraAgua;

namespace Application.JarraAgua;

public class JarraAguaUseCase : IJarraAguaUseCase
{
    private const int CapA = 4;
    private const int CapB = 3;

    public List<JarraAguaState> Resolver(int objetivo = 2)
    {
        var visitados = new HashSet<(int, int)>();
        var queue = new Queue<List<JarraAguaState>>();
        var inicial = new JarraAguaState(0, 0);
        queue.Enqueue(new List<JarraAguaState> { inicial });
        visitados.Add((0, 0));

        while (queue.Count > 0)
        {
            var camino = queue.Dequeue();
            var actual = camino[^1];

            if (actual.JarraA == objetivo || actual.JarraB == objetivo)
                return camino;

            foreach (var siguiente in GenerarSiguientes(actual))
            {
                if (!visitados.Contains((siguiente.JarraA, siguiente.JarraB)))
                {
                    visitados.Add((siguiente.JarraA, siguiente.JarraB));
                    var nuevoCamino = new List<JarraAguaState>(camino) { siguiente };
                    queue.Enqueue(nuevoCamino);
                }
            }
        }
        return new List<JarraAguaState>(); // No hay solución
    }

    private IEnumerable<JarraAguaState> GenerarSiguientes(JarraAguaState estado)
    {
        // Llenar A
        yield return new JarraAguaState(CapA, estado.JarraB);
        // Llenar B
        yield return new JarraAguaState(estado.JarraA, CapB);
        // Vaciar A
        yield return new JarraAguaState(0, estado.JarraB);
        // Vaciar B
        yield return new JarraAguaState(estado.JarraA, 0);
        // Transferir A -> B
        int transferAB = System.Math.Min(estado.JarraA, CapB - estado.JarraB);
        yield return new JarraAguaState(estado.JarraA - transferAB, estado.JarraB + transferAB);
        // Transferir B -> A
        int transferBA = System.Math.Min(estado.JarraB, CapA - estado.JarraA);
        yield return new JarraAguaState(estado.JarraA + transferBA, estado.JarraB - transferBA);
    }
}
