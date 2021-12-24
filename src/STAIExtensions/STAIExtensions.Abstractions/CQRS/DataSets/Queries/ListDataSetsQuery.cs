using MediatR;
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

    public ListDataSetsQueryHandler(IDataSetCollection dataSetCollection)
    {
        this._dataSetCollection = dataSetCollection ?? throw new ArgumentNullException(nameof(dataSetCollection));
    }

    #endregion

    #region Methods
    public Task<IEnumerable<DataSetInformation>> Handle(ListDataSetsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(this._dataSetCollection.ListDataSets());
    }
    #endregion
    
}