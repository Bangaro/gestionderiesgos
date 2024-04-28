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
                ViewBag.ActionMessage = "Debe ingresar un nombre de usuario y contraseña";
                ViewBag.ActionClass = "salmon";
                return View();
            }
          
            try
            {
                if (await _service.IsPrimerLogin(login))
                {
                    return RedirectToAction("CambiarPassword", "Usuarios");
                }
                
                if (await _service.Login(login))
                {
                    TempData["ActionMessage"] = "Inicio de sesión con éxito";
                    TempData["ActionClass"] = "light-green";
                    return RedirectToAction("Index", "Home");
                }
            }  catch (Exception e)
            {
                ViewBag.ActionMessage = "Correo electrónico o contraseña incorrectos";
                ViewBag.ActionClass = "salmon";
                return View();
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
            if(cambiarPasswordDto.OldPassword == null || cambiarPasswordDto.NewPassword == null || cambiarPasswordDto.ConfirmPassword == null || cambiarPasswordDto.Email == null)
            {
                ViewBag.ActionMessage = "Debes rellenar todos los espacios";
                ViewBag.ActionClass = "salmon";
                return View();
            }
            if (ModelState.IsValid)
            {
                // Busca al usuario por correo electrónico y contraseña temporal
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == cambiarPasswordDto.Email && u.ContraseñaTemporal == cambiarPasswordDto.OldPassword);

                if (user != null)
                {
                    
                    await _service.CambiarContraseña(cambiarPasswordDto);
                    
                    TempData["ActionMessage"] = "Contraseña cambiada con éxito";
                    TempData["ActionClass"] = "light-green";
                    return RedirectToAction("Index", "Home"); // Redirige a la página de inicio
                }

                ViewBag.ActionMessage = "Ha sucedido un error al cambiar la contraseña";
                ViewBag.ActionClass = "salmon";
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
