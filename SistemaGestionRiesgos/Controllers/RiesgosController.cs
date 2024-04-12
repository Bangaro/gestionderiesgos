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

namespace SistemaGestionRiesgos.Controllers
{
    public class RiesgosController : Controller
    {
        private readonly GestionDbContext _context;

        public RiesgosController(GestionDbContext context)
        {
            _context = context;
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
            // Obtener el Riesgo por su Id
            var riesgo = await _context.Riesgos.FirstOrDefaultAsync(r => r.IdRiesgo == Riesgo);

            if (riesgo == null)
            {
                // Manejar el caso donde no se encuentra el Riesgo
                return NotFound(); // Puedes devolver una vista específica para manejar el error
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
            return View("Index", new List<RiesgosPlanesViewModel> { riesgosPlanesViewModel });
        }


        // GET: Riesgos/Create
        public IActionResult Create()
        {
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario");
            return View();
        }

        // POST: Riesgos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRiesgo,Titulo,Descripcion,Impacto,Probabilidad,Causa,Consecuencia,IdUsuario")] Riesgo riesgo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(riesgo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "IdUsuario", riesgo.IdUsuario);
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

        // POST: Riesgos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Riesgos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var riesgo = await _context.Riesgos
                .Include(r => r.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdRiesgo == id);
            if (riesgo == null)
            {
                return NotFound();
            }

            return View(riesgo);
        }

        // POST: Riesgos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var riesgo = await _context.Riesgos.FindAsync(id);
            if (riesgo != null)
            {
                _context.Riesgos.Remove(riesgo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiesgoExists(int id)
        {
            return _context.Riesgos.Any(e => e.IdRiesgo == id);
        }
    }
}
