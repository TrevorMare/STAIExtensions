using System.Linq;
using STAIExtensions.Abstractions.WebApi;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_PerformanceCounters : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.PerformanceCounter>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_PerformanceCounters.json";
    protected override string TableName => "performanceCounters";
    
}