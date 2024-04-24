using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services;

public interface IPlanesService
{
    Task CrearPlan(Plan plan);
    Task EditarPlan(Plan plan);
    Task EliminarPlan(int id);
    bool PlanExists(int id);
}