using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class UpdateViewFromDataSetCommand : IRequest<bool>
{
    public string ViewId { get; set; }

    public IDataSet DataSet { get; set; }

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
    public UpdateViewFromDataSetCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
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
