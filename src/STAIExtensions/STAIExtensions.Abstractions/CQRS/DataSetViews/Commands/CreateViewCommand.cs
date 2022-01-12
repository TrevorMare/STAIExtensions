﻿using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.DependencyInjection;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Abstractions.CQRS.DataSetViews.Commands;

public class CreateViewCommand : IRequest<IDataSetView>
{

    public string ViewType { get; set; } = "";

    public string UserSessionId { get; set; } = "";

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
    private readonly TelemetryClient? _telemetryClient;
    #endregion

    #region ctor
    public CreateViewCommandHandler()
    {
        _viewCollection = DependencyExtensions.ServiceProvider?.GetRequiredService<IViewCollection>() ??
                          throw new Exception("Could not retrieve data set views collection from DI");
        this._telemetryClient = DependencyExtensions.TelemetryClient;
    }
    #endregion

    #region Methods
    public Task<IDataSetView> Handle(CreateViewCommand request, CancellationToken cancellationToken)
    {
        using var operation = _telemetryClient?.StartOperation<DependencyTelemetry>($"{this.GetType().Name} - {nameof(Handle)}");
        return Task.FromResult(_viewCollection.CreateView(request.ViewType, request.UserSessionId));
    }
    #endregion
    
}