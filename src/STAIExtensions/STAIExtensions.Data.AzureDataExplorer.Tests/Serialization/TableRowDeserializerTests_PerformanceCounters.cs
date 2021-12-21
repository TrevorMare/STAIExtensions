using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_PerformanceCounters : TableRowDeserializerTests_Base<PerformanceCounter>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_PerformanceCounters.json";
    protected override string TableName => "performanceCounters";
    
}