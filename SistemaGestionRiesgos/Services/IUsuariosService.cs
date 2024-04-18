using System.Security.Claims;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services;

public interface IUsuariosService
{
    Task<bool> Login(LoginDTO login);
    void Logout();
    Task<bool> IsPrimerLogin(LoginDTO login);
    Task<bool> CambiarContraseña(CambiarPasswordDTO cambiarPasswordDto);
    Task<Usuario?> ObtenerUsuarioConectado();
}