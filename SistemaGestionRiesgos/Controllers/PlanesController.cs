using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Controllers
{
    public class PlanesController : Controller
    {
        private readonly GestionDbContext _context;

        public PlanesController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: Planes
        public async Task<IActionResult> Index()
        {
            var gestionDbContext = _context.Planes.Include(p => p.IdRiesgoNavigation).Include(p => p.IdUsuarioNavigation);
            return View(await gestionDbContext.ToListAsync());
        }

       
        // GET: Planes/Create
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPlan,TipoPlan,Descripcion,IdRiesgo,IdUsuario")] Plan plan)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plan);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            ViewData["IdRiesgo"] = new SelectList(_context.Riesgos, "IdRiesgo", "IdRiesgo", plan.IdRiesgo);
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
        public async Task<IActionResult> Edit(int id, [Bind("IdPlan,TipoPlan,Descripcion,IdRiesgo,IdUsuario")] Plan plan)
        {
            if (id != plan.IdPlan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlanExists(plan.IdPlan))
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
            ViewData["IdRiesgo"] = new SelectList(_context.Riesgos, "IdRiesgo", "IdRiesgo", plan.IdRiesgo);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", plan.IdUsuario);
            return View(plan);
        }

        // GET: Planes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.Planes
                .Include(p => p.IdRiesgoNavigation)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdPlan == id);
            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: Planes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.Planes.FindAsync(id);
            if (plan != null)
            {
                _context.Planes.Remove(plan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlanExists(int id)
        {
            return _context.Planes.Any(e => e.IdPlan == id);
        }
    }
}
