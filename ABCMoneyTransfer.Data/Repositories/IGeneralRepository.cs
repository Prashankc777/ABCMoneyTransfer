using ABCMoneyTransfer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABCMoneyTransfer.Data.Repositories;

public interface IGeneralRepository
{
    public Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
}

public class GeneralController(AppDbContext appDbContext) : IGeneralRepository
{
    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
    {
        return await appDbContext.Set<T>().AsNoTracking().ToListAsync();
    }
}