namespace Domain.ShortestPath;

public class Distance
{
    public double Kilometers { get; }

    public Distance(double kilometers)
    {
        if (kilometers < 0) throw new ArgumentException("Distance cannot be negative", nameof(kilometers));
        Kilometers = kilometers;
    }

    public static Distance operator +(Distance first, Distance second) =>
        new Distance(first.Kilometers + second.Kilometers);
}
