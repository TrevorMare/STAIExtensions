using System.Reflection;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

/// <summary>
/// CQRS Query to load information about the types of views registered on the system
/// </summary>
public class GetRegisteredViewsQuery : IRequest<IEnumerable<ViewInformation>>
{
    
}

/// <summary>
/// CQRS Query handler to load information about the types of views registered on the system
/// </summary>
public class GetRegisteredViewsQueryHandler : IRequestHandler<GetRegisteredViewsQuery, IEnumerable<ViewInformation>>
{
    
    #region Members
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public GetRegisteredViewsQueryHandler()
    {
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion
    
    public Task<IEnumerable<ViewInformation>> Handle(GetRegisteredViewsQuery request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        var viewType = typeof(IDataSetView);
        var registeredTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => viewType.IsAssignableFrom(p) && p.IsAbstract == false) ?? new List<Type>();

        var result = (from registeredType in registeredTypes
            let instance = (IDataSetView) Activator.CreateInstance(registeredType)
            select new ViewInformation(registeredType.Name, registeredType.AssemblyQualifiedName,
                instance?.ViewParameterDescriptors)).ToList();

        return Task.FromResult<IEnumerable<ViewInformation>>(result);
    }
}
