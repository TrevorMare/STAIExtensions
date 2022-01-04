using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class RemoveViewCommand : IRequest<bool>
{
    public string ViewId { get; set; } 
   
    public RemoveViewCommand(string viewId)
    {
        this.ViewId = viewId;
    }
}

public class RemoveViewCommandHandler : IRequestHandler<RemoveViewCommand, bool>
{
      
    #region Members
    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;
    #endregion

    #region ctor
    public RemoveViewCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
    }
    #endregion
    
    public Task<bool> Handle(RemoveViewCommand request, CancellationToken cancellationToken)
    {
        _dataSetCollection.RemoveViewFromDataSets(request.ViewId);
        _viewCollection.RemoveView(request.ViewId);
        return Task.FromResult(true);
    }
}