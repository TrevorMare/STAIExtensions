using System;
using System.Linq;
using NSubstitute;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Data;
using STAIExtensions.Core.Collections;
using Xunit;

namespace STAIExtensions.Core.Tests.Collections;

public class DataSetCollectionTests
{

    [Fact]
    public void ctor_WhenNullOptionsPassed_ShouldNotFail()
    {
        var sut = new DataSetCollection(null);
        
        Assert.True(true);
    }
    
    [Fact]
    public void ctor_WhenOptionsPassed_ShouldSetMaximumDataSets()
    {
        var options = new DataSetCollectionOptions(5, null);
        var sut = new DataSetCollection(options);
        
        Assert.Equal(5, sut.MaximumDataSets);
    }

    [Fact]
    public void ctor_WhenOptionsPassed_ShouldSetMaximumViewsPerDataSet()
    {
        var options = new DataSetCollectionOptions(null, 5);
        var sut = new DataSetCollection(options);
        
        Assert.Equal(5, sut.MaximumViewsPerDataSet);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void RemoveViewFromDataSets_WhenViewIdIsNull_ShouldThrowArgumentNullException(string viewId)
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.RemoveViewFromDataSets(viewId));
    }
    
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void FindDataSetsByViewId_WhenViewIdIsNull_ShouldThrowArgumentNullException(string viewId)
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.FindDataSetsByViewId(viewId));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void FindDataSetByName_WhenDataSetNameIsNull_ShouldThrowArgumentNullException(string viewId)
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.FindDataSetsByViewId(viewId));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void FindDataSetById_WhenDataIdIsNull_ShouldThrowArgumentNullException(string datasetId)
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.FindDataSetById(datasetId));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void DetachViewFromDataSet_WhenDataSetIdIsNull_ShouldThrowArgumentNullException(string datasetId)
    {
        var sut = new DataSetCollection(null);
        var view = new Fixtures.DataSetViewFixture();

        Assert.Throws<ArgumentNullException>(() => sut.DetachViewFromDataSet(datasetId, view));
    }
    
    [Fact]
    public void DetachViewFromDataSet_WhenViewIsNull_ShouldThrowArgumentNullException()
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.DetachViewFromDataSet("123", null));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("  ")]
    public void AttachViewToDataSet_WhenDataSetIdIsNull_ShouldThrowArgumentNullException(string datasetId)
    {
        var sut = new DataSetCollection(null);
        var view = new Fixtures.DataSetViewFixture();

        Assert.Throws<ArgumentNullException>(() => sut.AttachViewToDataSet(datasetId, view));
    }
    
    [Fact]
    public void AttachViewToDataSet_WhenViewIsNull_ShouldThrowArgumentNullException()
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.AttachViewToDataSet("123", null));
    }
    
    [Fact]
    public void DetachDataSet_WhenDataSetIsNull_ShouldThrowArgumentNullException()
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.DetachDataSet(null));
    }
    
    
    [Fact]
    public void AttachDataSet_WhenDataSetIsNull_ShouldThrowArgumentNullException()
    {
        var sut = new DataSetCollection(null);

        Assert.Throws<ArgumentNullException>(() => sut.AttachDataSet(null));
    }
    
    [Fact]
    public void AttachDataSet_WhenDataSetNotAttached_ShouldAttachDataSet()
    {
        var sut = new DataSetCollection(null);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");

        var actual = sut.AttachDataSet(ds);
        Assert.True(actual);
    }
    
    [Fact]
    public void AttachDataSet_WhenDataSetAlreadyAttached_ShouldNotAttachDataSet()
    {
        var sut = new DataSetCollection(null);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        sut.AttachDataSet(ds);
        
        var actual = sut.AttachDataSet(ds);
        
        Assert.False(actual);
        Assert.Equal(1, sut.NumberOfDataSets);
    }
    
    [Fact]
    public void AttachDataSet_WhenMaximumDataSetsAttached_ShouldThrowException()
    {
        var options = new DataSetCollectionOptions(0, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        Assert.Throws<Exception>(() => sut.AttachDataSet(ds)) ;
    }
    
    [Fact]
    public void DetachDataSet_WhenDataSetExists_ShouldRemoveDataSet()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        sut.AttachDataSet(ds);
        var actual = sut.DetachDataSet(ds);
        
        Assert.True(actual);
        Assert.Equal(0, sut.NumberOfDataSets);
    }
    
    [Fact]
    public void DetachDataSet_WhenDataSetDoesNotExists_ShouldNotFail()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        
        var actual = sut.DetachDataSet(ds);
        
        Assert.False(actual);
        Assert.Equal(0, sut.NumberOfDataSets);
    }
    
    [Fact]
    public void ListDataSets_WhenCalled_ShouldReturnAttachedDataSets()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        sut.AttachDataSet(ds);
        var actual = sut.ListDataSets();
        
        Assert.True(actual.Any());
    }
    
    [Fact]
    public void FindDataSetById_WhenCasingIsUpper_ShouldReturnDataSet()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        sut.AttachDataSet(ds);
        var actual = sut.FindDataSetById(ds.DataSetId.ToUpper());
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void FindDataSetById_WhenSpacesPresent_ShouldReturnDataSet()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        sut.AttachDataSet(ds);
        var actual = sut.FindDataSetById($"  {ds.DataSetId}  ");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void FindDataSetByName_WhenCasingIsUpper_ShouldReturnDataSet()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        sut.AttachDataSet(ds);
        var actual = sut.FindDataSetByName(ds.DataSetName.ToUpper());
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void FindDataSetByName_WhenSpacesPresent_ShouldReturnDataSet()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        
        sut.AttachDataSet(ds);
        var actual = sut.FindDataSetByName($"  {ds.DataSetName}  ");
        
        Assert.NotNull(actual);
    }
    
    
    [Fact]
    public void AttachViewToDataSet_WhenDataSetNotFound_ShouldReturnFalse()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var view = new Fixtures.DataSetViewFixture();
        
        var actual = sut.AttachViewToDataSet(ds.DataSetId, view);
        
        Assert.False(actual);
    }
    
    [Fact]
    public void AttachViewToDataSet_WhenDataSetFoundAndViewAlreadyAttached_ShouldReturnFalse()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var view = new Fixtures.DataSetViewFixture();

        sut.AttachDataSet(ds);
        sut.AttachViewToDataSet(ds.DataSetId, view);
        
        var actual = sut.AttachViewToDataSet(ds.DataSetId, view);
        
        Assert.False(actual);
    }
    
    [Fact]
    public void AttachViewToDataSet_WhenDataSetFoundAndViewNotAttached_ShouldReturnTrue()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var view = new Fixtures.DataSetViewFixture();

        sut.AttachDataSet(ds);
       
        var actual = sut.AttachViewToDataSet(ds.DataSetId, view);
        
        Assert.True(actual);
    }
    
    [Fact]
    public void AttachViewToDataSet_WhenMaximumViewsPerDataSetSpecified_ShouldThrowException()
    {
        var options = new DataSetCollectionOptions(null, 0);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var view = new Fixtures.DataSetViewFixture();

        sut.AttachDataSet(ds);
       
        Assert.Throws<Exception>(() => sut.AttachViewToDataSet(ds.DataSetId, view)) ;
    }
    
    [Fact]
    public void DetachViewFromDataSet_WhenDataSetNotFound_ShouldReturnFalse()
    {
        var options = new DataSetCollectionOptions(null, 0);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var view = new Fixtures.DataSetViewFixture();

        var actual = sut.DetachViewFromDataSet(ds.DataSetId, view);
        Assert.False(actual);
    }
    
    [Fact]
    public void DetachViewFromDataSet_WhenDataSetViewNotFound_ShouldReturnFalse()
    {
        var options = new DataSetCollectionOptions(null, 0);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var view = new Fixtures.DataSetViewFixture();

        sut.AttachDataSet(ds);
        
        var actual = sut.DetachViewFromDataSet(ds.DataSetId, view);
        Assert.False(actual);
    }
    
    [Fact]
    public void DetachViewFromDataSet_WhenDataSetAndDataSetViewFound_ShouldReturnTrue()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        var ds = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var view = new Fixtures.DataSetViewFixture();

        sut.AttachDataSet(ds);
        sut.AttachViewToDataSet(ds.DataSetId, view);
        
        var actual = sut.DetachViewFromDataSet(ds.DataSetId, view);
        Assert.True(actual);
    }
    
    [Fact]
    public void FindDataSetsByViewId_WhenViewAttachedToDataSets_ShouldReturnDataSets()
    {
        var options = new DataSetCollectionOptions(null, null);
        var sut = new DataSetCollection(options);
        var telemetryLoader = Substitute.For<ITelemetryLoader>();
        
        var ds1 = new Fixtures.DataSetFixture(telemetryLoader, "DataSet1");
        var ds2 = new Fixtures.DataSetFixture(telemetryLoader, "DataSet2");
        
        var view = new Fixtures.DataSetViewFixture();

        sut.AttachDataSet(ds1);
        sut.AttachDataSet(ds2);
        
        sut.AttachViewToDataSet(ds1.DataSetId, view);
        sut.AttachViewToDataSet(ds2.DataSetId, view);
        
        var actual = sut.FindDataSetsByViewId($"  {view.Id.ToUpper()} ");
        Assert.True(actual?.Count() == 2);
    }
    
}