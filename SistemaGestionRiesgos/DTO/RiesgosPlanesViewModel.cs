using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.DTO;

public class RiesgosPlanesViewModel
{
    public Riesgo Riesgo { get; set; }
    public List<Plan> Planes { get; set; } = new List<Plan>();
}