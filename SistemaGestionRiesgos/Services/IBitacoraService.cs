using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services;

public interface IBitacoraService
{
    Task<List<Bitacora>> ListaBitacoras();
    Task CrearBitacora(Bitacora bitacora);
}