using STAIExtensions.Data.AzureDataExplorer.Attributes;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Fixtures;

public class TestSerializationClass
{

    public string TestColumnName { get; set; }
    
    [DataContractField("AttrColumn", typeof(Data.AzureDataExplorer.Serialization.CustomDimensionSeriliazation))]
    public string AttributeColumnName { get; set; }
    
    public string GetOnlyColumnName { get; }
    
}