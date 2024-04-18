using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;
using SistemaGestionRiesgos.Services;

namespace SistemaGestionRiesgos.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuariosService _service;
        private readonly GestionDbContext _context;

        public UsuariosController(IUsuariosService service, GestionDbContext context)
        {
            _service = service;
            _context = context;
        }


        public async Task<IActionResult> Login()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            if(login.Contraseña == null || login.Email == null)
            {
                ViewBag.Message = "Debe ingresar un nombre de usuario y contraseña";
                return View();
            }
            
            
            if(await _service.IsPrimerLogin(login))
            {
                return RedirectToAction("CambiarPassword", "Usuarios");
            }
            
            if (await _service.Login(login))
            {
                return RedirectToAction("Index", "Home");    
            }
            ViewBag.Message = "Ha ocurrido un error al iniciar sesión";
            return View();
        }
        
        
        [HttpGet]
        public IActionResult CambiarPassword()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CambiarPassword(CambiarPasswordDTO cambiarPasswordDto)
        {
            if (ModelState.IsValid)
            {
                // Busca al usuario por correo electrónico y contraseña temporal
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == cambiarPasswordDto.Email && u.ContraseñaTemporal == cambiarPasswordDto.OldPassword);

                if (user != null)
                {
                    
                    await _service.CambiarContraseña(cambiarPasswordDto);

                    ViewBag.Message = "Contraseña cambiada correctamente.";
                    return RedirectToAction("Index", "Home"); // Redirige a la página de inicio
                }

                ModelState.AddModelError(string.Empty, "Correo electrónico o contraseña temporal incorrectos.");
            }

            return View(cambiarPasswordDto); // Vuelve a mostrar la vista con el DTO
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            _service.Logout();
            return RedirectToAction("Index", "Home");
        }

    }
}
