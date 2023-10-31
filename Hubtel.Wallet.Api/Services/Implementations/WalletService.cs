using HandySquad.Exceptions;
using Hubtel.Wallet.Api.Dto;
using Hubtel.Wallet.Api.Enums;
using Hubtel.Wallet.Api.Repositories.Interfaces;
using Hubtel.Wallet.Api.Services.Interfaces;

namespace Hubtel.Wallet.Api.Services.Implementations;

public class WalletService:IWalletService
{
    private readonly IWalletRepository _walletRepository;
    private readonly IAuthenticationService _authenticationService;

    public WalletService(IWalletRepository walletRepository, IAuthenticationService authenticationService)
    {
        _walletRepository = walletRepository;
        _authenticationService = authenticationService;
    }

    public async Task<MessageResponseDto> AddWallet(WalletRequestDto walletRequestDto, string authorizationHeader)
    {
        if (!CheckForTypeOfCardWalletAgainstScheme(walletRequestDto.WalletScheme) || !CheckForTypeOfMobileWalletAgainstScheme(walletRequestDto.WalletScheme))
        {
            throw new BadRequest400Exception("account type does not match account scheme");
        }
        var user = await _authenticationService.GetUserFromHeader(authorizationHeader);
        var accountNumber = walletRequestDto.AccountNumber;
        var userWallets = await _walletRepository.GetAllWalletsByUser(user);
        if (userWallets.Count == 5)
        {
            throw new BadRequest400Exception("user wallet addition maxed out");
        }

        if (walletRequestDto.WalletType == WalletType.Card)
        {
            accountNumber = walletRequestDto.AccountNumber[..6];
        }
        if (await _walletRepository.GetWalletByTypeAndAccountNumber(walletRequestDto.WalletType,
                accountNumber) != null)
        {
            throw new Duplicate409Exception("wallet already added");
        }
        
        var newWallet = new Models.Wallet
        {
            AccountNumber = accountNumber,
            Name = walletRequestDto.name,
            User = user,
            WalletType = walletRequestDto.WalletType,
            WalletScheme = walletRequestDto.WalletScheme
        };
        await _walletRepository.AddWallet(newWallet);
        return new MessageResponseDto
        {
            Message = "wallet added successfully",
            Status = "success"
        };
    }

    public async Task<List<Models.Wallet>> GetAllWallets(string authorizationHeader)
    {
        var user =await _authenticationService.GetUserFromHeader(authorizationHeader);
        
        return await _walletRepository.GetAllWalletsByUser(user);
    }

    public async Task<Models.Wallet> GetWalletById(int id, string authorizationHeader)
    {
        var user = await _authenticationService.GetUserFromHeader(authorizationHeader);
        var wallet = await _walletRepository.GetWalletById(id, user);
        if (wallet == null)
        {
            throw new NotFound404Exception("wallet not found");
        }

        return wallet;
    }

    
    public async Task<MessageResponseDto> DeleteWalletById(int id, string authorizationHeader)
    {
        var user = await _authenticationService.GetUserFromHeader(authorizationHeader);
        var wallet = await _walletRepository.GetWalletById(id, user);
        if (wallet == null)
        {
            throw new NotFound404Exception("wallet not found");
        }

        return new MessageResponseDto
        {
            Message = "wallet deleted successfully",
            Status = "success"
        };
    }

    private bool CheckForTypeOfCardWalletAgainstScheme(WalletScheme walletScheme)
    {
        return walletScheme is WalletScheme.Visa or WalletScheme.MasterCard;
    }

    private bool CheckForTypeOfMobileWalletAgainstScheme(WalletScheme walletScheme)
    {
        return walletScheme is WalletScheme.Mtn  or WalletScheme.Vodafone or WalletScheme.AirtelTigo;
    }
}