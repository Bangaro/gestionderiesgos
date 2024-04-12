using System;
using System.Collections.Generic;

namespace SistemaGestionRiesgos.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Contraseña { get; set; }

    public virtual ICollection<Plan> Planes { get; set; } = new List<Plan>();

    public virtual ICollection<Riesgo> Riesgos { get; set; } = new List<Riesgo>();
}
