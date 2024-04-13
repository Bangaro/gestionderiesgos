using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Controllers
{
    public class BitacoraController : Controller
    {
        
        private readonly GestionDbContext _context;

        public BitacoraController(GestionDbContext context)
        {
            _context = context;
        }

        // GET: BitacoraController
        public async Task<IActionResult> Index()
        {
            var listaBitacoras = await _context.Bitacoras.ToListAsync();
            return View(listaBitacoras);
        }
       
    }
}
