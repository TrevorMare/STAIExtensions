using System.Linq;
using STAIExtensions.Abstractions.WebApi;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_CustomMetrics : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.CustomMetrics>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_CustomMetrics.json";
    protected override string TableName => "customMetrics";
    
}