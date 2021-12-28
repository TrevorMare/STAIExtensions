var System;
(function (System) {
    var Guid = (function () {
        function Guid(guid) {
            this.guid = guid;
            this._guid = guid;
        }
        Guid.prototype.ToString = function () {
            return this.guid;
        };
        Guid.MakeNew = function () {
            var result;
            var i;
            var j;
            result = "";
            for (j = 0; j < 32; j++) {
                if (j == 8 || j == 12 || j == 16 || j == 20)
                    result = result + '-';
                i = Math.floor(Math.random() * 16).toString(16).toUpperCase();
                result = result + i;
            }
            return new Guid(result);
        };
        return Guid;
    }());
    System.Guid = Guid;
})(System || (System = {}));
var STAIExtensionsHub = (function () {
    function STAIExtensionsHub(ownerId, connectionEndpoint, accessToken, dataSetUpdatedCallback, dataSetViewUpdatedCallback) {
        var _this = this;
        if (connectionEndpoint === void 0) { connectionEndpoint = "https://localhost:44309/STAIExtensionsHub"; }
        if (accessToken === void 0) { accessToken = null; }
        if (dataSetUpdatedCallback === void 0) { dataSetUpdatedCallback = null; }
        if (dataSetViewUpdatedCallback === void 0) { dataSetViewUpdatedCallback = null; }
        this._connectionSuccess = false;
        this._callbackHandler = new STAIExtensionsHubCallbackHandler();
        if (ownerId === null || ownerId === '') {
            throw 'The owner Id is required';
        }
        this._ownerId = ownerId;
        this._connectionEndpoint = connectionEndpoint;
        this._accessToken = accessToken;
        this._dataSetUpdatedCallback = dataSetUpdatedCallback;
        this._dataSetViewUpdatedCallback = dataSetViewUpdatedCallback;
        this._connection = new signalR.HubConnectionBuilder()
            .withUrl(this._connectionEndpoint, { accessTokenFactory: function () { return _this._accessToken; } })
            .build();
        this.SetupHandlers();
        this.StartConnection();
    }
    STAIExtensionsHub.prototype.StartConnection = function () {
        var instance = this;
        this._connection.start().then(function () {
            instance._connectionSuccess = true;
        })["catch"](function (err) {
            return console.error(err.toString());
        });
    };
    STAIExtensionsHub.prototype.ValidateConnection = function () {
        if (this._connectionSuccess == false) {
            throw 'Unable to call the Hub Method, Connection was not established';
        }
    };
    STAIExtensionsHub.prototype.GenerateCallbackId = function () {
        return System.Guid.MakeNew().ToString();
    };
    STAIExtensionsHub.prototype.SetupHandlers = function () {
        var instance = this;
        this._connection.on("OnDataSetUpdated", function (dataSetId) {
            console.log('OnDataSetUpdated');
            if (instance._dataSetUpdatedCallback !== null) {
                instance._dataSetUpdatedCallback(dataSetId);
            }
        });
        this._connection.on("OnDataSetViewUpdated", function (dataSetViewId) {
            console.log('OnDataSetViewUpdated');
            if (instance._dataSetViewUpdatedCallback !== null) {
                instance._dataSetViewUpdatedCallback(dataSetViewId);
            }
        });
        this._connection.on("OnDataSetViewCreated", function (iView, callbackId) {
            console.log('OnDataSetViewCreated');
            instance._callbackHandler.OnCallbackReceived(callbackId, iView);
        });
        this._connection.on("OnGetViewResponse", function (iView, callbackId) {
            console.log('OnGetViewResponse');
            instance._callbackHandler.OnCallbackReceived(callbackId, iView);
        });
        this._connection.on("OnListDataSetsResponse", function (response, callbackId) {
            console.log('OnListDataSetsResponse');
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });
        this._connection.on("OnGetRegisteredViewsResponse", function (response, callbackId) {
            console.log('OnGetRegisteredViewsResponse');
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });
        this._connection.on("OnRemoveViewResponse", function (response, callbackId) {
            console.log('OnRemoveViewResponse');
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });
        this._connection.on("OnAttachViewToDatasetResponse", function (response, callbackId) {
            console.log('OnAttachViewToDatasetResponse');
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });
        this._connection.on("OnDetachViewFromDatasetResponse", function (response, callbackId) {
            console.log('OnDetachViewFromDatasetResponse');
            instance._callbackHandler.OnCallbackReceived(callbackId, response);
        });
    };
    STAIExtensionsHub.prototype.CreateView = function (viewType, callback) {
        this.ValidateConnection();
        var callbackId = this.GenerateCallbackId();
        var instance = this;
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId: callbackId, CallbackName: "CreateView" });
        }
        console.log('CreateView');
        this._connection.invoke("CreateView", viewType, this._ownerId, callbackId)["catch"](function (err) {
            instance._callbackHandler.RemoveCallback(callbackId);
            return console.error(err.toString());
        });
    };
    STAIExtensionsHub.prototype.GetView = function (viewId, callback) {
        this.ValidateConnection();
        var callbackId = this.GenerateCallbackId();
        var instance = this;
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId: callbackId, CallbackName: "GetView" });
        }
        this._connection.invoke("GetView", viewId, this._ownerId, callbackId)["catch"](function (err) {
            instance._callbackHandler.RemoveCallback(callbackId);
            return console.error(err.toString());
        });
    };
    STAIExtensionsHub.prototype.ListDataSets = function (callback) {
        this.ValidateConnection();
        var callbackId = this.GenerateCallbackId();
        var instance = this;
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId: callbackId, CallbackName: "ListDataSets" });
        }
        this._connection.invoke("ListDataSets", callbackId)["catch"](function (err) {
            instance._callbackHandler.RemoveCallback(callbackId);
            return console.error(err.toString());
        });
    };
    STAIExtensionsHub.prototype.GetRegisteredViews = function (callback) {
        this.ValidateConnection();
        var callbackId = this.GenerateCallbackId();
        var instance = this;
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId: callbackId, CallbackName: "GetRegisteredViews" });
        }
        this._connection.invoke("GetRegisteredViews", callbackId)["catch"](function (err) {
            instance._callbackHandler.RemoveCallback(callbackId);
            return console.error(err.toString());
        });
    };
    STAIExtensionsHub.prototype.RemoveView = function (viewId, callback) {
        this.ValidateConnection();
        var callbackId = this.GenerateCallbackId();
        var instance = this;
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId: callbackId, CallbackName: "RemoveView" });
        }
        this._connection.invoke("RemoveView", viewId, callbackId)["catch"](function (err) {
            instance._callbackHandler.RemoveCallback(callbackId);
            return console.error(err.toString());
        });
    };
    STAIExtensionsHub.prototype.AttachViewToDataset = function (viewId, datasetId, callback) {
        this.ValidateConnection();
        var callbackId = this.GenerateCallbackId();
        var instance = this;
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId: callbackId, CallbackName: "AttachViewToDataset" });
        }
        this._connection.invoke("AttachViewToDataset", viewId, datasetId, callbackId)["catch"](function (err) {
            instance._callbackHandler.RemoveCallback(callbackId);
            return console.error(err.toString());
        });
    };
    STAIExtensionsHub.prototype.DetachViewFromDataset = function (viewId, datasetId, callback) {
        this.ValidateConnection();
        var callbackId = this.GenerateCallbackId();
        var instance = this;
        if (callback !== undefined && callback !== null) {
            this._callbackHandler.PushAwaitCallback({ CallbackFunc: callback, CallbackId: callbackId, CallbackName: "DetachViewFromDataset" });
        }
        this._connection.invoke("DetachViewFromDataset", viewId, datasetId, callbackId)["catch"](function (err) {
            instance._callbackHandler.RemoveCallback(callbackId);
            return console.error(err.toString());
        });
    };
    return STAIExtensionsHub;
}());
var STAIExtensionsHubCallbackHandler = (function () {
    function STAIExtensionsHubCallbackHandler() {
        this._awaitingCallbacks = new Map();
    }
    STAIExtensionsHubCallbackHandler.prototype.RemoveCallback = function (callbackId) {
        this._awaitingCallbacks["delete"](callbackId);
    };
    STAIExtensionsHubCallbackHandler.prototype.OnCallbackReceived = function (callbackId, args) {
        if (this._awaitingCallbacks.has(callbackId)) {
            var callback = this._awaitingCallbacks.get(callbackId);
            if (callback.CallbackFunc !== undefined && callback.CallbackFunc !== null) {
                callback.CallbackFunc(args);
            }
            this.RemoveCallback(callbackId);
        }
    };
    STAIExtensionsHubCallbackHandler.prototype.PushAwaitCallback = function (callback) {
        if (callback !== null) {
            this._awaitingCallbacks.set(callback.CallbackId, callback);
        }
    };
    return STAIExtensionsHubCallbackHandler;
}());
//# sourceMappingURL=STAIExtensions.js.map