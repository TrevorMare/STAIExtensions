using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_CustomMetrics : TableRowDeserializerTests_Base<CustomMetric>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_CustomMetrics.json";
    protected override string TableName => "customMetrics";
    
}