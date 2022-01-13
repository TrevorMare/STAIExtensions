using System.Text.Json;

namespace STAIExtensions.Host.SignalR.Client.Common;

public class ViewDetail
{
    
    public ViewBase? View { get; private set; } 
    
    public string? FullJson { get; private set;} 


    public ViewDetail(string? value)
    {
        if (string.IsNullOrEmpty(value) || value.Trim() == "") return;
        ParseObject(value);
    }
    
    public ViewDetail(ViewBase view, string fullJson)
    {
        this.View = view;
        this.FullJson = fullJson;
    }

    private void ParseObject(string value)
    {
        var jsonOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        this.View = System.Text.Json.JsonSerializer.Deserialize<ViewBase>(value, jsonOptions);
        this.FullJson = value;
    }
}