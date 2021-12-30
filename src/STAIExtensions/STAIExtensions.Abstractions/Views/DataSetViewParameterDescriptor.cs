namespace STAIExtensions.Abstractions.Views;

public class DataSetViewParameterDescriptor
{

    public bool Required { get; set; } = false;

    public string Name { get; set; } = "";

    public string Type { get; set; } = "";

    public string Description { get; set; } = "";
    
    public DataSetViewParameterDescriptor()
    {
        
    }

    public DataSetViewParameterDescriptor(string name, string type, bool required, string description = "")
    {
        this.Required = required;
        this.Name = name;
        this.Type = type;
        this.Description = description;
    }
}