using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Models;

namespace Hubtel.Wallet.Api.Repositories.Interfaces;

public interface IWalletRepository
{
    Task<Models.Wallet?> AddWallet(Models.Wallet wallet);
    Task<Api.Models.Wallet?> GetWalletByTypeAndAccountNumber(WalletType walletType, string accountNumber);
    Task<Models.Wallet?> GetWalletById(int id,User user);
    Task<List<Models.Wallet>> GetAllWalletsByUser(User user);
    Task DeleteWallet(Models.Wallet wallet);
    Task SaveChanges();
}