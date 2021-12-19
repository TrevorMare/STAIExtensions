using STAIExtensions.Data.AzureDataExplorer.DataContracts;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_BrowserTiming : TableRowDeserializerTests_Base<BrowserTiming>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_BrowserTimings.json";
    protected override string TableName => "browserTimings";
    
    
    
}