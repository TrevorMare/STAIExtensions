using System.Linq;
using STAIExtensions.Abstractions.WebApi;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_Traces : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.Trace>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Traces.json";
    protected override string TableName => "traces";
    
}