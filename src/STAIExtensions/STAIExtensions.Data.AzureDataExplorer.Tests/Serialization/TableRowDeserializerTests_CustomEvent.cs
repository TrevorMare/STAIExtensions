using STAIExtensions.Data.AzureDataExplorer.DataContracts;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_CustomEvent : TableRowDeserializerTests_Base<CustomEvent>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_CustomEvents.json";
    protected override string TableName => "customEvents";
    
    
    
}