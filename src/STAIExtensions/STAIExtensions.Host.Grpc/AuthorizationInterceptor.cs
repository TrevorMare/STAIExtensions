using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;

namespace STAIExtensions.Host.Grpc;

public class AuthorizationInterceptor : Interceptor
{
    private readonly GrpcHostOptions _options;

    public AuthorizationInterceptor(GrpcHostOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        if (!_options.UseDefaultAuthorization) return await continuation(request, context);
        
        var extractedToken = ExtractContextAuthorizationKey(context) ?? "";
        
        if (string.Equals(extractedToken.Trim(), _options.BearerToken.Trim(), StringComparison.OrdinalIgnoreCase))
            return await continuation(request, context);
        
        var httpContext = context.GetHttpContext();
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        throw new RpcException(new Status(StatusCode.PermissionDenied,
            "Authorization token is invalid or not supplied"));
    }
  

    private string? ExtractContextAuthorizationKey(ServerCallContext context)
    {
        var authHeader = context.RequestHeaders.Select(x => x)
            .FirstOrDefault(x => x.Key.ToLower() == "authorization");
        return authHeader?.Value.ToLower().Replace("bearer ", "");
    }
    
}