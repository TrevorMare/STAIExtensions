using MediatR;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.Queries;

public class ListDataSetsQuery : IRequest<IEnumerable<string>>
{
    
    
    
}

public class ListDataSetsQueryHandler : IRequestHandler<ListDataSetsQuery, IEnumerable<string>>
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
    public Task<IEnumerable<string>> Handle(ListDataSetsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(this._dataSetCollection.ListDataSets());
    }
    #endregion
}