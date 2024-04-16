using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services;

public interface IRiesgosService
{
    Task<List<RiesgosPlanesViewModel>> FiltrarRiesgos(string Impacto, string Probabilidad);
    Task<List<RiesgosPlanesViewModel>> BuscarRiesgos(string Titulo);
    Task<RiesgosPlanesViewModel> SeleccionarRiesgo(int Riesgo, string TipoPlan);
    void CrearRiesgo(Riesgo riesgo);
}