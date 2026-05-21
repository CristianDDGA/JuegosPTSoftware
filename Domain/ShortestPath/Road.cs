namespace Domain.ShortestPath;

public class Road
{
    public City Origin { get; }
    public City Destination { get; }
    public Distance Distance { get; }

    public Road(City origin, City destination, Distance distance)
    {
        Origin = origin;
        Destination = destination;
        Distance = distance;
    }

    public City GetOtherCity(City city)
    {
        if (city.Equals(Origin)) return Destination;
        if (city.Equals(Destination)) return Origin;
        throw new InvalidOperationException("City not part of this road");
    }
}
