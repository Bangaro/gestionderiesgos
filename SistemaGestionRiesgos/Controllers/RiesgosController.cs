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

            //TODO: Realizar las validaciones del formulario, que no haya nada en blanco
            //TODO: Se puede mostrar un mensaje si faltan espacios por rellenar
            if(riesgo.Descripcion == null || riesgo.Titulo == null || riesgo.Causa == null || riesgo.Consecuencia == null)
            {
                ViewBag.ActionMessage = "Por favor, rellene todos los campos";
                ViewBag.ActionClass = "salmon";
                return View();
            }
            
            if (ModelState.IsValid)
            {
                await _service.CrearRiesgo(riesgo); 
                
                TempData["ActionMessage"] = "Riesgo creado con éxito";
                TempData["ActionClass"] = "light-green";
                
                return RedirectToAction("Index", "Home");
            }

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", riesgo.IdUsuario);
            return View(riesgo);
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

        
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Riesgo riesgo)
        {
            if (id != riesgo.IdRiesgo)
            {
                return NotFound();
            }
            
            if(riesgo.Descripcion == null || riesgo.Titulo == null || riesgo.Causa == null || riesgo.Consecuencia == null)
            {
                ViewBag.ActionMessage = "Por favor, rellene todos los campos para realizar las modificaciones";
                ViewBag.ActionClass = "salmon";
                return View();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.EditarRiesgo(riesgo);

                    TempData["ActionMessage"] = "Riesgo editado con éxito";
                    TempData["ActionClass"] = "light-green";
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
                TempData["ActionMessage"] = "Riesgo eliminado con éxito";
                TempData["ActionClass"] = "light-green";
            }catch(InvalidOperationException ex)
            {
                ViewBag.Message = ex.Message;
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }

        
    }
}
