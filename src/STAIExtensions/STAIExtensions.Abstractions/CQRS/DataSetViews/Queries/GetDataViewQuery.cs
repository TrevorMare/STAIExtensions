using MediatR;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Queries;

public class GetDataViewQuery : IRequest<IDataSetView?>
{

    public string ViewId { get; init; } = "";

    public string UserSessionId { get; init; } = "";

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
    public GetDataViewQueryHandler(IViewCollection viewCollection)
    {
        _viewCollection = viewCollection ?? throw new ArgumentNullException(nameof(viewCollection));
    }
    #endregion

    #region Methods
    public Task<IDataSetView?> Handle(GetDataViewQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_viewCollection.GetView(request.ViewId, request.UserSessionId ));
    }
    #endregion
    
}