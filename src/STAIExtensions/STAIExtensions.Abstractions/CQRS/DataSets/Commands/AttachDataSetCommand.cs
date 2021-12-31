using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;

namespace STAIExtensions.Abstractions.CQRS.DataSets.Commands;

public class AttachDataSetCommand : IRequest<bool>
{
    public IDataSet DataSet { get; set; } 

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
    public AttachDataSetCommandHandler()
    {
        _dataSetCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IDataSetCollection>() ??
                             throw new Exception("Could not retrieve data set collection from DI");
    }
    #endregion

    #region Methods
    public Task<bool> Handle(AttachDataSetCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_dataSetCollection.AttachDataSet(request.DataSet));
    }
    #endregion
    
}