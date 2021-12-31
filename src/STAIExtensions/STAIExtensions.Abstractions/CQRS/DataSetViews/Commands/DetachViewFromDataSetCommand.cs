using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class DetachViewFromDataSetCommand : IRequest<bool>
{
    #region Properties

    public string ViewId { get; set; } = "";

    public string DataSetId { get; set; } = "";

    public string UserSessionId { get; set; } = "";

    #endregion

    #region ctor

    public DetachViewFromDataSetCommand()
    {
        
    }

    public DetachViewFromDataSetCommand(string viewId, string dataSetId, string userSessionId)
    {
        this.ViewId = viewId;
        this.DataSetId = dataSetId;
        this.UserSessionId = userSessionId;
    }
    #endregion
}

public class DetachViewFromDataSetCommandHandler : IRequestHandler<DetachViewFromDataSetCommand, bool>
{
    
    #region Members

    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;

    #endregion

    #region ctor
    public DetachViewFromDataSetCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
    }
    #endregion

    #region Methods
    public Task<bool> Handle(DetachViewFromDataSetCommand request, CancellationToken cancellationToken)
    {
        var view = _viewCollection.GetView(request.ViewId, request.UserSessionId);

        return Task.FromResult(view != null && _dataSetCollection.DetachViewFromDataSet(request.DataSetId, view));
    }
    #endregion
}
