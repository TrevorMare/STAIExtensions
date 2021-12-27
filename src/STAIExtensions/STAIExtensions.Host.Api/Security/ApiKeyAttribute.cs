using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace STAIExtensions.Host.Api.Security;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{

    #region MyRegion

    private ApiOptions? _options;

    #endregion

    #region ctor

    public ApiKeyAttribute()
    {
        
    }

    #endregion

    #region Methods
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _options = (ApiOptions?)context.HttpContext.RequestServices.
            GetService(typeof(ApiOptions));

        if (_options?.UseAuthorization == true)
        {
            if (!ExtractApiKey(context, out var extractedApiKey)) return;

            if (!IsApiKeyValid(context, extractedApiKey)) return;
        }
        await next();
    }

    private bool IsApiKeyValid(ActionExecutingContext context, string extractedApiKey)
    {
        if (!_options.AllowedApiKeys.Any(key => string.Equals(key, extractedApiKey, StringComparison.OrdinalIgnoreCase)))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = "Api Key is not valid"
            };
            return false;
        }

        return true;
    }

    private bool ExtractApiKey(ActionExecutingContext context, out StringValues extractedApiKey)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(_options.HeaderName, out extractedApiKey))
        {
            context.Result = new ContentResult()
            {
                StatusCode = 401,
                Content = $"Api Key was not provided in the header {_options.HeaderName}"
            };
            return false;
        }
        return true;
    }

    #endregion
    
}