namespace STAIExtensions.Core.Tests.Fixtures;

public class TestSerializationClass
{

    public string TestColumnName { get; set; }
    
    [Abstractions.Attributes.DataContractField("AttrColumn", typeof(Abstractions.DataContracts.Serialization.CustomDimensionSeriliazation))]
    public string AttributeColumnName { get; set; }
    
    public string GetOnlyColumnName { get; }
    
}