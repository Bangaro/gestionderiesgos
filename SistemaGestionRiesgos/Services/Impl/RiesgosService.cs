
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services.Impl;

public class RiesgosService: IRiesgosService
{
    
    private readonly GestionDbContext _context;
    private readonly IBitacoraService _bitacoraService;
    private readonly IUsuariosService _usuariosService;


    public RiesgosService(GestionDbContext context, IBitacoraService bitacoraService, IUsuariosService usuariosService)
    {
        _context = context;
        _bitacoraService = bitacoraService;
        _usuariosService = usuariosService;
    }
      
    public async Task<List<RiesgosPlanesViewModel>> FiltrarRiesgos(string Impacto, string Probabilidad)
    {
        // Obtener los Riesgos que coinciden con el impacto y la probabilidad
        var riesgos = await _context.Riesgos
            .Where(r => r.Impacto == Impacto && r.Probabilidad == Probabilidad)
            .ToListAsync();

        List<RiesgosPlanesViewModel> listaRiesgosPlanes = new List<RiesgosPlanesViewModel>();

        // Iterar sobre cada Riesgo
        foreach (var riesgo in riesgos)
        {
            // Obtener todos los Planes asociados al Riesgo
            var planes = await _context.Planes.Where(p => p.IdRiesgo == riesgo.IdRiesgo).ToListAsync();

            // Crear un nuevo RiesgosPlanesViewModel y agregarlo a la lista
            var riesgosPlanesViewModel = new RiesgosPlanesViewModel
            {
                Riesgo = riesgo,
                Planes = planes // Asignar la lista de Planes asociados al Riesgo
            };

            listaRiesgosPlanes.Add(riesgosPlanesViewModel);
        }

        // Pasar la lista de RiesgosPlanesViewModel a la vista
        return listaRiesgosPlanes;
    }
    
    
    public async Task<List<RiesgosPlanesViewModel>> BuscarRiesgos(string Titulo)
    {
        // Obtener todos los riesgos o filtrar por título si se proporciona uno
        var riesgosQuery = _context.Riesgos.AsQueryable();

        riesgosQuery = riesgosQuery.Where(r => EF.Functions.Like(r.Titulo, $"%{Titulo}%"));

        var riesgos = await riesgosQuery.ToListAsync();

        // Crear la lista de RiesgosPlanesViewModel
        var listaRiesgosPlanes = new List<RiesgosPlanesViewModel>();

        foreach (var riesgo in riesgos)
        {
            // Obtener todos los Planes asociados al Riesgo
            var planes = await _context.Planes.Where(p => p.IdRiesgo == riesgo.IdRiesgo).ToListAsync();

            // Crear un nuevo RiesgosPlanesViewModel y agregarlo a la lista
            var riesgosPlanesViewModel = new RiesgosPlanesViewModel
            {
                Riesgo = riesgo,
                Planes = planes // Asignar la lista de Planes asociados al Riesgo
            };

            listaRiesgosPlanes.Add(riesgosPlanesViewModel);
        }
        

        return listaRiesgosPlanes;
    }
    
    public async Task<RiesgosPlanesViewModel> SeleccionarRiesgo(int Riesgo, string TipoPlan)
    {
        // Obtener el Riesgo por su Id
        var riesgo = await _context.Riesgos.FirstOrDefaultAsync(r => r.IdRiesgo == Riesgo);

        if (riesgo == null)
        {
            return null;
        }

        // Obtener todos los Planes asociados al Riesgo y al TipoPlan especificado
        var planes = await _context.Planes
            .Where(p => p.IdRiesgo == riesgo.IdRiesgo && p.TipoPlan == TipoPlan)
            .ToListAsync();

        // Crear un nuevo RiesgosPlanesViewModel y agregarlo a la lista
        var riesgosPlanesViewModel = new RiesgosPlanesViewModel
        {
            Riesgo = riesgo,
            Planes = planes // Asignar la lista de Planes asociados al Riesgo
        };

        // Pasar la lista de RiesgosPlanesViewModel a la vista
        return riesgosPlanesViewModel;
    }

    public async Task CrearRiesgo(Riesgo riesgo)
    {
        
        // Obtener el usuario actual desde el contexto de HTTP
        Usuario? user = await _usuariosService.ObtenerUsuarioConectado();

        riesgo.IdUsuario = user.IdUsuario;
        
        _context.Add(riesgo);
        await _context.SaveChangesAsync();

        var bitacora = new Bitacora
        {
            Descripcion = "" + riesgo.Titulo,
            IdUsuario = riesgo.IdUsuario,
            Tabla = "Riesgos",
            TipoAccion = "Crear",
            Fecha = DateTime.Now
        };
        await _bitacoraService.CrearBitacora(bitacora);
    }

    public async Task EditarRiesgo(Riesgo riesgo)
    {
        var riesgoActual = _context.Riesgos.FirstOrDefault(r => r.IdRiesgo == riesgo.IdRiesgo);

        riesgoActual.IdUsuario = _usuariosService.ObtenerUsuarioConectado().Id;
        
        riesgoActual.Probabilidad = riesgo.Probabilidad;
        riesgoActual.Impacto = riesgo.Impacto;
        riesgoActual.Causa = riesgo.Causa;
        riesgoActual.Titulo = riesgo.Titulo;
        riesgoActual.Consecuencia = riesgo.Consecuencia;
        
        
        var bitacora = new Bitacora
        {
            Descripcion = "" + riesgo.Titulo,
            IdUsuario = riesgoActual.IdUsuario,
            Tabla = "Riesgos",
            TipoAccion = "Editar",
            Fecha = DateTime.Now
        };
        await _bitacoraService.CrearBitacora(bitacora);
        
        _context.Update(riesgoActual);
        await _context.SaveChangesAsync();
    }

    public async Task EliminarRiesgo(int id)
    {
        var riesgo = _context.Riesgos.FirstOrDefault(r => r.IdRiesgo == id);

        if (riesgo == null) throw new InvalidOperationException("Sucedió un error al eliminar el riesgo.");
        
        var planes = _context.Planes.Where(p => p.IdRiesgo == id).ToList();
        foreach (var plan in planes)
        {
            _context.Planes.Remove(plan);
        }
        
        var bitacora = new Bitacora
        {
            Descripcion = "" + riesgo.Titulo,
            IdUsuario = riesgo.IdUsuario,
            Tabla = "Riesgos",
            TipoAccion = "Eliminar",
            Fecha = DateTime.Now
        };
        await _bitacoraService.CrearBitacora(bitacora);
        
        _context.Riesgos.Remove(riesgo);
        await _context.SaveChangesAsync();
    }
    
    public bool RiesgoExists(int id)
    {
        return _context.Riesgos.Any(e => e.IdRiesgo == id);
    }
}