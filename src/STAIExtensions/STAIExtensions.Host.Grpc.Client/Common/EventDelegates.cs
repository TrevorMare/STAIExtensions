namespace STAIExtensions.Host.Grpc.Client.Common;

public delegate void OnDataSetUpdatedHandler(object sender, string dataSetId);
public delegate void OnDataSetViewUpdatedHandler(object sender, string viewId);
public delegate void OnDataSetViewUpdatedJsonHandler(object sender, DataSetViewUpdatedJsonParams viewId);
public delegate void OnConnectionStateChangedHandler(object sender, ConnectionState connectionState);