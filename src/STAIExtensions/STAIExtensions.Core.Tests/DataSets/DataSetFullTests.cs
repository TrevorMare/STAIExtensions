using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace STAIExtensions.Core.Tests.DataSets;

public class DataSetFullTests
{
    private IServiceProvider ServiceProvider => BuildServiceProvider();
    
    private IServiceProvider BuildServiceProvider()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.UseSTAIExtensions();
        return serviceCollection.BuildServiceProvider();
    }
    
    [Fact(Skip = "Only local tests")]
    public void RefreshDataSet_WhenCalled_ShouldPopulateData()
    {

        var sut = new Core.DataSets.DataSetFull(ServiceProvider);

        sut.SetupApiConfig("xxx", "yyy");
        sut.RefreshDataSet();

    }
}