using MediatR;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class DetachViewFromDataSetCommand : IRequest<bool>
{
    #region Properties

    public string ViewId { get; init; } = "";

    public string DataSetId { get; init; } = "";

    public string UserSessionId { get; init; } = "";

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
    public DetachViewFromDataSetCommandHandler(IViewCollection viewCollection, IDataSetCollection dataSetCollection)
    {
        _viewCollection = viewCollection ?? throw new ArgumentNullException(nameof(viewCollection));
        _dataSetCollection = dataSetCollection ?? throw new ArgumentNullException(nameof(dataSetCollection));
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
