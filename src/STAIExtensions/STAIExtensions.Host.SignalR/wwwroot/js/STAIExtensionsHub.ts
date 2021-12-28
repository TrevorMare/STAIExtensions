﻿///<reference path="Utils.ts"/>

class STAIExtensionsHub {

    private _connection: any;
    private _accessToken: string;
    private _connectionEndpoint: string;
    private _connectionSuccess: Boolean = false;
    private _ownerId: string;
    private _callbackHandler: STAIExtensionsHubCallbackHandler = new STAIExtensionsHubCallbackHandler();

    public constructor(ownerId: string, connectionEndpoint: string = "https://localhost:44309/STAIExtensionsHub", accessToken: string = null) {
        
        if (ownerId === null || ownerId === '') {
            throw 'The owner Id is required';
        }
        
        this._connectionEndpoint = connectionEndpoint;
        this._accessToken = accessToken;
        
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
    
    private GenerateCallbackId(): string {
        return System.Guid.MakeNew().ToString();    
    }
    
    private SetupHandlers() : void {
        const instance = this;
        
        
        this._connection.on("OnDataSetViewCreated", function (iView: any, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, iView);
        });

        this._connection.on("OnGetViewResponse", function (iView: any, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, iView);
        });

        this._connection.on("OnListDataSetsResponse", function (response: any, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnGetRegisteredViewsResponse", function (response: any, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnRemoveViewResponse", function (response: any, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnAttachViewToDatasetResponse", function (response: any, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });

        this._connection.on("OnDetachViewFromDatasetResponse", function (response: any, callbackId: string) {
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });
    }
    
    public CreateView(viewType: string, callback: (iView: any) => any): void {
        this.ValidateConnection();
        const callbackId = this.GenerateCallbackId();
        const instance = this;
        
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId : callbackId, CallbackName: "CreateView"})
        }
        
        this._connection.invoke("CreateView", viewType, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);
            
            // @ts-ignore
            return console.error(err.toString());
        });
    }

    public GetView(viewId: string, callback: (iView: any) => any): void {
        this.ValidateConnection();
        const callbackId = this.GenerateCallbackId();
        const instance = this;

        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId : callbackId, CallbackName: "GetView"})
        }

        this._connection.invoke("GetView", viewId, this._ownerId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);

            // @ts-ignore
            return console.error(err.toString());
        });
    }

    public ListDataSets(callback: (response: any) => any): void {
        this.ValidateConnection();
        const callbackId = this.GenerateCallbackId();
        const instance = this;

        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId : callbackId, CallbackName: "ListDataSets"})
        }

        this._connection.invoke("ListDataSets", callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);

            // @ts-ignore
            return console.error(err.toString());
        });
    }

    public GetRegisteredViews(callback: (response: any) => any): void {
        this.ValidateConnection();
        const callbackId = this.GenerateCallbackId();
        const instance = this;

        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId : callbackId, CallbackName: "GetRegisteredViews"})
        }

        this._connection.invoke("GetRegisteredViews", callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);

            // @ts-ignore
            return console.error(err.toString());
        });
    }

    public RemoveView(viewId: string, callback: (response: any) => any): void {
        this.ValidateConnection();
        const callbackId = this.GenerateCallbackId();
        const instance = this;

        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId : callbackId, CallbackName: "RemoveView"})
        }

        this._connection.invoke("RemoveView", viewId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);

            // @ts-ignore
            return console.error(err.toString());
        });
    }

    public AttachViewToDataset(viewId: string, datasetId: string, callback: (response: any) => any): void {
        this.ValidateConnection();
        const callbackId = this.GenerateCallbackId();
        const instance = this;

        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId : callbackId, CallbackName: "AttachViewToDataset"})
        }

        this._connection.invoke("AttachViewToDataset", viewId, datasetId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);

            // @ts-ignore
            return console.error(err.toString());
        });
    }

    public DetachViewFromDataset(viewId: string, datasetId: string, callback: (response: any) => any): void {
        this.ValidateConnection();
        const callbackId = this.GenerateCallbackId();
        const instance = this;

        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId : callbackId, CallbackName: "DetachViewFromDataset"})
        }

        this._connection.invoke("DetachViewFromDataset", viewId, datasetId, callbackId).catch(function (err: any) {
            instance._callbackHandler.RemoveCallback(callbackId);

            // @ts-ignore
            return console.error(err.toString());
        });
    }



}