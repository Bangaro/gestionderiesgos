using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionRiesgos.Models;

public partial class Riesgo
{
    [Key]
    public int IdRiesgo { get; set; }

    public string? Titulo { get; set; }

    public string? Descripcion { get; set; }

    public string? Impacto { get; set; }

    public string? Probabilidad { get; set; }

    public string? Causa { get; set; }

    public string? Consecuencia { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<Plan> Planes { get; set; } = new List<Plan>();
}
