using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Context;

public partial class GestionDbContext : DbContext
{
    public GestionDbContext()
    {
    }

    public GestionDbContext(DbContextOptions<GestionDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Usuario?> Usuarios { get; set; }
    public DbSet<Bitacora> Bitacoras { get; set; }
    public DbSet<Plan> Planes { get; set; }
    public DbSet<Riesgo> Riesgos { get; set; }
    
    

}
