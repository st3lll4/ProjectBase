using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.DAL.EF;
using App.Domain.Identity;
using Asp.Versioning;
using Base.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTOs;
using WebApp.DTOs.Identity;

namespace WebApp.ApiControllers.Identity;

/// <summary>
/// Handles user account operations and authentication
/// </summary>
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/account/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly Random _random = new();
    private readonly AppDbContext _context;
    private const string UserPassProblem = "User/Password problem";
    private const int RandomDelayMin = 500;
    private const int RandomDelayMax = 3000;

    private const string SettingsJWTPrefix = "JWTSecurity";
    private const string SettingsJWTKey = SettingsJWTPrefix + ":Key";
    private const string SettingsJWTIssuer = SettingsJWTPrefix + ":Issuer";
    private const string SettingsJWTAudience = SettingsJWTPrefix + ":Audience";
    private const string SettingsJWTExpiresInSeconds = SettingsJWTPrefix + ":ExpiresInSeconds";
    private const string SettingsJWTRefreshTokenExpiresInSeconds = SettingsJWTPrefix + ":RefreshTokenExpiresInSeconds";

    /// <summary>
    /// Constructor
    /// </summary>
    public AccountController(IConfiguration configuration, UserManager<AppUser> userManager,
        ILogger<AccountController> logger, SignInManager<AppUser> signInManager, AppDbContext context)
    {
        _configuration = configuration;
        _userManager = userManager;
        _logger = logger;
        _signInManager = signInManager;
        _context = context;
    }


    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    /// <param name="loginInfo">Login model</param>
    /// <param name="jwtExpiresInSeconds">Optional, use custom jwt expiration</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional, use custom refresh token expiration</param>
    /// <returns>JWT and refresh token</returns>
    /// <response code="200">Returns the JWT token</response>
    /// <response code="400">If the credentials are invalid</response>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult<JWTResponse>> Login(
        [FromBody] LoginRequest loginInfo,
        [FromQuery] int jwtExpiresInSeconds,
        [FromQuery] int refreshTokenExpiresInSeconds
    )
    {
        // verify user
        var appUser = await _userManager.FindByEmailAsync(loginInfo.Email);
        if (appUser == null)
        {
            _logger.LogWarning("WebApi login failed, email {} not found", loginInfo.Email);
            await Task.Delay(_random.Next(RandomDelayMin, RandomDelayMax));

            return NotFound(new ApiErrorResponse
            {
                ErrorMessage = UserPassProblem
            });
        }

        // verify password
        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginInfo.Password, false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("WebApi login failed, password {} for email {} was wrong", loginInfo.Password,
                loginInfo.Email);
            await Task.Delay(_random.Next(RandomDelayMin, RandomDelayMax));

            return NotFound(new ApiErrorResponse
            {
                ErrorMessage = UserPassProblem
            });
        }

        // create UserPrincipal, uhh problematic?
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        if (!_context.Database.ProviderName!.Contains("InMemory"))
        {
            var deletedRows = await _context.AppRefreshTokens
                .Where(t => t.UserId == appUser.Id && t.ExpirationDate < DateTime.UtcNow)
                .ExecuteDeleteAsync();
            _logger.LogInformation("Deleted {} refresh tokens", deletedRows);
        }
        else
        {
            var expiredTokens = await _context.AppRefreshTokens
                .Where(token => token.UserId == appUser.Id && token.ExpirationDate < DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Count != 0)
            {
                _context.AppRefreshTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }
        }

        var refreshToken = new AppRefreshToken()
        {
            UserId = appUser.Id,
            ExpirationDate =
                GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds)
        };
        _context.AppRefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );

        var responseData = new JWTResponse()
        {
            Jwt = jwt,
            RefreshToken = refreshToken.RefreshToken
        };

        return Ok(responseData);
    }

    /// <summary>
    /// Register new user, returns JWT and refresh token
    /// </summary>
    /// <param name="registrationData">The registration information</param>
    /// <param name="jwtExpiresInSeconds">Optional custom jwt expiration</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional custom refresh token expiration</param>
    /// <returns>JWTResponse object if registration is successful</returns>
    /// <response code="200">If registration was successful</response>
    /// /// <response code="400">If the registration data is invalid</response>
    [HttpPost]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType<JWTResponse>((int)HttpStatusCode.OK)]
    [ProducesResponseType<JWTResponse>((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<JWTResponse>> Register(
        [FromBody] RegisterRequest registrationData,
        [FromQuery] int? jwtExpiresInSeconds,
        [FromQuery] int? refreshTokenExpiresInSeconds)
    {
        var appUser = await _userManager.FindByEmailAsync(registrationData.Email);
        if (appUser != null)
        {
            _logger.LogWarning("User with email {} is already registered", registrationData.Email);
            return BadRequest(
                new ApiErrorResponse()
                {
                    ErrorMessage = $"User with email {registrationData.Email} is already registered"
                }
            );
        }

        var refreshToken = new AppRefreshToken()
        {
            ExpirationDate =
                GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds)
        };

        appUser = new AppUser()
        {
            Email = registrationData.Email,
            UserName = registrationData.Email,
            FirstName = registrationData.FirstName,
            LastName = registrationData.LastName,
            RefreshTokens = new List<AppRefreshToken>() { refreshToken }
        };

        var res = await _userManager.CreateAsync(appUser, registrationData.Password);

        if (res.Succeeded)
        {
            _logger.LogInformation("User {Email} created a new account with password", appUser.Email);

            await _userManager.AddClaimAsync(appUser, new Claim(ClaimTypes.Email, appUser.Email));

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
            var jwt = IdentityExtensions.GenerateJwt(
                claimsPrincipal.Claims,
                _configuration.GetValue<string>(SettingsJWTKey)!,
                _configuration.GetValue<string>(SettingsJWTIssuer)!,
                _configuration.GetValue<string>(SettingsJWTAudience)!,
                GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
            );
            _logger.LogInformation("WebApi login. User {User}", registrationData.Email);
            return Ok(new JWTResponse()
            {
                Jwt = jwt,
                RefreshToken = refreshToken.RefreshToken,
            });
        }

        var errors = res.Errors.Select(error => error.Description).ToList();
        return BadRequest(new ApiErrorResponse() { ErrorMessage = string.Join("; ", errors) });
    }

    /// <summary>
    /// Renew JWT using a refresh token
    /// </summary>
    /// <param name="refreshTokenModel">Data for renewal</param>
    /// <param name="jwtExpiresInSeconds">Optional custom expiration for jwt</param>
    /// <param name="refreshTokenExpiresInSeconds">Optional custom expiration for refresh token</param>
    /// <returns></returns>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(JWTResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [HttpPost]
    public async Task<ActionResult<JWTResponse>> RenewRefreshToken(
        [FromBody] JWTResponse refreshTokenModel,
        [FromQuery] int? jwtExpiresInSeconds,
        [FromQuery] int? refreshTokenExpiresInSeconds
    )
    {
        JwtSecurityToken jwtToken;
        // get user info from jwt
        try
        {
            jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Jwt);
            if (jwtToken == null)
            {
                return BadRequest(
                    new ApiErrorResponse()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        ErrorMessage = "No JWT token"
                    }
                );
            }
        }
        catch (Exception e)
        {
            return BadRequest(new ApiErrorResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "No JWT token " + e
                }
            );
        }

        // validate jwt, ignore expiration date
        if (!IdentityExtensions.IsJwtValid(
                refreshTokenModel.Jwt,
                _configuration.GetValue<string>(SettingsJWTKey)!,
                _configuration.GetValue<string>(SettingsJWTIssuer)!,
                _configuration.GetValue<string>(SettingsJWTAudience)!
            ))
        {
            return BadRequest("JWT validation fail");
        }

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest(
                new ApiErrorResponse()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "No email in jwt"
                }
            );
        }

        // get user and tokens
        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return NotFound(new ApiErrorResponse()
            {
                ErrorMessage = $"User with email {userEmail} not found"
            });
        }

        // load and compare refresh tokens

        var tokenToUpdate = await _context.AppRefreshTokens
            .Where(x =>
                (x.RefreshToken == refreshTokenModel.RefreshToken && x.ExpirationDate > DateTime.UtcNow) ||
                (x.PreviousRefreshToken == refreshTokenModel.RefreshToken && x.PreviousExpirationDate > DateTime.UtcNow)
            )
            .FirstOrDefaultAsync();

        if (tokenToUpdate == null)
        {
            return Problem("No valid refresh token found");
        }
        
        // get claims based user
        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

        // generate jwt
        var jwt = IdentityExtensions.GenerateJwt(
            claimsPrincipal.Claims,
            _configuration.GetValue<string>(SettingsJWTKey)!,
            _configuration.GetValue<string>(SettingsJWTIssuer)!,
            _configuration.GetValue<string>(SettingsJWTAudience)!,
            GetExpirationDateTime(jwtExpiresInSeconds, SettingsJWTExpiresInSeconds)
        );

        var newRefreshToken = Guid.NewGuid().ToString();
        var newExpirationDate =
            GetExpirationDateTime(refreshTokenExpiresInSeconds, SettingsJWTRefreshTokenExpiresInSeconds);

        // update token ilma mällu laadimata ja välja querymata, kuna ainult 4 rida vaga updateda
        await _context.AppRefreshTokens
            .Where(rt => rt.Id == tokenToUpdate.Id)
            .ExecuteUpdateAsync(setter => setter
                .SetProperty(rt => rt.PreviousRefreshToken, tokenToUpdate.RefreshToken)
                .SetProperty(rt => rt.PreviousExpirationDate, DateTime.UtcNow.AddMinutes(1))
                .SetProperty(rt => rt.RefreshToken, newRefreshToken)
                .SetProperty(rt => rt.ExpirationDate, newExpirationDate));

        var res = new JWTResponse()
        {
            Jwt = jwt,
            RefreshToken = newRefreshToken
        };

        return Ok(res);
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    [HttpPost]
    public async Task<ActionResult> Logout(
        [FromBody] LogoutRequest logout)
    {
        var appUser = await _context.Users
            .Where(u => u.Id == User.GetUserId())
            .SingleOrDefaultAsync();

        if (appUser == null)
        {
            return NotFound(
                new ApiErrorResponse()
                {
                    ErrorMessage = "User/Password problem"
                }
            );
        }

        var tokensToRemove = await _context.AppRefreshTokens
            .Where(x =>
                (x.RefreshToken == logout.RefreshToken) ||
                (x.PreviousRefreshToken == logout.RefreshToken)
            )
            .ToListAsync();

        foreach (var token in tokensToRemove)
        {
            _context.AppRefreshTokens.Remove(token);
        }

        var deleteCount = await _context.SaveChangesAsync();

        return Ok(new { TokenDeleteCount = deleteCount });
        
    }

    // FOR testing????
    private DateTime GetExpirationDateTime(int? expiresInSeconds, string settingsKey)
    {
        if (expiresInSeconds is null or <= 0)
        {
            expiresInSeconds = 600;
        }

        var configValue = _configuration.GetValue<int>(settingsKey);

        expiresInSeconds = expiresInSeconds.Value < configValue
            ? expiresInSeconds
            : configValue;

        return DateTime.UtcNow.AddSeconds(expiresInSeconds.Value);
    }
}
