using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Host.Grpc;

internal class ExceptionInterceptor : Interceptor
{
    
    private readonly ILogger<ExceptionInterceptor>? _logger;
 
    public ExceptionInterceptor()
    {
        _logger = Abstractions.DependencyExtensions.CreateLogger<ExceptionInterceptor>();
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await continuation(request, context);
        }
        catch (Exception exception)
        {
            var httpContext = context.GetHttpContext();
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            throw new RpcException(new Status(StatusCode.Internal, exception.ToString()));
        }
    }
    
}