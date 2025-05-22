using FluentValidation;
using System.Security.Claims;
using UserContacts.Bll.Helpers;
using UserContacts.Core.Errors;
using UserContacts.Manager.Bll.Helpers.Security;
using UserContactsManager.Bll.Dtos;
using UserContactsManager.Bll.Repository.RepositoryService;
using UserContactsManager.Bll.Repository.Settings;
using UserContactsManager.Dal.Entities;


namespace UserContacts.Bll.Services;

public class AuthService : IAuthService
{
    private readonly IRoleRepository _roleRepo;
    private readonly IValidator<UserCreateDto> _validator;
    private readonly IUserRepository _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IValidator<UserCreateDto> _validatorForLogin;
    private readonly IRefreshTokenRepository _refTokRepo;
    private readonly JwtAppSettings _jwtSetting;

    public AuthService(IRoleRepository roleRepo, IValidator<UserCreateDto> validator,
    IUserRepository userRepo, ITokenService tokenService,
    JwtAppSettings jwtSetting, IValidator<UserCreateDto> validatorForLogin,
    IRefreshTokenRepository refTokRepo)
    {
        _jwtSetting = jwtSetting;
        _roleRepo = roleRepo;
        _validator =validator;
        _userRepo = userRepo;
        _tokenService = tokenService;
        _validatorForLogin = validatorForLogin;
        _refTokRepo = refTokRepo;

    }
    //private readonly int Expires = int.Parse(_jwtSetting.Lifetime);
    public async Task<long> SignUpUserAsync(UserCreateDto userCreateDto)
    {
        var validatorResult = await _validator.ValidateAsync(userCreateDto);
        if (!validatorResult.IsValid)
        {
            string errorMessages = string.Join("; ", validatorResult.Errors.Select(e => e.ErrorMessage));
            throw new AuthException(errorMessages);
        }

        var tupleFromHasher = PasswordHasher.Hasher(userCreateDto.Password);
        var user = new User()
        {
            FirstName = userCreateDto.FirstName,
            LastName = userCreateDto.LastName,
            UserName = userCreateDto.UserName,
            Email = userCreateDto.Email,
            PhoneNumber = userCreateDto.PhoneNumber,
            Password = tupleFromHasher.Hash,
            Salt = tupleFromHasher.Salt,
        };
        user.RoleId = (int)await _roleRepo.GetRoleIdAsync("User");

        return await _userRepo.InsertUserAync(user);
    }

    public async Task<LoginResponseDto> LoginUserAsync(UserLoginDto userLoginDto)
    {
        var user = await _userRepo.SelectUserByUserNameAync(userLoginDto.UserName);

        var checkUserPassword = PasswordHasher.Verify(userLoginDto.Password, user.Password, user.Salt);

        if (checkUserPassword == false)
        {
            throw new UnauthorizedException("UserName or password incorrect");
        }

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.Name,
        };

        var token = _tokenService.GenerateToken(userGetDto);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = refreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refTokRepo.AddRefreshToken(refreshTokenToDB);

        var loginResponseDto = new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            TokenType = "Bearer",
            Expires = 24
        };


        return loginResponseDto;
    }


    public async Task<LoginResponseDto> RefreshTokenAsync(RefreshRequestDto request)
    {
        ClaimsPrincipal? principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal == null) throw new ForbiddenException("Invalid access token.");


        var userClaim = principal.FindFirst(c => c.Type == "UserId");
        var userId = long.Parse(userClaim.Value);


        var refreshToken = await _refTokRepo.SelectRefreshToken(request.RefreshToken, userId);
        if (refreshToken == null || refreshToken.Expires < DateTime.UtcNow || refreshToken.IsRevoked)
            throw new UnauthorizedException("Invalid or expired refresh token.");

        refreshToken.IsRevoked = true;

        var user = await _userRepo.SelectUserByIdAync(userId);

        var userGetDto = new UserGetDto()
        {
            UserId = user.UserId,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.Name,
        };

        var newAccessToken = _tokenService.GenerateToken(userGetDto);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenToDB = new RefreshToken()
        {
            Token = newRefreshToken,
            Expires = DateTime.UtcNow.AddDays(21),
            IsRevoked = false,
            UserId = user.UserId
        };

        await _refTokRepo.AddRefreshToken(refreshTokenToDB);

        return new LoginResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            TokenType = "Bearer",
            Expires = 24
        };
    }

    //public async Task LogOut(string token) => await _refTokRepo.DeleteRefreshToken(token);
    public async Task LogOut(string token)
    {
        var refreshToken = await _refTokRepo.SelectRefreshToken(token, 0);

        await _refTokRepo.DeleteRefreshToken(token);
    }
}
