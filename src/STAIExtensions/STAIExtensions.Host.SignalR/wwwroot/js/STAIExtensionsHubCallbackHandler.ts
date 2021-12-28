interface ICallbackMethod {
    CallbackId: string,
    CallbackName: string,
    CallbackFunc: any
}

class STAIExtensionsHubCallbackHandler {
    
    private _awaitingCallbacks: Map<string, ICallbackMethod>;
    
    constructor() {
        this._awaitingCallbacks = new Map<string, ICallbackMethod>();
    }
    
    public RemoveCallback(callbackId: string): void {
        this._awaitingCallbacks.delete(callbackId);
    }
    
    public OnCallbackReceived(callbackId: string, args: any[]): void {
        if (this._awaitingCallbacks.has(callbackId)) {
            const callback = this._awaitingCallbacks.get(callbackId);
            
            if (callback.CallbackFunc !== undefined && callback.CallbackFunc !== null) {
                callback.CallbackFunc(...args);                
            }
            
            this.RemoveCallback(callbackId);
        }
    }
    
    public PushAwaitCallback(callback: ICallbackMethod): void {
        if (callback !== null) {
            this._awaitingCallbacks.set(callback.CallbackId, callback);            
        }
    }
}