using STAIExtensions.Data.AzureDataExplorer.DataContracts;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_CustomMetrics : TableRowDeserializerTests_Base<CustomMetrics>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_CustomMetrics.json";
    protected override string TableName => "customMetrics";
    
}