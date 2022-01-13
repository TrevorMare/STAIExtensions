using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Views;

namespace STAIExtensions.Host.SignalR.Client.Common;

public class ViewBase 
{
    public string Id { get; set; }
    public string? OwnerId { get; set; }
    public DateTime? ExpiryDate { get; set;}
    public DateTime? LastUpdate { get; set;}
    public string ViewTypeName { get; set;}
    public bool RefreshEnabled { get; set;}
    public IEnumerable<DataSetViewParameterDescriptor>? ViewParameterDescriptors { get; set;}
    public TimeSpan SlidingExpiration { get; set; }

}