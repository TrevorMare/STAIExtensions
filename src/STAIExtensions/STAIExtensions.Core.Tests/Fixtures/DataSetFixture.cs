using System.Collections.Generic;
using System.Threading.Tasks;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Abstractions.Queries;

namespace STAIExtensions.Core.Tests.Fixtures;

public class DataSetFixture : Core.DataSets.DataSet
{
    public DataSetFixture(ITelemetryLoader telemetryLoader, string? dataSetName = default) : base(telemetryLoader, dataSetName)
    {
    }

    protected override Task ExecuteQueries()
    {
        throw new System.NotImplementedException();
    }

    protected override Task ProcessQueryRecords<T>(DataContractQuery<T> query, IEnumerable<T> records)
    {
        throw new System.NotImplementedException();
    }
}