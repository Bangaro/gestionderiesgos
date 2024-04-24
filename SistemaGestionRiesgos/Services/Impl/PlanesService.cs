using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services.Impl;

public class PlanesService: IPlanesService
{
    private readonly GestionDbContext _context;
    private readonly IBitacoraService _bitacoraService;
    private readonly IUsuariosService _usuariosService;


    public PlanesService(GestionDbContext context, IBitacoraService bitacoraService, IUsuariosService usuariosService)
    {
        _context = context;
        _bitacoraService = bitacoraService;
        _usuariosService = usuariosService;
    }
    public async Task CrearPlan(Plan plan)
    {
        // Obtener el usuario actual desde el contexto de HTTP
        Usuario? user = await _usuariosService.ObtenerUsuarioConectado();

        plan.IdUsuario = user.IdUsuario;
        
        _context.Add(plan);
        await _context.SaveChangesAsync();

        var bitacora = new Bitacora
        {
            Descripcion = "" + plan.Descripcion,
            IdUsuario = plan.IdUsuario,
            Tabla = "Planes",
            TipoAccion = "Crear"
        };
        await _bitacoraService.CrearBitacora(bitacora);
    }

    public async Task EditarPlan(Plan plan)
    {
        var planActual = _context.Planes.FirstOrDefault(p => p.IdPlan == plan.IdPlan);

        planActual.IdUsuario = _usuariosService.ObtenerUsuarioConectado().Id;
        
        planActual.TipoPlan = plan.TipoPlan;
        planActual.Descripcion = plan.Descripcion;
        
        var bitacora = new Bitacora
        {
            Descripcion = "" + plan.Descripcion,
            IdUsuario = plan.IdUsuario,
            Tabla = "Planes",
            TipoAccion = "Editar"
        };
        await _bitacoraService.CrearBitacora(bitacora);
        
        _context.Update(planActual);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarPlan(int id)
    {
        var plan = _context.Planes.FirstOrDefault(p => p.IdPlan == id);

        if (plan == null) throw new InvalidOperationException("Sucedió un error al eliminar el riesgo.");
        
        var bitacora = new Bitacora
        {
            Descripcion = "" + plan.Descripcion,
            IdUsuario = plan.IdUsuario,
            Tabla = "Planes",
            TipoAccion = "Eliminar"
        };
        await _bitacoraService.CrearBitacora(bitacora);
        
        _context.Planes.Remove(plan);
        await _context.SaveChangesAsync();
    }

    public bool PlanExists(int id)
    {
        return _context.Planes.Any(p => p.IdPlan == id);
    }
}