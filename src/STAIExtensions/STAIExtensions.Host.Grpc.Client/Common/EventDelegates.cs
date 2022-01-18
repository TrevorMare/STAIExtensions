namespace STAIExtensions.Host.Grpc.Client.Common;

/// <summary>
/// Delegate model for when a Data Set is updated
/// </summary>
public delegate void OnDataSetUpdatedHandler(object sender, string dataSetId);

/// <summary>
/// Delegate model for when a Data View is updated
/// </summary>
public delegate void OnDataSetViewUpdatedHandler(object sender, string viewId);

/// <summary>
/// Delegate model for when a Data View is updated, but contains additional data like the payload of the view
/// </summary>
public delegate void OnDataSetViewUpdatedJsonHandler(object sender, DataSetViewUpdatedJsonParams viewId);

/// <summary>
/// Delegate model for when the connection state of the client manager changes
/// </summary>
public delegate void OnConnectionStateChangedHandler(object sender, ConnectionState connectionState);