using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionRiesgos.Models;

public partial class Plan
{
    [Key]
    public int IdPlan { get; set; }

    public string? TipoPlan { get; set; }

    public string? Descripcion { get; set; }

    public int? IdRiesgo { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Riesgo? IdRiesgoNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
