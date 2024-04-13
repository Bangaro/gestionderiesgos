
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services.Impl;

public class RiesgosService: IRiesgosService
{
    
    private readonly GestionDbContext _context;

    public RiesgosService(GestionDbContext context)
    {
        _context = context;
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

    public async void CrearRiesgo(Riesgo riesgo)
    {
        _context.Add(riesgo);
        await _context.SaveChangesAsync();
    }


}