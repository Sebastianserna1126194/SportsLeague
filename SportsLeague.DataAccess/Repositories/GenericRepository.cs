using Microsoft.EntityFrameworkCore;
using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Interfaces.Repositories;

namespace SportsLeague.DataAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : AuditBase
{
    protected readonly LeagueDbContext Context;
    protected readonly DbSet<T> DbSet;

    public GenericRepository(LeagueDbContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<T> CreateAsync(T entity)
    {
        entity.CreatedAt = DateTime.UtcNow;
        entity.UpdatedAt = null;

        await DbSet.AddAsync(entity);
        await Context.SaveChangesAsync();

        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;

        DbSet.Update(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var currentEntity = await GetByIdAsync(id);

        if (currentEntity is null)
        {
            return;
        }

        DbSet.Remove(currentEntity);
        await Context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await DbSet.AnyAsync(entity => entity.Id == id);
    }
}
