using System;
using System.Collections.Generic;
using System.Linq;
using STAIExtensions.Abstractions.Collections;
using STAIExtensions.Abstractions.Views;
using STAIExtensions.Core.Collections;
using Xunit;

namespace STAIExtensions.Core.Tests.Collections;

public class ViewCollectionsTest
{


    [Fact]
    public void ctor_WhenOptionsIsNull_ShouldNotFail()
    {
        var sut = new ViewCollection(null);
        Assert.NotNull(sut);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void ctor_WhenOptionsSet_ViewsExpireShouldBeSet(bool actual)
    {
        var options = new ViewCollectionOptions(1000, true, actual, TimeSpan.FromDays(1));
        var sut = new ViewCollection(options);

        Assert.Equal(actual, sut.ViewsExpire);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData(5)]
    public void ctor_WhenOptionsSet_MaximumViewsShouldBeSet(int? actual)
    {
        var options = new ViewCollectionOptions(actual, true, true, TimeSpan.FromDays(1));
        var sut = new ViewCollection(options);

        Assert.Equal(actual, sut.MaximumViews);
    }
    
    [Fact]
    public void ctor_WhenOptionsSet_SlidingExpirationTimeSpanShouldBeSet()
    {
        var actual = TimeSpan.FromHours(1);
        
        var options = new ViewCollectionOptions(1000, true, true, actual);
        var sut = new ViewCollection(options);

        Assert.Equal(actual, sut.DefaultSlidingExpiryTimeSpan);
    }
    
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateView_WhenViewTypeNameNotSet_ShouldThrowArgumentNullException(string viewType)
    {
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);

        Assert.Throws<ArgumentNullException>(() => sut.CreateView(viewType, null));
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateView_WhenOwnerIdNotSetAndUseStrictViewsSet_ShouldThrowArgumentNullException(string ownerId)
    {
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);

        Assert.Throws<ArgumentNullException>(() => sut.CreateView("TestView", ownerId));
    }
    
    [Fact]
    public void CreateView_WhenMaximumViewsReached_ShouldThrowException()
    {
        var options = new ViewCollectionOptions(0, true, true, null);
        var sut = new ViewCollection(options);
        Assert.Throws<Exception>(() => sut.CreateView("TestView", "123"));
    }
    
    [Fact]
    public void CreateView_WhenViewTypeNotFound_ShouldThrowException()
    {
        var options = new ViewCollectionOptions(0, true, true, null);
        var sut = new ViewCollection(options);
        Assert.Throws<Exception>(() => sut.CreateView("TestView", "123"));
    }
    
