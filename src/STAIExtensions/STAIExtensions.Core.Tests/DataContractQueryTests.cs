using System;
using Xunit;

namespace STAIExtensions.Core.Tests;

public class DataContractQueryTests
{

    [Fact]
    public void ctor_WhenAIQueryApiNull_MustThrowException()
    {
        Assert.Throws<ArgumentNullException>(() => new Core.DataContractQuery(null));
    }
    
    
}