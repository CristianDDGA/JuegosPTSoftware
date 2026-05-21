using Domain.ProblemaViajero;
using System;
using System.Collections.Generic;

namespace Application.ProblemaViajero;

public class ViajeroUseCase : IViajeroUseCase
{
    private int _cityCount;
    private int[,]? _distances;
    private int _bestDistance;
    private List<int>? _bestRoute;

    public ViajeroResult CalcularMejorRuta(int[,] distanceMatrix)
    {
        _distances = distanceMatrix;
        _cityCount = distanceMatrix.GetLength(0);
        _bestDistance = int.MaxValue;
        _bestRoute = new List<int>();

        var initialRoute = new List<int> { 0 };
        ExploreRoutes(0, 1, initialRoute, 0);

        var steps = new List<string>();
        for (int stepIndex = 0; stepIndex < _bestRoute.Count - 1; stepIndex++)
        {
            int origin = _bestRoute[stepIndex];
            int destination = _bestRoute[stepIndex + 1];
            steps.Add($"- Tramo {stepIndex + 1}: De {(char)('A' + origin)} a {(char)('A' + destination)} = {_distances[origin, destination]} unid.");
        }

        return new ViajeroResult
        {
            RutaOptima = _bestRoute,
            DistanciaTotal = _bestDistance,
            DesglosePasos = steps
        };
    }

    private void ExploreRoutes(int currentCity, int visitedCities, List<int> currentRoute, int currentDistance)
    {
        if (currentDistance >= _bestDistance) return;

        if (visitedCities == _cityCount)
        {
            _bestDistance = currentDistance;
            _bestRoute = new List<int>(currentRoute);
            return;
        }

        var candidates = new List<int>();
        for (int city = 0; city < _cityCount; city++)
        {
            if (!currentRoute.Contains(city))
            {
                candidates.Add(city);
            }
        }

        candidates.Sort((first, second) => _distances[currentCity, first].CompareTo(_distances[currentCity, second]));

        foreach (int nextCity in candidates)
        {
            currentRoute.Add(nextCity);

            ExploreRoutes(
                nextCity,
                visitedCities + 1,
                currentRoute,
                currentDistance + _distances[currentCity, nextCity]
            );

            currentRoute.RemoveAt(currentRoute.Count - 1);
        }
    }
}
