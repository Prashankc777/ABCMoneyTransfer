using ABCMoneyTransfer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABCMoneyTransfer.Data.Repositories;

public interface ITransactionRepository
{
    Task Insert(Transaction transaction);
    Task<IEnumerable<Transaction>> GetAll();
}

public class TransactionRepository(AppDbContext appDbContext) : ITransactionRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task Insert(Transaction transaction)
    {
        await _appDbContext.Transactions.AddAsync(transaction);
        await _appDbContext.SaveChangesAsync();
        
    }

    public async Task<IEnumerable<Transaction>> GetAll()
    {
        return await _appDbContext.Transactions
            .Select(x=> new Transaction()
            {
                Id = x.Id,
                Receiver = new User()
                {
                    Id = x.Receiver.Id,
                    GivenName = x.Receiver.GivenName
                },
                Sender = new User()
                {
                    Id = x.Sender.Id,
                    GivenName = x.Sender.GivenName
                },
                TransferAmountMyr = x.TransferAmountMyr,
                ExchangeRate = x.ExchangeRate,
                CreatedDate = x.CreatedDate,
                Amount = x.Amount
            })
            .AsNoTracking().ToListAsync();
    }
}