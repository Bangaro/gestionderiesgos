using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        
        //TODO: Implementar la vista de la bitacora, la cual debe mostrar una lista de las bitacoras registradas en el sistema
        //TODO: Puede hacerse con las cards de bootstrap

        // GET: BitacoraController
        public async Task<IActionResult> Index()
        {
            var listaBitacoras = await _context.Bitacoras.ToListAsync();
            return View(listaBitacoras);
        }

        [Authorize]
        public IActionResult MostrarBitacora()
        {
            return View();
        }

    }
}
