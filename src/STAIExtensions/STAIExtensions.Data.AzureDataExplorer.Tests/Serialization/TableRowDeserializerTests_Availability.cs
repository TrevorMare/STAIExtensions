using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_Availability : TableRowDeserializerTests_Base<Availability>
{
    protected override string FixtureFilePath => "Fixtures/datacontract_partial_AvailabilityResults.json";
    protected override string TableName => "availabilityResults";
    
}