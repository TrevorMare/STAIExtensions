using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class CleanupViewsCommand : IRequest<int>
{
    
}

public class CleanupViewsCommandHandler : IRequestHandler<CleanupViewsCommand, int>
{
    #region Members

    private readonly IViewCollection _viewCollection;
    private readonly IDataSetCollection _dataSetCollection;

    #endregion

    #region ctor
    public CleanupViewsCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
    }
    #endregion

    #region Methods

    public Task<int> Handle(CleanupViewsCommand request, CancellationToken cancellationToken)
    {
        var expiredViews = _viewCollection.GetExpiredViews();

        var dataSetViews = expiredViews as IDataSetView[] ?? expiredViews.ToArray();
        
        if (!dataSetViews.Any()) return Task.FromResult(0);

        foreach (var expiredView in dataSetViews)
        {
            _dataSetCollection.RemoveViewFromDataSets(expiredView.Id);
            _viewCollection.RemoveView(expiredView);
        }

        return Task.FromResult(dataSetViews.Count());
    }

    #endregion
    
}

// Find all the views that is past the expiry date


// Detach the views from the datasets

// Remove the datasets from memory