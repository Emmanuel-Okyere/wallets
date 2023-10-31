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
    private readonly ILogger<WalletService> _logger;

    public WalletService(IWalletRepository walletRepository, IAuthenticationService authenticationService, ILogger<WalletService> logger)
    {
        _walletRepository = walletRepository;
        _authenticationService = authenticationService;
        _logger = logger;
    }

    public async Task<MessageResponseDto> AddWallet(WalletRequestDto walletRequestDto, string authorizationHeader)
    {
        _logger.LogInformation("wallet creation for {} initiated",walletRequestDto.AccountNumber);
        if (walletRequestDto.WalletType == WalletType.Card && !CheckForTypeOfCardWalletAgainstScheme(walletRequestDto.WalletScheme))
        {
            _logger.LogInformation("wallet creation for {} initiated error: types not matching",walletRequestDto.AccountNumber);
            throw new BadRequest400Exception("account type does not match account scheme accepted schemes are [Visa, Mastercard]");
        }
        if (walletRequestDto.WalletType == WalletType.MobileMoney && !CheckForTypeOfMobileWalletAgainstScheme(walletRequestDto.WalletScheme))
        {
            _logger.LogInformation("wallet creation for {} initiated error: types not matching",walletRequestDto.AccountNumber);
            throw new BadRequest400Exception("account type does not match account scheme accepted schemes are [Mtn, Vodafone, AirtelTigo]");
        }
        var user = await _authenticationService.GetUserFromHeader(authorizationHeader);
        var accountNumber = walletRequestDto.AccountNumber;
        var userWallets = await _walletRepository.GetAllWalletsByUser(user);
        if (userWallets.Count == 5)
        {
            _logger.LogInformation("wallet creation for {} initiated error: already has 5 wallets",walletRequestDto.AccountNumber);
            throw new BadRequest400Exception("user wallet addition maxed out");
        }

        if (walletRequestDto.WalletType == WalletType.Card)
        {
            accountNumber = walletRequestDto.AccountNumber.Replace("-","")[..6];
        }
        if (await _walletRepository.GetWalletByTypeAndAccountNumber(walletRequestDto.WalletType,
                accountNumber) != null)
        {
            _logger.LogInformation("wallet creation for {} initiated error: wallet already added",walletRequestDto.AccountNumber);
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
        _logger.LogInformation("wallet creation for {} initiated success",walletRequestDto.AccountNumber);
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

        await _walletRepository.DeleteWallet(wallet);
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