using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace STAIExtensions.Host.SignalR;

public class AuthTokenRequirement : IAuthorizationRequirement
{
}

public class AuthTokenRequirementHandler : AuthorizationHandler<AuthTokenRequirement>, IAuthorizationRequirement
{

    #region Members
    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly SignalRHostOptions _options;
    
    #endregion

    #region ctor

    public AuthTokenRequirementHandler(IHttpContextAccessor httpContextAccessor, SignalRHostOptions options)
    {
        this._httpContextAccessor = httpContextAccessor;
        this._options = options ?? throw new ArgumentNullException(nameof(options));
    }

    #endregion
    
    #region Methods

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthTokenRequirement requirement)
    {
        if (this._options.UseDefaultAuthorization == false)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        
        var accessToken = ExtractAccessToken(_httpContextAccessor.HttpContext);
        if (string.IsNullOrEmpty(accessToken))
            return Task.CompletedTask;
        
        if (string.Equals(this._options.BearerToken.Trim(), accessToken.Trim(), StringComparison.OrdinalIgnoreCase))
            context.Succeed(requirement);
        
        return Task.CompletedTask;
    }
    
   
    #endregion

    #region Private Methods

    private string? ExtractAccessToken(HttpContext? context)
    {
        if (context == null) return null;

        var queryAccessToken = context.Request.Query["access_token"];
        if (!string.IsNullOrEmpty(queryAccessToken)) return queryAccessToken;
        
        // Check the headers for the access token
        var headerAccessToken = context.Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(headerAccessToken)) return null;

        return headerAccessToken[0].ToLower().Replace("bearer ", "").Trim();
    }

    #endregion

   
}