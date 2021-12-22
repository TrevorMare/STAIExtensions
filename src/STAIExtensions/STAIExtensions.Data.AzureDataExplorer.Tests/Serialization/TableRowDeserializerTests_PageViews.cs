using STAIExtensions.Abstractions.DataContracts.Models;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests_PageViews : TableRowDeserializerTests_Base<PageView>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_PageViews.json";
    protected override string TableName => "pageViews";
    
}