using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IUsuariosService _userService;

        public RiesgosController(GestionDbContext context, IRiesgosService service, IUsuariosService userService)
        {
            _context = context;
            _service = service;
            _userService = userService;
        }

        // GET: Riesgos
        public async Task<IActionResult> Index(List<RiesgosPlanesViewModel> riesgosPlanesViewModels = null)
        {
            return View(riesgosPlanesViewModels);
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
        [Authorize]
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Descripcion,Impacto,Probabilidad,Causa,Consecuencia")] Riesgo riesgo)
        {

            if (ModelState.IsValid)
            {
                await _service.CrearRiesgo(riesgo); // Espera la finalización del método asincrónico
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }


        // GET: Riesgos/Edit/5
        public async Task<IActionResult> Edit(int id)
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

        // POST: Riesgos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Riesgo riesgo)
        {
            if (id != riesgo.IdRiesgo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.EditarRiesgo(riesgo);
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.RiesgoExists(riesgo.IdRiesgo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            
            
            return View(riesgo);
        }
        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try{
                await _service.EliminarRiesgo(id);
            }catch(InvalidOperationException ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        
    }
}
