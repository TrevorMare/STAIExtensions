using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace STAIExtensions.Core.Tests.Collections;

public class SizedListTests
{

    [Fact]
    public void ctor_WhenMaximumNumberOfItemsLessThan0_ShouldThrowException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Core.Collections.SizedList<object>(maximumNumberOfItems: -1));
    }
    
    [Fact]
    public void ctor_WhenMaximumNumberOfItemsEqualTo0_ShouldAllow()
    {
        var sut = new Core.Collections.SizedList<object>(maximumNumberOfItems: 0);
        Assert.NotNull(sut);
    }
    
    [Fact]
    public void ctor_WhenMaximumNumberOfItemsSet_OnlyMaxItemsShouldBeAdded()
    {
        var localArray = new List<int>() {1, 2, 3};
        var sut = new Core.Collections.SizedList<int>(localArray,  0);
        Assert.False(sut.Any());
    }
    
    [Fact]
    public void ctor_WhenMaximumNumberOfItemsSet_ItemsShouldBeAdded()
    {
        var localArray = new List<int>() {1, 2, 3};
        var sut = new Core.Collections.SizedList<int>(localArray,  2);
        Assert.True(2 == sut.Count);
    }
    
    [Fact]
    public void ctor_WhenNoMaximumNumberOfItemsSet_ItemsShouldBeAdded()
    {
        var localArray = new List<int>() {1, 2, 3};
        var sut = new Core.Collections.SizedList<int>(localArray,  null);
        Assert.True(3 == sut.Count);
    }
    
    [Fact]
    public void Add_WhenAddingToList_CallbackShouldBeRaisedOnce()
    {
        int numberOfCalls = 0;
        Action callback = () =>
        {
            numberOfCalls++;
        };

        var sut = new Core.Collections.SizedList<int>(collectionChangedCallback: callback);
        sut.Add(1);
        Assert.Equal(1, numberOfCalls);
    }
    
    [Fact]
    public void AddRange_WhenAddRangeToList_CallbackShouldBeRaisedOnce()
    {
        int numberOfCalls = 0;
        Action callback = () =>
        {
            numberOfCalls++;
        };

        var sut = new Core.Collections.SizedList<int>(collectionChangedCallback: callback);
        sut.AddRange(new int[] { 1, 2, 3, 4 });
        Assert.Equal(1, numberOfCalls);
    }
    
    [Fact]
    public void Clear_WhenListCleared_CallbackShouldBeRaisedOnce()
    {
        int numberOfCalls = 0;
        Action callback = () =>
        {
            numberOfCalls++;
        };

        var sut = new Core.Collections.SizedList<int>();
        sut.AddRange(new int[] { 1, 2, 3, 4 });

        sut.CollectionChangedCallback = callback;
        sut.Clear();
        
        Assert.True(1 == numberOfCalls);
    }
    
    [Fact]
    public void Clear_WhenListCleared_ShouldClearList()
    {
        var sut = new Core.Collections.SizedList<int>();
        sut.AddRange(new int[] { 1, 2, 3, 4 });

        sut.Clear();
        
        Assert.False(sut.Any());
    }
    
    [Fact]
    public void Remove_WhenCalled_ShouldRemoveItem()
    {
        var items = new List<int>() {1, 2, 3, 4};
        var sut = new Core.Collections.SizedList<int>(items);

        sut.Remove(1);
        
        Assert.False(sut.Exists(x => x == 1));
    }
    
    [Fact]
    public void Insert_WhenCalled_ShouldInsertItem()
    {
        var items = new List<int>() { 2, 3, 4};
        var sut = new Core.Collections.SizedList<int>(items);

        sut.Insert(0, 1);
        
        Assert.True(sut.Exists(x => x == 1));
    }
    
    [Fact]
    public void RemoveAt_WhenCalled_ShouldRemoveItem()
    {
        var items = new List<int>() {1, 2, 3, 4};
        var sut = new Core.Collections.SizedList<int>(items);

        sut.RemoveAt(0);
        
        Assert.False(sut.Exists(x => x == 1));
        
    }

    [Fact]
    public void MaxSize_OnChange_ListShouldAdapt()
    {
        var items = new List<int>() {1, 2, 3, 4};
        var sut = new Core.Collections.SizedList<int>(items);

        sut.MaxSize = 2;
        Assert.True(2 == sut.Count);
    }

    [Fact]
    public void Add_WhenAddingNewerItems_ShouldRemoveOldestEntries()
    {
        var items = new List<int>() {1, 2, 3, 4};
        var sut = new Core.Collections.SizedList<int>(items, 4);

        sut.Add(5);
        sut.Add(6);
        
        Assert.False(sut.Exists(x => x == 1));
        Assert.False(sut.Exists(x => x == 2));
    }
    
    [Fact]
    public void Add_WhenTrimming_ShouldRemoveOldestEntries()
    {
        var items = new List<int>() {1, 2, 3, 4, 5, 6};
        var sut = new Core.Collections.SizedList<int>(items, 6);

        sut.MaxSize = 4;
        
        Assert.False(sut.Exists(x => x == 1));
        Assert.False(sut.Exists(x => x == 2));
    }
    
}