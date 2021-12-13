using System.Linq;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_PageViews : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.PageView>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_PageViews.json";
    protected override string TableName => "pageViews";
    
}