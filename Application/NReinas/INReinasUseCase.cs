using Domain.NReinas;

namespace Application.NReinas;
public interface INReinasUseCase
{
    List<NReinasSolution> Resolver(int n);
}
