using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services.Impl;

public class HomeService: IHomeService
{
    private readonly GestionDbContext _context;

    public HomeService(GestionDbContext context)
    {
        _context = context;
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
}