using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SistemaGestionRiesgos.Models;

public partial class Usuario
{
    [Key]
    public int IdUsuario { get; set; }
    public string? UserName { get; set; }
    
    public string? ContraseñaTemporal { get; set; }
    
    public string? Contraseña { get; set; }
    
    public string? Email { get; set; }

    public bool? Activo { get; set; } = true;

    public bool? PrimerInicio { get; set; } = true;

    public virtual ICollection<Bitacora> Bitacoras { get; set; } = new List<Bitacora>();

    public virtual ICollection<Plan> Planes { get; set; } = new List<Plan>();

    public virtual ICollection<Riesgo> Riesgos { get; set; } = new List<Riesgo>();
}
