using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HandySquad.Exceptions;
using Hubtel.Wallet.Api.Dto;
using Hubtel.Wallet.Api.Models;
using Hubtel.Wallet.Api.Repositories.Interfaces;
using Hubtel.Wallet.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Hubtel.Wallet.Api.Services.Implementations;

public class AuthenticationService:IAuthenticationService
{
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthenticationService(ILogger<AuthenticationService> logger, IUserRepository userRepository, IConfiguration configuration)
    {
        _logger = logger;
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<UserResponseDto> Register(RegisterRequestDto registerRequestDto)
    {
        _logger.LogInformation("Registration for {} initiated",registerRequestDto.PhoneNumber);
        if (await _userRepository.GetUserByPhoneNumber(registerRequestDto.PhoneNumber) != null)
        {
            throw new Duplicate409Exception("user already exist");
        }
        if(!registerRequestDto.Password.Equals(registerRequestDto.ConfirmPassword))
        {
            throw new BadRequest400Exception("passwords do not match");
        }
        CreatePasswordHashAndSalt(registerRequestDto.Password, out var passwordHash, out var passwordSalt);
        var newUser = new User
        {
            PhoneNumber = registerRequestDto.PhoneNumber,
            Username = registerRequestDto.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        var savedUser = await _userRepository.AddUser(newUser);
        var token = CreateToken(registerRequestDto.PhoneNumber);
        return new UserResponseDto
        {
            Message = "user saved successfully",
            Status = "success",
            Data = savedUser,
            Token = token
        };
    }

    public async Task<UserResponseDto> Login(LoginDto loginDto)
    {
        _logger.LogInformation("login in for {} initiated",loginDto.PhoneNumber);
        var user = await _userRepository.GetUserByPhoneNumber(loginDto.PhoneNumber);
        if (user == null)
        {
            _logger.LogInformation("Login in for {} failure, phone number does not exist",loginDto.PhoneNumber);
            throw new NotFound404Exception("user does not exist");
        }

        if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            _logger.LogInformation("Login in for {} failure, password incorrect",loginDto.PhoneNumber);
            throw new BadRequest400Exception("password incorrect");
        }
        var token = CreateToken(loginDto.PhoneNumber);
        _logger.LogInformation("Login in  for {} completed",loginDto.PhoneNumber);
        return new UserResponseDto
        {
            Message = "login success",
            Status = "success",
            Data = user,
            Token = token
        };
    }
    private void CreatePasswordHashAndSalt(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        _logger.LogInformation("creating password hash and salt");
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        _logger.LogInformation("creating password hash and salt completed");
    }
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        _logger.LogInformation("Verifying password hash");
        using var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        for (var i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != passwordHash[i])
            {
                return false;
            }
        }
        _logger.LogInformation("password hash verified");
        return true;
    }
    private string CreateToken(string phone)
    {
        _logger.LogInformation("creating JWT token");
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.MobilePhone,phone)
        };
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:Key").Value!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = credentials,
            Audience = _configuration.GetSection("JWT:Audience").Value!,
            Issuer = _configuration.GetSection("JWT:Issuer").Value!,
            IssuedAt = DateTime.UtcNow,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        _logger.LogInformation("writing JWT Token completed");
        return tokenHandler.WriteToken(token);
    }
    public async Task<User> GetUserFromHeader(string authorizationHeader)
    {
        if (authorizationHeader.IsNullOrEmpty() || !authorizationHeader.StartsWith("Bearer "))
        {
            throw new BadRequest400Exception("authorization not found in header");
        }
        var token = authorizationHeader["Bearer ".Length..];
        var tokenHandler = new JwtSecurityTokenHandler();
        if (!tokenHandler.ReadJwtToken(token).Payload.TryGetValue(ClaimTypes.MobilePhone, out var userPhoneNumber))
        {
            throw new BadRequest400Exception("authorization not found in header");
        }
        var user = await _userRepository.GetUserByPhoneNumber(userPhoneNumber.ToString());
        if (user == null)
        {
            throw new BadRequest400Exception("user not found in header");
        }
        return user;
    }
}