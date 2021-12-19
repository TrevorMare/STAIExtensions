using System;
using System.Collections.Generic;
using System.Linq;
using STAIExtensions.Data.AzureDataExplorer.Models;
using STAIExtensions.Data.AzureDataExplorer.Serialization;
using STAIExtensions.Data.AzureDataExplorer.Tests.AIDataGenerator;
using STAIExtensions.Data.AzureDataExplorer.Tests.Fixtures;
using Xunit;

namespace STAIExtensions.Data.AzureDataExplorer.Tests.Serialization;

public class TableRowDeserializerTests
{
    [Fact]
    public void DeserializeTableRows_WhenTableNull_ShouldThrowArgumentNullException()
    {
        var sut = new TableRowDeserializer();
        
        Assert.Throws<ArgumentNullException>(() => sut.DeserializeTableRows<CustomDataContract>(null));
    }
    
    [Fact]
    public void DeserializeTableRows_WhenTableRowsNull_ShouldReturnNull()
    {
        var sut = new TableRowDeserializer();
        
        var actual = sut.DeserializeTableRows<CustomDataContract>(new ApiClientQueryResultTable());
        
        Assert.Null(actual);
    }

    [Theory]
    [InlineData("TestColumnName")]
    [InlineData(" TestColumnName ")]
    [InlineData("testcolumnNAME")]
    public void GetPropertyInfoByColumnName_WhenColumnNamePresent_ShouldReturnProperty(string columnName)
    {
        var sut = new TableRowDeserializer();
        var properties = typeof(Fixtures.TestSerializationClass).GetProperties().ToList();

        var actual = sut.GetPropertyInfoByColumnName(properties, columnName);
        Assert.NotNull(actual);
    }
    
    [Theory]
    [InlineData("AttrColumn")]
    [InlineData(" AttrColumn ")]
    [InlineData("AttrCOLUMN")]
    public void GetPropertyInfoByColumnName_WhenColumnAttributePresent_ShouldReturnProperty(string columnName)
    {
        var sut = new TableRowDeserializer();
        var properties = typeof(Fixtures.TestSerializationClass).GetProperties().ToList();

        var actual = sut.GetPropertyInfoByColumnName(properties, columnName);
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetPropertyInfoByColumnName_WhenColumnNamePresentAndCannotWrite_ShouldReturnNull()
    {
        var sut = new TableRowDeserializer();
        var properties = typeof(Fixtures.TestSerializationClass).GetProperties().ToList();

        var actual = sut.GetPropertyInfoByColumnName(properties, "GetOnlyColumnName");
        Assert.Null(actual);
    }

    [Fact]
    public void BuildColumnPropertyMapping_WhenTableIsNull_ShouldReturnNull()
    {
        var sut = new TableRowDeserializer();

        var actual = sut.BuildColumnPropertyMapping<object>(null);
        Assert.Null(actual);
    }
    
    [Fact]
    public void BuildColumnPropertyMapping_WhenTableColumnsIsNull_ShouldReturnNull()
    {
        var sut = new TableRowDeserializer();
        var actual = sut.BuildColumnPropertyMapping<object>(new ApiClientQueryResultTable());
        Assert.Null(actual);
    }

    [Fact]
    public void BuildColumnPropertyMapping_WhenTableSupplied_ShouldMapProperties()
    {
        var sut = new TableRowDeserializer();

        var parameters = new ApiClientQueryResultTable()
        {
            Columns = new List<ApiClientQueryResultColumn>()
            {
                new ApiClientQueryResultColumn() { ColumnName = "TestColumnName" },
                new ApiClientQueryResultColumn() { ColumnName = "AttrColumn" },
                new ApiClientQueryResultColumn() { ColumnName = "GetOnlyColumnName" }
            }
        };
        
        var actual = sut.BuildColumnPropertyMapping<Fixtures.TestSerializationClass>(parameters);
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void BuildColumnPropertyMapping_WhenReadOnlyProperty_ShouldNotMapProperty()
    {
        var sut = new TableRowDeserializer();

        var parameters = new ApiClientQueryResultTable()
        {
            Columns = new List<ApiClientQueryResultColumn>()
            {
                new ApiClientQueryResultColumn() { ColumnName = "TestColumnName" },
                new ApiClientQueryResultColumn() { ColumnName = "AttrColumn" },
                new ApiClientQueryResultColumn() { ColumnName = "GetOnlyColumnName" }
            }
        };
        
        var actual = sut.BuildColumnPropertyMapping<Fixtures.TestSerializationClass>(parameters);
        
        
        Assert.Null(actual.FirstOrDefault(x => x.Column.ColumnName == "GetOnlyColumnName").Property);
    }
    
    [Fact]
    public void BuildColumnPropertyMapping_WhenCanWriteProperty_ShouldMapProperty()
    {
        var sut = new TableRowDeserializer();

        var parameters = new ApiClientQueryResultTable()
        {
            Columns = new List<ApiClientQueryResultColumn>()
            {
                new ApiClientQueryResultColumn() { ColumnName = "TestColumnName" },
                new ApiClientQueryResultColumn() { ColumnName = "AttrColumn" },
                new ApiClientQueryResultColumn() { ColumnName = "GetOnlyColumnName" }
            }
        };
        
        var actual = sut.BuildColumnPropertyMapping<Fixtures.TestSerializationClass>(parameters);
        
        
        Assert.NotNull(actual.FirstOrDefault(x => x.Column.ColumnName == "AttrColumn").Property);
        Assert.NotNull(actual.FirstOrDefault(x => x.Column.ColumnName == "TestColumnName").Property);
    }
    
    [Fact]
    public void BuildColumnPropertyMapping_WhenSerializationAttributeSpecified_ShouldHaveSerializerInstance()
    {
        var sut = new TableRowDeserializer();

        var parameters = new ApiClientQueryResultTable()
        {
            Columns = new List<ApiClientQueryResultColumn>()
            {
                new ApiClientQueryResultColumn() { ColumnName = "TestColumnName" },
                new ApiClientQueryResultColumn() { ColumnName = "AttrColumn" },
                new ApiClientQueryResultColumn() { ColumnName = "GetOnlyColumnName" }
            }
        };
        
        var actual = sut.BuildColumnPropertyMapping<Fixtures.TestSerializationClass>(parameters);
        
        
        Assert.NotNull(actual.FirstOrDefault(x => x.Column.ColumnName == "AttrColumn").FieldDeserializer);
    }

    [Fact(Skip = "Only for generation")]
    public void GenerateData()
    {
        var dataGenerator = new ApplicationInsightsDataGenerator();
        dataGenerator.GenerateTelemetry();

    }
    
    
}