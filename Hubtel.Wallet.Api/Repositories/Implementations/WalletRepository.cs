using Hubtel.Wallet.Api.Data;
using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Models;
using Hubtel.Wallet.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Hubtel.Wallet.Api.Repositories.Implementations;

public class WalletRepository:IWalletRepository
{
    private readonly DataContext _dataContext;

    public WalletRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<Models.Wallet?> AddWallet(Models.Wallet wallet)
    {
        var savedWallet = await _dataContext.Wallets.AddAsync(wallet);
        await _dataContext.SaveChangesAsync();
        return savedWallet.Entity;
    }

    public async Task<Models.Wallet?> GetWalletByTypeAndAccountNumber(WalletType walletType, string accountNumber)
    {
        var savedWallet = await _dataContext.Wallets.FirstOrDefaultAsync(wallet =>
            wallet.WalletType == walletType && wallet.AccountNumber == accountNumber);
        return savedWallet;
    }

    public async Task<Models.Wallet?> GetWalletById(int id, User user)
    {
        return await _dataContext.Wallets.FirstOrDefaultAsync(wallet => wallet.Id == id && wallet.User == user);
    }

    public async Task<List<Models.Wallet>> GetAllWalletsByUser(User user)
    {
        return await _dataContext.Wallets
            .Where(wallet => wallet.User == user).ToListAsync();
    }

    public async Task DeleteWallet(Models.Wallet wallet)
    {
        _dataContext.Wallets.Remove(wallet);
        await _dataContext.SaveChangesAsync();
    }

    public async Task SaveChanges()
    {
        await _dataContext.SaveChangesAsync();
    }
}