namespace STAIExtensions.Host.SignalR.Client.Common;

public delegate void OnDataSetUpdatedHandler(object sender, string dataSetId);

public delegate void OnDataSetViewUpdatedHandler(object sender, string viewId);

public delegate void OnDataViewUpdatedHandler(object sender, ViewDetail view);
