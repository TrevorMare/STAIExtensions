using System;
using Xunit;

namespace STAIExtensions.Core.Tests;

public class AIQueryAPITests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ctor_WhenAppIdNullOrEmpty_ShouldThrowException(string appId)
    {
        Assert.Throws<ArgumentNullException>(() => new Core.AIQueryAPI(appId));
    }
}