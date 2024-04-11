
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;


namespace SistemaGestionRiesgos.Controllers;

public class HomeController : Controller
{
    private readonly GestionDbContext _context;

    public HomeController(GestionDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(List<RiesgosPlanesViewModel> listaRiesgosPlanes = null)
    {
        // Obtener todos los riesgos
        var riesgos = await _context.Riesgos.ToListAsync();

        // Calcular conteos de riesgos por probabilidad e impacto
        var bajobajo = riesgos.Count(r => r.Probabilidad == "Bajo" && r.Impacto == "Bajo");
        var moderadobajo = riesgos.Count(r => r.Probabilidad == "Bajo" && r.Impacto == "Moderado");

        // Almacenar los conteos en ViewBag
        ViewBag.BajoBajo = bajobajo;
        ViewBag.ModeradoBajo = moderadobajo;
        
        return View(listaRiesgosPlanes); 
    }

    [HttpPost]
    public async Task<IActionResult> FiltrarRiesgos(string Impacto, string Probabilidad)
    {
        // Obtener los Riesgos que coinciden con el impacto y la probabilidad
        var riesgos =  await _context.Riesgos
            .Where(r => r.Impacto == Impacto && r.Probabilidad == Probabilidad)
            .ToListAsync();

        List<RiesgosPlanesViewModel> listaRiesgosPlanes = new List<RiesgosPlanesViewModel>();

        // Iterar sobre cada Riesgo y obtener el Plan correspondiente (si existe)
        foreach (var riesgo in riesgos)
        {
            // Buscar el Plan asociado al Riesgo (si existe)
            var plan = await _context.Planes.FirstOrDefaultAsync(p => p.IdRiesgo == riesgo.IdRiesgo);

            // Crear un nuevo RiesgosPlanesViewModel y agregarlo a la lista
            var riesgosPlanesViewModel = new RiesgosPlanesViewModel
            {
                Riesgo = riesgo,
                Plan = plan // Puede ser null si no hay un Plan asociado al Riesgo
            };

            listaRiesgosPlanes.Add(riesgosPlanesViewModel);
        }

        // Pasar la lista de RiesgosPlanesViewModel a la vista
        return View("Index", listaRiesgosPlanes);
    }

    [HttpPost]
    public async Task<IActionResult> BuscarRiesgos(string Titulo)
    {
        // Obtener todos los riesgos o filtrar por título si se proporciona uno
        var riesgosQuery = _context.Riesgos.AsQueryable();
            
        if (string.IsNullOrEmpty(Titulo))
        {
            ViewBag.Message = "Ingresa un valor para realizar la búsqueda";
            return View("Index");
            
        }

        riesgosQuery = riesgosQuery.Where(r => EF.Functions.Like(r.Titulo, $"%{Titulo}%"));
        
        var riesgos = await riesgosQuery.ToListAsync();

        // Crear la lista de RiesgosPlanesViewModel
        var listaRiesgosPlanes = new List<RiesgosPlanesViewModel>();

        foreach (var riesgo in riesgos)
        {
            // Buscar el Plan asociado al Riesgo (si existe)
            var plan = await _context.Planes.FirstOrDefaultAsync(p => p.IdRiesgo == riesgo.IdRiesgo);

            // Crear un nuevo RiesgosPlanesViewModel y agregarlo a la lista
            var riesgosPlanesViewModel = new RiesgosPlanesViewModel
            {
                Riesgo = riesgo,
                Plan = plan // Puede ser null si no hay un Plan asociado al Riesgo
            };

            listaRiesgosPlanes.Add(riesgosPlanesViewModel);
        }

        if (listaRiesgosPlanes.IsNullOrEmpty())
        {
            ViewBag.Message = "No se encontraron riesgos que coincidan con la búsqueda.";
        }

        return View("Index", listaRiesgosPlanes);
    }

}