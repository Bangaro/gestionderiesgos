using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services;

public interface IRiesgosService
{
    Task<List<RiesgosPlanesViewModel>> FiltrarRiesgos(string Impacto, string Probabilidad);
    Task<List<RiesgosPlanesViewModel>> BuscarRiesgos(string Titulo);
    Task<RiesgosPlanesViewModel> SeleccionarRiesgo(int Riesgo, string TipoPlan);
    Task CrearRiesgo(Riesgo riesgo);
    Task EditarRiesgo(Riesgo riesgo);
    Task EliminarRiesgo(int id);
    bool RiesgoExists(int id);
}