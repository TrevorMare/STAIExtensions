using System;
using System.Threading.Tasks;
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
        Assert.Throws<ArgumentNullException>(() => new Core.AIQueryApi(appId));
    }

    [Fact]
    public void ExecuteQuery_WhenErrorThrown_ShouldReturnResponseObject()
    {
        var sut = new Core.AIQueryApi("123");
        var actual = sut.ExecuteQuery(null);
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void ExecuteQuery_WhenErrorThrown_ResultShouldContainErrorMessage()
    {
        var sut = new Core.AIQueryApi("123");
        var actual = sut.ExecuteQuery(null);
        Assert.False(string.IsNullOrEmpty(actual?.ErrorMessage));
    }
    
    [Fact]
    public void ExecuteQuery_WhenErrorThrown_ResultSuccessShouldBeFalse()
    {
        var sut = new Core.AIQueryApi("123");
        var actual = sut.ExecuteQuery(null);
        Assert.False(actual?.Success);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ExecuteQuery_WhenQueryIsEmptyOrNull_ResultShouldBeFalse(string query)
    {
        var sut = new Core.AIQueryApi("123");
        var actual = sut.ExecuteQuery(query);
        Assert.False(actual.Success);
    }
    
    [Fact]
    public async Task ExecuteQueryAsync_WhenErrorThrown_ShouldReturnResponseObject()
    {
        var sut = new Core.AIQueryApi("123");
        var actual = await sut.ExecuteQueryAsync(null);
        Assert.NotNull(actual);
    }
    
    [Fact]
    public async Task ExecuteQueryAsync_WhenErrorThrown_ResultShouldContainErrorMessage()
    {
        var sut = new Core.AIQueryApi("123");
        var actual = await sut.ExecuteQueryAsync(null);
        Assert.False(string.IsNullOrEmpty(actual?.ErrorMessage));
    }
    
    [Fact]
    public async Task ExecuteQueryAsync_WhenErrorThrown_ResultSuccessShouldBeFalse()
    {
        var sut = new Core.AIQueryApi("123");
        var actual = await sut.ExecuteQueryAsync(null);
        Assert.False(actual?.Success);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task ExecuteQueryAsync_WhenQueryIsEmptyOrNull_ResultShouldBeFalse(string query)
    {
        var sut = new Core.AIQueryApi("123");
        var actual = await sut.ExecuteQueryAsync(query);
        Assert.False(actual.Success);
    }
    
}