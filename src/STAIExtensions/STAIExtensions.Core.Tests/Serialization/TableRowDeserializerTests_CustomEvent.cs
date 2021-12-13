using System.Linq;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_CustomEvent : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.CustomEvent>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_CustomEvents.json";
    protected override string TableName => "customEvents";
    
    
    
}