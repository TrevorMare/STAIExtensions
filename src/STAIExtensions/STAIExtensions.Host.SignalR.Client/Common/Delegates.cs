namespace STAIExtensions.Host.SignalR.Client.Common;

/// <summary>
/// Model structure for when the Data Set has been updated
/// </summary>
public delegate void OnDataSetUpdatedHandler(object sender, string dataSetId);

/// <summary>
/// Model structure for when the Data Set View has been updated
/// </summary>
public delegate void OnDataSetViewUpdatedHandler(object sender, string viewId);

/// <summary>
/// Model structure for when the Data Set has been updated, but with additional Json payload
/// </summary>
public delegate void OnDataViewUpdatedHandler(object sender, ViewDetail view);
