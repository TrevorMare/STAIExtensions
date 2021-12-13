using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.Json;
using STAIExtensions.Core.ApiClient;
using STAIExtensions.Core.Serialization;
using Xunit;

namespace STAIExtensions.Core.Tests.Serialization;

public class TableRowDeserializerTests_Exceptions : TableRowDeserializerTests_Base<Abstractions.DataContracts.Models.Exception>
{

    protected override string FixtureFilePath => "Fixtures/datacontract_partial_Exceptions.json";
    protected override string TableName => "exceptions";
    
}