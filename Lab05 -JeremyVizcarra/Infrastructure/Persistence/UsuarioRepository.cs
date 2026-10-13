using Lab05_JeremyVizcarra.Application.Ports.Out;
using Lab05_JeremyVizcarra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lab05_JeremyVizcarra.Infrastructure.Persistence;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(Lab05DbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObtenerPorUsername(string username)
    {
        return await Consulta()
            .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
    }

    public async Task<bool> ExisteUsername(string username)
    {
        return await _dbSet.AnyAsync(u => u.Username.ToLower() == username.ToLower());
    }
}
