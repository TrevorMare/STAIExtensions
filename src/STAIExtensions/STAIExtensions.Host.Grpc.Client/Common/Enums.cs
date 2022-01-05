namespace STAIExtensions.Host.Grpc.Client.Common;

public enum ConnectionState
{
    Closed = 0,
    Connecting = 1,
    Connected = 2,
    Reconnecting = 3,
    Failed = 4
}