using System.Reflection;
using MediatR;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

public class GetRegisteredViewsQuery : IRequest<IEnumerable<ViewInformation>>
{
    
}

public class GetRegisteredViewsQueryHandler : IRequestHandler<GetRegisteredViewsQuery, IEnumerable<ViewInformation>>
{
    
    public Task<IEnumerable<ViewInformation>> Handle(GetRegisteredViewsQuery request, CancellationToken cancellationToken)
    {
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
