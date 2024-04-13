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
    public async void CrearBitacora(BitacoraDTO bitacoraDto)
    {
        var bitacora = new Bitacora
        {
            Descripcion = bitacoraDto.Descripcion,
            Tabla = bitacoraDto.Tabla,
            TipoAccion = bitacoraDto.TipoAccion,
            IdUsuario = bitacoraDto.IdUsuario
        };

        _context.Add(bitacora);
        await _context.SaveChangesAsync();
    }
}