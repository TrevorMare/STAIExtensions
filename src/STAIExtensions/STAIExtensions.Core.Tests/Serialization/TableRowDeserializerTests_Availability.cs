using System.Linq;
using STAIExtensions.Abstractions.ApiClient.Models;
using STAIExtensions.Abstractions.WebApi;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_Availability : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.Availability>
{
    protected override string FixtureFilePath => "Fixtures/datacontract_partial_AvailabilityResults.json";
    protected override string TableName => "availabilityResults";
    
}