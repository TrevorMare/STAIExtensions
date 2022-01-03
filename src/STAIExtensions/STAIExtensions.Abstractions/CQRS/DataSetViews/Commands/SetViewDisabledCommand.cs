using MediatR;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class SetViewDisabledCommand : IRequest<bool>
{

    #region Properties

    public string ViewId { get; set; }

    public string OwnerId { get; set; }

    #endregion
    
    #region ctor

    public SetViewDisabledCommand(string viewId, string ownerId)
    {
        this.ViewId = viewId;
        this.OwnerId = ownerId;
    }
    #endregion
    
}

public class SetViewDisabledCommandHandler : IRequestHandler<SetViewDisabledCommand, bool>
{
    
    #region Members
    private readonly IViewCollection _viewCollection;
    #endregion

    #region ctor
    public SetViewDisabledCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
    }
    #endregion

    #region Methods
    public Task<bool> Handle(SetViewDisabledCommand request, CancellationToken cancellationToken)
    {
        var result = false;

        var view = _viewCollection.GetView(request.ViewId, request.OwnerId);
        if (view != null)
        {
            result = view.RefreshEnabled == true;
            view.SetViewRefreshDisabled();
        }

        return Task.FromResult(result);
    }
    #endregion
    
}