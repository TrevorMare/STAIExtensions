using System;
using System.Linq;
using STAIExtensions.Abstractions.Common;
using Xunit;

namespace STAIExtensions.Core.Tests.Queries;

public class QueryBuilderTests
{
    //
    // [Fact]
    // public void GetDataContractSources_WhenAllPassed_ShouldNotReturnAll()
    // {
    //     var sut = new QueryBuilder();
    //     var actual = sut.GetDataContractSources(AzureApiDataContractSource.All);
    //
    //     Assert.False(actual.Contains(AzureApiDataContractSource.All));
    // }
    //
    // [Fact]
    // public void GetDataContractSources_WhenAllPassed_ShouldNotReturnOtherValues()
    // {
    //     var sut = new QueryBuilder();
    //     var actual = sut.GetDataContractSources(AzureApiDataContractSource.All);
    //
    //     Assert.Equal(Enum.GetValues(typeof(AzureApiDataContractSource)).Length - 1, actual.Count());
    // }
    //
    // [Fact]
    // public void BuildDataContractQueries_WhenBuiltWithInterval_ShouldHaveRowDeserializers()
    // {
    //     var sut = new QueryBuilder();
    //     var actual = sut.BuildDataContractQueries(AzureApiDataContractSource.All, 1, AgoPeriod.Days, null, null);
    //
    //     foreach (var azureApiDataContractSource in actual)
    //     {
    //         Assert.NotNull(azureApiDataContractSource.DataRowDeserializer);
    //     }
    // }
    //
    // [Fact]
    // public void BuildDataContractQueries_WhenBuiltWithTimespan_ShouldHaveRowDeserializers()
    // {
    //     var sut = new QueryBuilder();
    //     var actual = sut.BuildDataContractQueries(AzureApiDataContractSource.All, TimeSpan.FromDays(1), null, null);
    //
    //     foreach (var azureApiDataContractSource in actual)
    //     {
    //         Assert.NotNull(azureApiDataContractSource.DataRowDeserializer);
    //     }
    // }
    //
    // [Fact]
    // public void GetDataContractSources_WhenSpecifiedPassed_ShouldReturnOnlyPassedValues()
    // {
    //     var sut = new QueryBuilder();
    //     var actual = sut.GetDataContractSources(AzureApiDataContractSource.Availability | AzureApiDataContractSource.Dependency);
    //
    //     Assert.Equal(2, actual.Count());
    // }

}