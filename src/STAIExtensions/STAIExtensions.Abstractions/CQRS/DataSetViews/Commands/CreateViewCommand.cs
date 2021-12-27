using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class CreateViewCommand : IRequest<IDataSetView>
{

    public string ViewType { get; init; } = "";

    public string UserSessionId { get; init; } = "";

    public CreateViewCommand()
    {
        
    }

    public CreateViewCommand(string viewType, string userSessionId)
    {
        this.ViewType = viewType;
        this.UserSessionId = userSessionId;
    }
}

public class CreateViewCommandHandler : IRequestHandler<CreateViewCommand, IDataSetView>
{

    #region Members

    private readonly IViewCollection _viewCollection;

    #endregion

    #region ctor
    public CreateViewCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
    }
    #endregion

    #region Methods
    public Task<IDataSetView> Handle(CreateViewCommand request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_viewCollection.CreateView(request.ViewType, request.UserSessionId));
    }
    #endregion
    
}