    [Fact]
    public void CreateView_WhenViewTypeFound_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var actual = sut.CreateView(t.AssemblyQualifiedName, "123");
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void CreateView_WhenShortNameViewTypeFound_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var actual = sut.CreateView(t.Name, "123");
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void CreateView_WhenViewTypeFound_ShouldViewIdShouldBeSet()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var actual = sut.CreateView(t.AssemblyQualifiedName, "123");
        Assert.False(string.IsNullOrEmpty(actual.Id));
    }
    
    [Fact]
    public void CreateView_WhenViewTypeWithSpacesPassed_ShouldTrimTypeName()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"  {t.AssemblyQualifiedName}  ";
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var actual = sut.CreateView(typeName, "123");
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void CreateView_WhenViewCreated_ShouldSetOwnerId()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var actual = sut.CreateView(typeName, "123");
        Assert.Equal("123", actual.OwnerId);
    }
    
    [Fact]
    public void CreateView_WhenViewCreatedAndSlidingExpirationPresent_ShouldSetViewSlidingExpiration()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        var slidingExpire = TimeSpan.FromDays(1);
        
        var options = new ViewCollectionOptions(1000, true, true, slidingExpire);
        var sut = new ViewCollection(options);
        var actual = sut.CreateView(typeName, "123");
        Assert.Equal(slidingExpire, actual.SlidingExpiration);
    }
    
    [Fact]
    public void CreateView_WhenViewCreatedAndSlidingExpirationPresent_NextExpireShouldBeSet()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        var slidingExpire = TimeSpan.FromDays(1);
        
        var options = new ViewCollectionOptions(1000, true, true, slidingExpire);
        var sut = new ViewCollection(options);
        var actual = sut.CreateView(typeName, "123");
        Assert.NotNull(actual.ExpiryDate);
    }
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetView_WhenOwnerIdNotSetAndUseStrictViewsIsFalse_ShouldNotFail(string ownerId)
    {
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);

        sut.GetView("123", ownerId);
        Assert.True(true);
    }
    
    
    [Fact]
    public void GetView_WhenViewCreatedWithDifferentOwnerAndUseStrictViews_ShouldNotReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, "123");

        var actual = sut.GetView(view.Id, "321");
        
        Assert.Null(actual);
    }
    
    [Fact]
    public void GetView_WhenViewCreatedWithDifferentOwnerAndNotUseStrictViews_ShouldNotReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, "123");

        var actual = sut.GetView(view.Id, "321");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetView_WhenOwnerIdPassedInStrictViewWithSpaces_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, "123");

        var actual = sut.GetView(view.Id, "  123  ");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetView_WhenViewIdPassedInStrictViewWithSpaces_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, "123");

        var actual = sut.GetView($"   {view.Id}   ", "123");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetView_WhenViewIdPassedInNonStrictViewWithSpaces_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        var actual = sut.GetView($"   {view.Id}   ", null);
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetView_WhenOwnerIdPassedInStrictViewWithCasing_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, "ABC");

        var actual = sut.GetView(view.Id, "abc");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetView_WhenViewIdPassedInStrictViewWithCasing_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, true, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, "123");

        var actual = sut.GetView($"{view.Id.ToUpper()}", "123");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetView_WhenViewIdPassedInNonStrictViewWithCasing_ShouldReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        var actual = sut.GetView($"{view.Id.ToUpper()}", null);
        
        Assert.NotNull(actual);
    }
    
    
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void GetViewForUpdate_WhenViewIdIsNull_ShouldThrowArgumentNullException(string viewId)
    {
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);

        Assert.Throws<ArgumentNullException>(() => sut.GetViewForUpdate(viewId)) ;
    }
    
    [Fact]
    public void GetViewForUpdate_WhenViewIdIsSetAndExists_ShouldIgnoreCaseAndReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        var actual = sut.GetViewForUpdate($"{view.Id.ToUpper()}");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetViewForUpdate_WhenViewIdIsSetAndExists_ShouldIgnoreSpacesAndReturnView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        var actual = sut.GetViewForUpdate($"   {view.Id}   ");
        
        Assert.NotNull(actual);
    }
    
    [Fact]
    public void GetExpiredViews_WhenExpiryDateIsSetNow_ShouldReturnViewToExpire()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.Zero);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        var actual = sut.GetExpiredViews();
        
        Assert.True(actual.Any());
    }
    
    [Fact]
    public void GetExpiredViews_WhenExpiryDateIsSetFuture_ShouldReturnViewToExpire()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(1));
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        var actual = sut.GetExpiredViews();
        
        Assert.False(actual.Any());
    }
    
    [Fact]
    public void RemoveView_WhenViewIsNull_ShouldThrowArgumentNullException()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(1));
        var sut = new ViewCollection(options);
        Assert.Throws<ArgumentNullException>(() => sut.RemoveView((IDataSetView?) null)); 
    }
    
    [Fact]
    public void RemoveView_WhenViewIdIsNull_ShouldThrowArgumentNullException()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(1));
        var sut = new ViewCollection(options);
        
        Assert.Throws<ArgumentNullException>(() => sut.RemoveView((string?) null)); 
    }
    
    [Fact]
    public void RemoveView_WhenViewIdIsNotNullAndViewExists_ShouldRemoveView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(1));
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        sut.RemoveView(view.Id);
        
        Assert.Equal(0, sut.ViewCount);
    }
    
    [Fact]
    public void RemoveView_WhenViewIdIsNotNullAndViewExistsAndCaseUpper_ShouldRemoveView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(1));
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        sut.RemoveView(view.Id.ToUpper());
        
        Assert.Equal(0, sut.ViewCount);
    }
    
    [Fact]
    public void RemoveView_WhenViewIdIsNotNullAndViewExistsAndContainsSpaces_ShouldRemoveView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(1));
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        sut.RemoveView($"  {view.Id}  ");
        
        Assert.Equal(0, sut.ViewCount);
    }
    
    [Fact]
    public void RemoveView_WhenViewIsNotNullAndViewExists_ShouldRemoveView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, TimeSpan.FromMinutes(1));
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        sut.RemoveView(view);
        
        Assert.Equal(0, sut.ViewCount);
    }
    
    [Fact]
    public void SetViewParameters_WhenViewIsNotNullAndViewExists_ShouldRemoveView()
    {
        var t = typeof(Fixtures.DataSetViewFixture);
        string typeName = $"{t.AssemblyQualifiedName}";
        
        var options = new ViewCollectionOptions(1000, false, true, null);
        var sut = new ViewCollection(options);
        var view = sut.CreateView(typeName, null);

        sut.SetViewParameters(view.Id,
            null,
            new Dictionary<string, object>()
            {
                {"ABC", 123}
            });

        var actual = (Fixtures.DataSetViewFixture)sut.GetView(view.Id, null);
        Assert.NotNull(actual?.FixtureParameters);
        Assert.True(actual?.FixtureParameters?.ContainsKey("ABC"));
    }
    
}