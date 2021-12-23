using MediatR;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.Commands;

public class AttachViewToDataSetCommand : IRequest<bool>
{
    
    #region Properties

    public string ViewId { get; init; } = "";

    public string DataSetId { get; init; } = "";

    public string UserSessionId { get; init; } = "";

    #endregion

    #region ctor

    public AttachViewToDataSetCommand()
    {
        
    }

    public AttachViewToDataSetCommand(string viewId, string dataSetId, string userSessionId)
    {
        this.ViewId = viewId;
        this.DataSetId = dataSetId;
        this.UserSessionId = userSessionId;
    }
    #endregion
}


public class AttachViewToDataSetCommandHandler : IRequestHandler<AttachViewToDataSetCommand, bool>
{

    #region Members

    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;

    #endregion

    #region ctor
    public AttachViewToDataSetCommandHandler(IViewCollection viewCollection, IDataSetCollection dataSetCollection)
    {
        _viewCollection = viewCollection ?? throw new ArgumentNullException(nameof(viewCollection));
        _dataSetCollection = dataSetCollection ?? throw new ArgumentNullException(nameof(dataSetCollection));
    }
    #endregion

    #region Methods
    public Task<bool> Handle(AttachViewToDataSetCommand request, CancellationToken cancellationToken)
    {
        var view = _viewCollection.GetView(request.ViewId, request.UserSessionId);

        return Task.FromResult(view != null && _dataSetCollection.AttachViewToDataSet(request.DataSetId, view));
    }
    #endregion

   
}