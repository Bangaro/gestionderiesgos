using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionRiesgos.Models;

public partial class Bitacora
{
    [Key]
    public int IdBitacora { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Descripcion { get; set; }

    public string? TipoAccion { get; set; }

    public string? Tabla { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
