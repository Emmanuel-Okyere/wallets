using Hubtel.Wallet.Api.Data;
using Hubtel.Wallet.Api.Models;
using Hubtel.Wallet.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallet.Api.Repositories.Implementations;

public class UserRepository:IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<User?> AddUser(User user)
    {
        var savedUser = _dataContext.Users.Add(user); 
        await _dataContext.SaveChangesAsync();
        return savedUser.Entity;
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _dataContext.Users.FirstOrDefaultAsync(user => user.Id == id);
    }

    public async Task<User?> GetUserByPhoneNumber(string phoneNumber)
    {
        return await _dataContext.Users.FirstOrDefaultAsync(a => a.PhoneNumber == phoneNumber);
    }
    
}