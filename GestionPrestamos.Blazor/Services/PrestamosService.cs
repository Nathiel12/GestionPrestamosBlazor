using GestionPrestamos.Context;
using GestionPrestamos.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GestionPrestamos.Services;

public class PrestamosService(IDbContextFactory<Contexto> DbFactory)
{
    private async Task<bool> Existe(int prestamoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .AnyAsync(p => p.PrestamoId == prestamoId);
    }

    private async Task<bool> Insertar(Prestamos prestamo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Prestamos.Add(prestamo); 
        return await contexto.SaveChangesAsync() > 0;
    }

    private async Task<bool> Modificar(Prestamos prestamo)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();

        var cuotasExistentes = contexto.PrestamosDetalles.Where(p => p.PrestamoId == prestamo.PrestamoId).ToList();
        contexto.PrestamosDetalles.RemoveRange(cuotasExistentes);

        contexto.Update(prestamo);

        return await contexto.SaveChangesAsync() > 0;
    }

    public async Task<bool> Guardar(Prestamos prestamo)
    {
        prestamo.Balance = prestamo.Monto;
        if (!await Existe(prestamo.PrestamoId))
        {
            return await Insertar(prestamo);
        }
        else
        {
            return await Modificar(prestamo);
        }
    }

    public async Task<Prestamos> Buscar(int prestamoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos.Include(d => d.Deudor).Include(p => p.PrestamosDetalle).FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);
    }

    public async Task<bool> Eliminar(int prestamoId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        
        var prestamo = await contexto.Prestamos
            .Include(p => p.PrestamosDetalle) 
            .FirstOrDefaultAsync(p => p.PrestamoId == prestamoId);

        if (prestamo == null)
            return false;
        
        contexto.PrestamosDetalles.RemoveRange(prestamo.PrestamosDetalle);

        contexto.Prestamos.Remove(prestamo);

        var cantidad = await contexto.SaveChangesAsync();

        return cantidad > 0;
    }

    public async Task<List<Prestamos>> GetList(Expression<Func<Prestamos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Include(d => d.Deudor)
            .Where(criterio)
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<List<Prestamos>> GetPrestamosPendientes(int deudorId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos
            .Where(p => p.DeudorId == deudorId && p.Balance > 0)
            .OrderBy(p => p.PrestamoId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Prestamos?> BuscarPrestamo(int id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Prestamos.
            Include(p => p.Deudor)
           .FirstOrDefaultAsync(p => p.DeudorId == id);
    }
    public async Task<bool> EliminarDetalle(int detalleId)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var detalle = await contexto.PrestamosDetalles.FindAsync(detalleId);

        if (detalle != null)
        {
            contexto.PrestamosDetalles.Remove(detalle);
            await contexto.SaveChangesAsync();
            return true;
        }
        return false;
    }
}
