using System.Linq;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_Dependencies : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.Dependency>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Dependencies.json";
    protected override string TableName => "dependencies";
    
}