using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services;

public interface IRiesgosService
{
    Task<RiesgosPlanesViewModel> SeleccionarRiesgo(int Riesgo, string TipoPlan);
    void CrearRiesgo(Riesgo riesgo);
}