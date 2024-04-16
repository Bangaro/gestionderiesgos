using Microsoft.EntityFrameworkCore;
using SistemaGestionRiesgos.Context;
using SistemaGestionRiesgos.DTO;
using SistemaGestionRiesgos.Models;

namespace SistemaGestionRiesgos.Services.Impl;

public class BitacoraService: IBitacoraService
{
    
    private readonly GestionDbContext _context;

    public BitacoraService(GestionDbContext context)
    {
        _context = context;
    }

    public async Task<List<Bitacora>> ListaBitacoras()
    {
        var listaBitacora = await _context.Bitacoras.ToListAsync();
        return listaBitacora;
    }

    public async void CrearBitacora(BitacoraDTO bitacoraDto)
    {
        _context.Add(bitacoraDto);
        await _context.SaveChangesAsync();
    }
}