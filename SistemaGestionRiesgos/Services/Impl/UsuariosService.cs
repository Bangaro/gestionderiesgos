using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services.Impl;

public class UsuariosService: IUsuariosService
{
    
    private readonly GestionDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuariosService(GestionDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    
    public async Task<bool> Login(LoginDTO login)
    {
        var usuarioClaims = new List<Claim>() { new Claim(ClaimTypes.Name, login.Email) };
        var grandmaIdentity = new ClaimsIdentity(usuarioClaims, "User Identity");
        var usuarioPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
        
        
        //TODO:Verificar contra base de datos que existe un usuario con ese email y contraseña

        await _httpContextAccessor.HttpContext.SignInAsync(usuarioPrincipal);
        return true;
    }
    

    public async Task<bool> IsPrimerLogin(LoginDTO login)
    {
        var user = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == login.Email);

        if (user != null && user.PrimerInicio == true)
        {
            return true;
        }
        return false;
    }
    
    public async Task<bool> CambiarContraseña(CambiarPasswordDTO cambiarPasswordDto)
    {
        var user = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == cambiarPasswordDto.Email && u.ContraseñaTemporal == cambiarPasswordDto.OldPassword);
        
        user.Contraseña = cambiarPasswordDto.NewPassword;
        user.ContraseñaTemporal = null;
        user.PrimerInicio = false;
        
        
        var usuarioClaims = new List<Claim>() { new Claim(ClaimTypes.Name, cambiarPasswordDto.Email) };
        var grandmaIdentity = new ClaimsIdentity(usuarioClaims, "User Identity");
        var usuarioPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

        await _httpContextAccessor.HttpContext.SignInAsync(usuarioPrincipal);
        await _context.SaveChangesAsync();
        return true;
    }
    

    public async void Logout()
    {
        _httpContextAccessor.HttpContext.SignOutAsync();
    }
    
    public async Task<Usuario?> ObtenerUsuarioConectado() {
        Usuario user = await _context.Usuarios.
            FirstOrDefaultAsync(u => u.Email == _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).Value);
        return user;
    }
    
    
}