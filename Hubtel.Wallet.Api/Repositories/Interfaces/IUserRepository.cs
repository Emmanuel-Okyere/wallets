using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> AddUser(User user);
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByPhoneNumber(string phoneNumber);
}