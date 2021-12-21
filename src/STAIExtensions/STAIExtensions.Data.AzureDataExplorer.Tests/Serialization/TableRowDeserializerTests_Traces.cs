using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_Traces : TableRowDeserializerTests_Base<Trace>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Traces.json";
    protected override string TableName => "traces";
    
}