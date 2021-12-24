using MediatR;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Commands;

public class AttachDataSetCommand : IRequest<bool>
{
    public IDataSet DataSet { get; init; }

    public AttachDataSetCommand()
    {
        
    }

    public AttachDataSetCommand(IDataSet dataSet)
    {
        this.DataSet = dataSet;
    }
}

public class AttachDataSetCommandHandler : IRequestHandler<AttachDataSetCommand, bool>
{
    
    #region Members
    private readonly IDataSetCollection _dataSetCollection;
    #endregion

    #region ctor
    public AttachDataSetCommandHandler(IDataSetCollection dataSetCollection)
    {
        _dataSetCollection = dataSetCollection ?? throw new ArgumentNullException(nameof(dataSetCollection));
    }
    #endregion

    #region Methods
    public Task<bool> Handle(AttachDataSetCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dataSetCollection.AttachDataSet(request.DataSet));
    }
    #endregion
    
}