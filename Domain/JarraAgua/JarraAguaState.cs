namespace Domain.JarraAgua;

public record JarraAguaState(int JarraA, int JarraB)
{
    public override string ToString() => $"A: {JarraA}L, B: {JarraB}L";
}
