using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class SetViewParametersCommand : IRequest<bool>
{

    #region Properties

    public string ViewId { get; set; }

    public string OwnerId { get; set; }

    public Dictionary<string, object>? ViewParameters { get; set; }

    #endregion

    #region ctor

    public SetViewParametersCommand(string viewId, string ownerId, Dictionary<string, object>? viewParameters)
    {
        this.ViewId = viewId;
        this.OwnerId = ownerId;
        this.ViewParameters = viewParameters;
    }
    #endregion
    
}

public class SetViewParametersCommandHandler : IRequestHandler<SetViewParametersCommand, bool>
{
   
    #region Members
    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;
    #endregion

    #region ctor
    public SetViewParametersCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
    }
    #endregion
    
    public async Task<bool> Handle(SetViewParametersCommand request, CancellationToken cancellationToken)
    {

        var view = _viewCollection.GetView(request.ViewId, request.OwnerId);

        if (view == null) return false;
        view.SetViewParameters(request.ViewParameters);
        
        var dataSets = _dataSetCollection.FindDataSetsByViewId(request.ViewId);
        if (dataSets == null || dataSets.Any() == false) return true;

        var enumerable = dataSets as IDataSet[] ?? dataSets.ToArray();
        foreach (var dataSet in enumerable)
        {
            await view?.UpdateViewFromDataSet(dataSet)!;
        }
        
        return true;
    }
}