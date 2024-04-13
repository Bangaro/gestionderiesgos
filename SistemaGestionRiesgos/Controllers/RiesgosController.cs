using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;
using SistemaGestionRiesgos.Services;


namespace SistemaGestionRiesgos.Controllers
{
    public class RiesgosController : Controller
    {
        private readonly GestionDbContext _context;
        private readonly IRiesgosService _service;

        public RiesgosController(GestionDbContext context, IRiesgosService service)
        {
            _context = context;
            _service = service;
        }

        // GET: Riesgos
        public async Task<IActionResult> Index(List<RiesgosPlanesViewModel> riesgosPlanesViewModels = null)
        {
            return View(riesgosPlanesViewModels);
        }
        
        public IActionResult RedirectToIndex()
        {
            // Redirigir explícitamente a la página Index del controlador Home
            return RedirectToAction("Index","Home");
        }

        
        [HttpPost]
        public async Task<IActionResult> SeleccionarRiesgo(int Riesgo, string TipoPlan)
        {
            try
            {
                var riesgosPlanesViewModel = await _service.SeleccionarRiesgo(Riesgo, TipoPlan);
                // Pasar la lista de RiesgosPlanesViewModel a la vista
                return View("Index", new List<RiesgosPlanesViewModel> { riesgosPlanesViewModel });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(); // Riesgo no encontrado
            }
        }


        // GET: Riesgos/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Riesgos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRiesgo,Titulo,Descripcion,Impacto,Probabilidad,Causa,Consecuencia,IdUsuario")] Riesgo Riesgo)
        {
            if (ModelState.IsValid)
            {
                _service.CrearRiesgo(Riesgo);
                return RedirectToAction("Index","Home");
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", Riesgo.IdUsuario);
            return RedirectToAction("Index","Home");
        }

        // GET: Riesgos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgo = await _context.Riesgos.FindAsync(id);
            if (riesgo == null)
            {
                return NotFound();
            }

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", riesgo.IdUsuario);
            return View(riesgo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRiesgo,Titulo,Descripcion,Impacto,Probabilidad,Causa,Consecuencia,IdUsuario")] Riesgo riesgo)
        {
            if (id != riesgo.IdRiesgo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(riesgo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RiesgoExists(riesgo.IdRiesgo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", riesgo.IdUsuario);
            return View(riesgo);
        }

        private bool RiesgoExists(int id)
        {
            return _context.Riesgos.Any(e => e.IdRiesgo == id);
        }
    }
}
