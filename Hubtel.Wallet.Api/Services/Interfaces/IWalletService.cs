using Hubtel.Wallet.Api.Dto;
namespace Hubtel.Wallet.Api.Services.Interfaces;

public interface IWalletService
{
    Task<MessageResponseDto> AddWallet(WalletRequestDto walletRequestDto, string authorizationHeader);
    Task<List<Models.Wallet>> GetAllWallets(string authorizationHeader);
    Task<Models.Wallet> GetWalletById(int id, string authorizationHeader);
    Task<MessageResponseDto> DeleteWalletById(int id, string authorizationHeader);
}