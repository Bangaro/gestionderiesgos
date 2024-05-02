using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.Models;
using SistemaGestionRiesgos.Services;

namespace SistemaGestionRiesgos.Controllers
{
    public class PlanesController : Controller
    {
        private readonly GestionDbContext _context;
        private readonly IPlanesService _service;
        

        public PlanesController(GestionDbContext context, IPlanesService service)
        {
            _context = context;
            _service = service;
        }


        // GET: Planes
        public async Task<IActionResult> Index()
        {
            var gestionDbContext = _context.Planes.Include(p => p.IdRiesgoNavigation).Include(p => p.IdUsuarioNavigation);
            return View(await gestionDbContext.ToListAsync());
        }

       
        // GET: Planes/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            
            // Obtener todos los riesgos disponibles desde la base de datos
            var riesgos = _context.Riesgos.ToList();

            // Crear una lista de SelectListItem para el dropdown
            ViewBag.Riesgos = riesgos.Select(r => new SelectListItem
            {
                Text = r.Titulo, // Mostrar el título del riesgo en el dropdown
                Value = r.IdRiesgo.ToString() // Almacenar el ID del riesgo como valor
            });

            return View();
        }

        // POST: Planes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlan,TipoPlan,Descripcion,IdRiesgo,IdUsuario")] Plan plan)
        {
            //TODO: Realizar las validaciones del formulario, que no haya nada en blanco
            //TODO: Se puede mostrar un mensaje si faltan espacios por rellenar
            
            if (ModelState.IsValid)
            {
                await _service.CrearPlan(plan); 
                
                //MENSAJE PARA NOTIFICACIONES
                TempData["ActionMessage"] = "Plan creado con éxito";
                TempData["ActionClass"] = "light-green";
                
                
                
                return RedirectToAction("Index", "Home");
            }

            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", plan.IdUsuario);
            return View(plan);
        }

        // GET: Planes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Planes.FindAsync(id);
            if (plan == null)
            {
                return NotFound();
            }
            ViewData["IdRiesgo"] = new SelectList(_context.Riesgos, "IdRiesgo", "IdRiesgo", plan.IdRiesgo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", plan.IdUsuario);
            return View(plan);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("IdPlan,TipoPlan,Descripcion,IdRiesgo,IdUsuario")] Plan plan)
        {
            //TODO: Realizar las validaciones del formulario, que no haya nada en blanco
            //TODO: Se puede mostrar un mensaje si faltan espacios por rellenar
            
            if (id != plan.IdPlan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _service.EditarPlan(plan);
                    
                    TempData["ActionMessage"] = "Plan editado con éxito";
                    TempData["ActionClass"] = "light-green";
                    return RedirectToAction("Index", "Home");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.PlanExists(plan.IdPlan))
                    {
                        return NotFound();
                    }
                }
            }
            return View(plan);
        }

        // POST: Planes/Delete/5
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try{
                await _service.EliminarPlan(id);
                TempData["ActionMessage"] = "Plan eliminado con éxito";
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
