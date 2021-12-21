using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_Dependencies : TableRowDeserializerTests_Base<Dependency>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Dependencies.json";
    protected override string TableName => "dependencies";
    
}