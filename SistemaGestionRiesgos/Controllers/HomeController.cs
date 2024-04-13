
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;
using SistemaGestionRiesgos.Services;



namespace SistemaGestionRiesgos.Controllers;

public class HomeController : Controller
{
    private readonly GestionDbContext _context;
    private readonly IHomeService _service;

    public HomeController(GestionDbContext context, IHomeService service)
    {
        _context = context;
        _service = service;
    }

    public async Task<IActionResult> Index(List<RiesgosPlanesViewModel> listaRiesgosPlanes = null)
    {
        // Obtener todos los riesgos
        var riesgos = await _context.Riesgos.ToListAsync();

        SetMatrix(riesgos);
        
        return View(listaRiesgosPlanes); 
    }

    
    [HttpPost]
    public async Task<IActionResult> FiltrarRiesgos(string Impacto, string Probabilidad)
    {
        var listaRiesgosPlanes = await _service.FiltrarRiesgos(Impacto, Probabilidad);
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

        List<RiesgosPlanesViewModel> listaRiesgosPlanes = await _service.BuscarRiesgos(Titulo);
        
        if (!listaRiesgosPlanes.Any())
        {
            ViewBag.Message = "No se encontraron riesgos que coincidan con la búsqueda.";
        }

        return View("Index", listaRiesgosPlanes);
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


}