using System;

namespace STAIExtensions.Core
{
    public class AIQueryAPI : Abstractions.Interfaces.IAIQueryAPI
    {

        #region Members
        private const string _queryAPIBaseUrl = "https://api.applicationinsights.io/v1/apps/{app-id}/query";
        private readonly string _queryAPIUrl;
        #endregion

        #region Properties
        public string AppId { get; }
        #endregion
        
        #region ctor

        public AIQueryAPI(string appId)
        {
            if (string.IsNullOrEmpty(appId) || appId.Trim() == "")
                throw new ArgumentNullException(nameof(appId));
            this.AppId = appId;
            this._queryAPIUrl = _queryAPIBaseUrl.Replace("{app-id}", this.AppId);
        }
        #endregion
        
        
    }
}