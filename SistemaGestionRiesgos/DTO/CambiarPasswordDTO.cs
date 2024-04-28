using System.ComponentModel.DataAnnotations;

namespace SistemaGestionRiesgos.DTO;

public class CambiarPasswordDTO
{
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
}