///<reference path="Utils.ts"/>
///<reference path="Models.ts"/>
class STAIExtensionsHub {

    private _connection: any;
    private _accessToken: string;
    private _connectionEndpoint: string;
    private _connectionSuccess: Boolean = false;
    private _ownerId: string;
    private _callbackHandler: STAIExtensionsHubCallbackHandler = new STAIExtensionsHubCallbackHandler();
    private _dataSetUpdatedCallback: (dataSetId: string) => {};
    private _dataSetViewUpdatedCallback: (dataSetViewId: string) => {};

    public constructor(ownerId: string, 
                       connectionEndpoint: string = "https://localhost:44309/STAIExtensionsHub", 
                       accessToken: string = null,
                       dataSetUpdatedCallback: (dataSetId: string) => {} = null,
                       dataSetViewUpdatedCallback: (dataSetViewId: string) => {} = null) {
        
        if (ownerId === null || ownerId === '') {
            throw 'The owner Id is required';
        }
        this._ownerId = ownerId;
        this._connectionEndpoint = connectionEndpoint;
        this._accessToken = accessToken;
        this._dataSetUpdatedCallback = dataSetUpdatedCallback;
        this._dataSetViewUpdatedCallback = dataSetViewUpdatedCallback;
        
        // @ts-ignore
        this._connection = new signalR.HubConnectionBuilder()
            .withUrl(this._connectionEndpoint, { accessTokenFactory: () => this._accessToken })
            .build();
        
        this.SetupHandlers();
        
        this.StartConnection();
    }
    
    private StartConnection() : void {
        const instance = this;
        this._connection.start().then(function () {
            instance._connectionSuccess = true;
        }).catch(function (err: any) {
            // @ts-ignore
            return console.error(err.toString());
        });
    }
    
    private ValidateConnection(): void {
        if (this._connectionSuccess == false) {
            throw 'Unable to call the Hub Method, Connection was not established';
        }
    }
    
    private static GenerateCallbackId(): string {
        return System.Guid.MakeNew().ToString();    
    }
    
    private SetupHandlers() : void {
        const instance = this;

        this._connection.on("OnDataSetUpdated", function (dataSetId: string) {
            if (instance._dataSetUpdatedCallback !== null) {
                instance._dataSetUpdatedCallback(dataSetId);
            }
        });

        this._connection.on("OnDataSetViewUpdated", function (dataSetViewId: string) {
            if (instance._dataSetViewUpdatedCallback !== null) {
                instance._dataSetViewUpdatedCallback(dataSetViewId);
            }
        });
        
        this._connection.on("OnDataSetViewCreated", function (iView: IView, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, iView);
        });

        this._connection.on("OnGetViewResponse", function (iView: IView, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, iView);
        });

        this._connection.on("OnListDataSetsResponse", function (response: IDataSetInformation[], callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnGetRegisteredViewsResponse", function (response: IViewInformation[], callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnRemoveViewResponse", function (response: Boolean, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnAttachViewToDatasetResponse", function (response: Boolean, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnDetachViewFromDatasetResponse", function (response: Boolean, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnSetViewParametersResponse", function (response: Boolean, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnSetViewAutoRefreshEnabledResponse", function (response: Boolean, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnSetViewAutoRefreshDisabledResponse", function (response: Boolean, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnGetMyViewsResponse", function (response: IMyView[], callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });
    }
    
    public CreateView(viewType: string, success: (callbackId: string, iView: IView) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;
        
        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "CreateView"})
        }
        // @ts-ignore
        console.log('CreateView');
        this._connection.invoke("CreateView", viewType, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });
        
        return callbackId;
    }

    public GetView(viewId: string, success: (callbackId: string, iView: IView) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "GetView"})
        }

        this._connection.invoke("GetView", viewId, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public ListDataSets(success: (callbackId: string, response: IDataSetInformation[]) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "ListDataSets"})
        }

        this._connection.invoke("ListDataSets", callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public GetRegisteredViews(success: (callbackId: string, response: IViewInformation[]) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "GetRegisteredViews"})
        }

        this._connection.invoke("GetRegisteredViews", callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public RemoveView(viewId: string, success: (callbackId: string, response: Boolean) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "RemoveView"})
        }

        this._connection.invoke("RemoveView", viewId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public AttachViewToDataset(viewId: string, datasetId: string, success: (callbackId: string, response: Boolean) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "AttachViewToDataset"})
        }

        this._connection.invoke("AttachViewToDataset", viewId, datasetId, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });
        return callbackId;
    }

    public DetachViewFromDataset(viewId: string, datasetId: string, success: (callbackId: string, response: Boolean) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "DetachViewFromDataset"})
        }

        this._connection.invoke("DetachViewFromDataset", viewId, datasetId, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public SetViewParameters(viewId: string, viewParameters: any, success: (callbackId: string, response: Boolean) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "SetViewParameters"})
        }

        this._connection.invoke("SetViewParameters", viewId, this._ownerId, viewParameters, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public SetViewAutoRefreshEnabled(viewId: string, success: (callbackId: string, response: Boolean) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "SetViewAutoRefreshEnabled"})
        }

        this._connection.invoke("SetViewAutoRefreshEnabled", viewId, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public SetViewAutoRefreshDisabled(viewId: string, success: (callbackId: string, response: Boolean) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "SetViewAutoRefreshDisabled"})
        }

        this._connection.invoke("SetViewAutoRefreshDisabled", viewId, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }

    public GetMyViews(success: (callbackId: string, response: IMyView[]) => any = null, error: (err: any) => any = null): string {
        this.ValidateConnection();
        const callbackId = STAIExtensionsHub.GenerateCallbackId();
        const instance = this;

        if (success !== undefined && success !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: success, CallbackId : callbackId, CallbackName: "GetMyViews"})
        }

        this._connection.invoke("GetMyViews", callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            if (error !== undefined && error !== null) {
                error(err);
            }
        });

        return callbackId;
    }
}