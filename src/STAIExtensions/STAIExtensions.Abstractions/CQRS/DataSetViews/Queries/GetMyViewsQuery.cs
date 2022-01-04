using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

public class GetMyViewsQuery : IRequest<IEnumerable<MyViewInformation>>
{

    #region Properties

    public string OwnerId { get; set; }

    #endregion

    public GetMyViewsQuery(string ownerId)
    {
        this.OwnerId = ownerId;
    }
}

public class GetMyViewsQueryHandler : IRequestHandler<GetMyViewsQuery, IEnumerable<MyViewInformation>>
{
    
    #region Members

    private readonly IViewCollection _viewCollection;

    #endregion

    #region ctor
    public GetMyViewsQueryHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
    }
    #endregion
    
    public Task<IEnumerable<MyViewInformation>> Handle(GetMyViewsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_viewCollection.GetMyViews(request.OwnerId ));
    }
}
