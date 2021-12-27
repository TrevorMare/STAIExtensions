using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Commands;


public class DetachDataSetCommand : IRequest<bool>
{
    public IDataSet DataSet { get; init; }

    public DetachDataSetCommand()
    {
        
    }

    public DetachDataSetCommand(IDataSet dataSet)
    {
        this.DataSet = dataSet;
    }
}

public class DetachDataSetCommandHandler : IRequestHandler<DetachDataSetCommand, bool>
{
    
    #region Members
    private readonly IDataSetCollection _dataSetCollection;
    #endregion

    #region ctor
    public DetachDataSetCommandHandler()
    {
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
    }
    #endregion

    #region Methods
    public Task<bool> Handle(DetachDataSetCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dataSetCollection.DetachDataSet(request.DataSet));
    }
    #endregion
    
}