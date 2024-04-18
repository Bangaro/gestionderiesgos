using System.ComponentModel.DataAnnotations;

namespace SistemaGestionRiesgos.DTO;

public class CambiarPasswordDTO
{
        
        [Required(ErrorMessage = "El email es requerido.")]
        
        public string Email { get; set; }
        [Required(ErrorMessage = "La contraseña actual es requerida.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es requerida.")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }
    
}