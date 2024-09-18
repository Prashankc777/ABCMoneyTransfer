using ABCMoneyTransfer.Data.AuthModels;
using ABCMoneyTransfer.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ABCMoneyTransfer.Data.Repositories;

public interface IUserRepository
{
    Task<string> Insert(User user);
    Task Update(User user);
    Task<User> GetById(int id);
    Task<string> Delete(int id);
    Task<IEnumerable<User>> GetAll(bool isTracking = false);
}

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<string> Insert(User user)
    {
         await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
        return user.GivenName;
    }

    public async Task Update(User user)
    {
        User userToUpdate = await _appDbContext.Users.FirstAsync(x => x.Id == user.Id);
        userToUpdate.GivenName = user.GivenName;
        userToUpdate.FirstName = user.FirstName;
        userToUpdate.MiddleName = user.MiddleName;
        userToUpdate.LastName = user.LastName;
        userToUpdate.Address = user.Address;
        userToUpdate.CountryId = user.CountryId;
        userToUpdate.IsSender = user.IsSender;
        userToUpdate.ModifiedDate = user.ModifiedDate;
        userToUpdate.ModifiedBy = user.ModifiedBy;
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<User> GetById(int id)
    {
        return await _appDbContext.Users.Select(u=> new User()
        {
            Id = u.Id,
            FirstName = u.FirstName,
            MiddleName = u.MiddleName,
            LastName = u.LastName,
            GivenName = u.GivenName, 
            Address = u.Address,
            IsSender = u.IsSender,
            CountryId = u.CountryId,
            Country = new Country()
            {
                Id = u.Country.Id,
                Name = u.Country.Name
            }
            
        }).FirstAsync(x => x.Id == id);
        
    }

    public async Task<string> Delete(int id)
    {
       

        User userToDelete = await _appDbContext.Users.FirstAsync(x => x.Id == id);

        if (await _appDbContext.Transactions.AnyAsync(x=>x.SenderId == id || x.ReceiverId == id))
        {
            throw new Exception("Transaction has been done cannot delete the user");
        }
        _appDbContext.Users.Remove(userToDelete);
        await _appDbContext.SaveChangesAsync();
        return userToDelete.GivenName;
    }

    public async Task<IEnumerable<User>> GetAll(bool isTracking = false)
    {
        IQueryable<User> users = _appDbContext.Users.Select(u => new User()
        {
            Id = u.Id,
            FirstName = u.FirstName,
            MiddleName = u.MiddleName,
            LastName = u.LastName,
            GivenName = u.GivenName,
            Address = u.Address,
            IsSender = u.IsSender,
            CountryId = u.CountryId,
            Country = new Country()
            {
                Id = u.Country.Id,
                Name = u.Country.Name
            }
        });

        users = isTracking ? users.AsTracking() : users.AsNoTracking();
        return await users.ToListAsync();
    }
}