using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

public class GetDataViewQuery : IRequest<IDataSetView?>
{

    public string ViewId { get; set; } = "";

    public string UserSessionId { get; set; } = "";

    #region ctor

    public GetDataViewQuery() {}

    public GetDataViewQuery(string viewId, string userSessionId)
    {
        this.ViewId = viewId;
        this.UserSessionId = userSessionId;
    }
    #endregion
    
}

public class GetDataViewQueryHandler : IRequestHandler<GetDataViewQuery, IDataSetView?>
{

    #region Members

    private readonly IViewCollection _viewCollection;

    #endregion

    #region ctor
    public GetDataViewQueryHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
    }
    #endregion

    #region Methods
    public Task<IDataSetView?> Handle(GetDataViewQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_viewCollection.GetView(request.ViewId, request.UserSessionId ));
    }
    #endregion
    
}