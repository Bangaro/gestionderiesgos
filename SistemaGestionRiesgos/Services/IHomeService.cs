using SistemaGestionRiesgos.DTO;

namespace SistemaGestionRiesgos.Services;

public interface IHomeService
{
    Task<List<RiesgosPlanesViewModel>> FiltrarRiesgos(string Impacto, string Probabilidad);
    Task<List<RiesgosPlanesViewModel>> BuscarRiesgos(string Titulo);
}