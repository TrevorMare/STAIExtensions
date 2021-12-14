using System;
using System.Linq;
using STAIExtensions.Abstractions.Common;
using STAIExtensions.Core.Queries;
using Xunit;

namespace STAIExtensions.Core.Tests.Queries;

public class QueryBuilderTests
{

    [Fact]
    public void GetDataContractSources_WhenAllPassed_ShouldNotReturnAll()
    {
        var sut = new QueryBuilder();
        var actual = sut.GetDataContractSources(AzureApiDataContractSource.All);

        Assert.False(actual.Contains(AzureApiDataContractSource.All));
    }

    [Fact]
    public void GetDataContractSources_WhenAllPassed_ShouldNotReturnOtherValues()
    {
        var sut = new QueryBuilder();
        var actual = sut.GetDataContractSources(AzureApiDataContractSource.All);

        Assert.Equal(Enum.GetValues(typeof(AzureApiDataContractSource)).Length - 1, actual.Count());
    }
    
    [Fact]
    public void GetDataContractSources_WhenSpecifiedPassed_ShouldReturnOnlyPassedValues()
    {
        var sut = new QueryBuilder();
        var actual = sut.GetDataContractSources(AzureApiDataContractSource.Availability | AzureApiDataContractSource.Dependency);

        Assert.Equal(2, actual.Count());
    }

}