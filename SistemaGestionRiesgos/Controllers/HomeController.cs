
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

        SetMatrix(riesgos);
        
        return View(listaRiesgosPlanes); 
    }

    private void SetMatrix(List<Riesgo> riesgos)
    {
         // Calcular conteos de riesgos por probabilidad e impacto
    var bajobajo = riesgos.Count(r => r.Probabilidad == "Bajo" && r.Impacto == "Bajo");
    var bajomoderado = riesgos.Count(r => r.Probabilidad == "Bajo" && r.Impacto == "Moderado");
    var bajoalto = riesgos.Count(r => r.Probabilidad == "Bajo" && r.Impacto == "Alto");
    var bajomuyalto = riesgos.Count(r => r.Probabilidad == "Bajo" && r.Impacto == "Muy Alto");

    var moderadobajo = riesgos.Count(r => r.Probabilidad == "Moderado" && r.Impacto == "Bajo");
    var moderadomoderado = riesgos.Count(r => r.Probabilidad == "Moderado" && r.Impacto == "Moderado");
    var moderadoalto = riesgos.Count(r => r.Probabilidad == "Moderado" && r.Impacto == "Alto");
    var moderadomuyalto = riesgos.Count(r => r.Probabilidad == "Moderado" && r.Impacto == "Muy Alto");

    var altoabajo = riesgos.Count(r => r.Probabilidad == "Alto" && r.Impacto == "Bajo");
    var altomoderado = riesgos.Count(r => r.Probabilidad == "Alto" && r.Impacto == "Moderado");
    var altoalto = riesgos.Count(r => r.Probabilidad == "Alto" && r.Impacto == "Alto");
    var altomuyalto = riesgos.Count(r => r.Probabilidad == "Alto" && r.Impacto == "Muy Alto");

    var muyaltoabajo = riesgos.Count(r => r.Probabilidad == "Muy Alto" && r.Impacto == "Bajo");
    var muyaltomoderado = riesgos.Count(r => r.Probabilidad == "Muy Alto" && r.Impacto == "Moderado");
    var muyaltoalto = riesgos.Count(r => r.Probabilidad == "Muy Alto" && r.Impacto == "Alto");
    var muyaltomuyalto = riesgos.Count(r => r.Probabilidad == "Muy Alto" && r.Impacto == "Muy Alto");

    // Almacenar los conteos en la sesión
    HttpContext.Session.SetInt32("BajoBajo", bajobajo);
    HttpContext.Session.SetInt32("BajoModerado", bajomoderado);
    HttpContext.Session.SetInt32("BajoAlto", bajoalto);
    HttpContext.Session.SetInt32("BajoMuyAlto", bajomuyalto);

    HttpContext.Session.SetInt32("ModeradoBajo", moderadobajo);
    HttpContext.Session.SetInt32("ModeradoModerado", moderadomoderado);
    HttpContext.Session.SetInt32("ModeradoAlto", moderadoalto);
    HttpContext.Session.SetInt32("ModeradoMuyAlto", moderadomuyalto);

    HttpContext.Session.SetInt32("AltoBajo", altoabajo);
    HttpContext.Session.SetInt32("AltoModerado", altomoderado);
    HttpContext.Session.SetInt32("AltoAlto", altoalto);
    HttpContext.Session.SetInt32("AltoMuyAlto", altomuyalto);

    HttpContext.Session.SetInt32("MuyAltoBajo", muyaltoabajo);
    HttpContext.Session.SetInt32("MuyAltoModerado", muyaltomoderado);
    HttpContext.Session.SetInt32("MuyAltoAlto", muyaltoalto);
    HttpContext.Session.SetInt32("MuyAltoMuyAlto", muyaltomuyalto);
    }

    
    [HttpPost]
    public async Task<IActionResult> FiltrarRiesgos(string Impacto, string Probabilidad)
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

        if (!listaRiesgosPlanes.Any())
        {
            ViewBag.Message = "No se encontraron riesgos que coincidan con la búsqueda.";
        }

        return View("Index", listaRiesgosPlanes);
    }

}