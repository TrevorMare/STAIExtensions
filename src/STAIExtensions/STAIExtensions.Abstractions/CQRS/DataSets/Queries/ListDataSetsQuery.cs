using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Common;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Queries;

public class ListDataSetsQuery : IRequest<IEnumerable<DataSetInformation>>
{
    
    
    
}

public class ListDataSetsQueryHandler : IRequestHandler<ListDataSetsQuery, IEnumerable<DataSetInformation>>
{

    #region Members

    private readonly IDataSetCollection _dataSetCollection;

    #endregion
    
    #region ctor

    public ListDataSetsQueryHandler()
    {
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
    }
    #endregion

    #region Methods
    public Task<IEnumerable<DataSetInformation>> Handle(ListDataSetsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(this._dataSetCollection.ListDataSets());
    }
    #endregion
    
}