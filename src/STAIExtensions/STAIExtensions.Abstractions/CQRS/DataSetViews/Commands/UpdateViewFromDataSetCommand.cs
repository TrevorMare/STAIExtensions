using MediatR;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class UpdateViewFromDataSetCommand : IRequest<bool>
{
    public string ViewId { get; init; } = "";

    public IDataSet DataSet { get; init; }

    public UpdateViewFromDataSetCommand()
    {
        
    }

    public UpdateViewFromDataSetCommand(string viewId, IDataSet dataSet)
    {
        this.ViewId = viewId;
        this.DataSet = dataSet;
    }
}


public class UpdateViewFromDataSetCommandHandler : IRequestHandler<UpdateViewFromDataSetCommand, bool>
{
    
    #region Members
    private readonly IViewCollection _viewCollection;
    #endregion

    #region ctor
    public UpdateViewFromDataSetCommandHandler(IViewCollection viewCollection)
    {
        _viewCollection = viewCollection ?? throw new ArgumentNullException(nameof(viewCollection));
    }
    #endregion

    #region Methods
    public async Task<bool> Handle(UpdateViewFromDataSetCommand request, CancellationToken cancellationToken)
    {
        var view = _viewCollection.GetViewForUpdate(request.ViewId);
        await view?.OnDataSetUpdated(request.DataSet)!;
        return true;
    }
    #endregion
}
