using System.Collections;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Core.Collections;

/// <summary>
/// A list collection that automatically removes older entries from the source if new items are pushed in
/// </summary>
/// <typeparam name="T">The type of list</typeparam>
public class SizedList<T> : Abstractions.Collections.ISizedList<T> 
{
    
    #region Events
    /// <summary>
    /// An event that will be raised if the collection changes
    /// </summary>
    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    #endregion
    
    #region Members

    private ListLocation _trimListLocation = ListLocation.Start;
    
    private ListLocation _addListLocation = ListLocation.End;

    private int? _maxSize = null;
    
    private readonly SynchronizedCollection<T> _fullDataSource = new SynchronizedCollection<T>();

    private readonly ILogger<SizedList<T>>? _logger;
    #endregion

    #region Properties
    /// <summary>
    /// Gets or sets a callback action to be executed when a collection has changed 
    /// </summary>
    public Action? CollectionChangedCallback { get; set; }
    
    /// <summary>
    /// The number of items in the collection
    /// </summary>
    public int Count => _fullDataSource.Count();
    
    /// <summary>
    /// Gets or sets the Maximum size of the collection
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public int? MaxSize
    {
        get => _maxSize;
        set
        {
            if (value.HasValue && value.Value < 0)
                throw new ArgumentException("Maximum size must be greater or equal to 0");

            if (value != _maxSize)
            {
                _maxSize = value;
                this.TrimList();
            }
        }
    }

    #endregion

    #region ctor
    public SizedList(IEnumerable<T>? items = default, int? maximumNumberOfItems = null, Action? collectionChangedCallback = default)
    {
        
        if (maximumNumberOfItems is < 0)
            throw new ArgumentOutOfRangeException(nameof(maximumNumberOfItems));

        this.CollectionChangedCallback = collectionChangedCallback;

        this.CollectionChanged += (sender, args) =>
        {
            this.TrimList(); 
            this.CollectionChangedCallback?.Invoke();
        };

        this._maxSize = maximumNumberOfItems;
        this._logger = Abstractions.DependencyExtensions.CreateLogger<SizedList<T>>();
        if (items != null)
        {
            this.AddRange(items);
        }
    }
    #endregion

    #region Trim Size Methods
    private bool TrimList()
    {
        if (this.MaxSize.HasValue == true && this._fullDataSource.Count() > this.MaxSize.Value)
        {
            // Calculate the number of items to reduce the size by
            var reductionSize = this._fullDataSource.Count() - this.MaxSize.Value;

            if (reductionSize > 0)
            {
                this._logger?.LogTrace("Reducing Sized List by {NumberOfItems} items", reductionSize);
                
                return TrimList(reductionSize);
            }
        }
        return false;
    }
    
    private bool TrimList(int numberOfItems)
    {
        return _trimListLocation == ListLocation.Start ? TrimListStart(numberOfItems) : TrimListEnd(numberOfItems);
    }
    
    private bool TrimListStart(int numberOfItems)
    {
        var itemsRemoved = false;
        for (var iTrim = 0; iTrim < numberOfItems; iTrim++)
        {
            if (this._fullDataSource.Any())
            {
                this._fullDataSource.RemoveAt(0);
                itemsRemoved = true;
            }
            else
                break;
        }
        
        if (itemsRemoved)
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        
        return itemsRemoved;
    }

    private bool TrimListEnd(int numberOfItems)
    {
        var itemsRemoved = false;
        for (var iTrim = 0; iTrim < numberOfItems; iTrim++)
        {
            if (this._fullDataSource.Any())
            {
                this._fullDataSource.RemoveAt(this.Count() - 1);
                itemsRemoved = true;
            }
            else
                break;
        }
        
        if (itemsRemoved)
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        
        return itemsRemoved;
    }
    #endregion

    #region Collection Modify Methods
    /// <summary>
    /// Adds a range of items to the collection
    /// </summary>
    /// <param name="items">The items to add</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void AddRange(IEnumerable<T> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        var enumerable = items as T[] ?? items.ToArray();
        
        if (!enumerable.Any())
            return;
        
        foreach (var item in enumerable)
        {
            AddItem(item, false, false);
        }
        
        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        this.TrimList();
    }
    
    /// <summary>
    /// Adds an item to the collection
    /// </summary>
    /// <param name="item">The item to add</param>
    public void Add(T item)
    {
        AddItem(item, true, true);
    }

    private void AddItem(T item, bool raiseEvents, bool trimList)
    {
        if (this._addListLocation == ListLocation.Start)
            this._fullDataSource.Insert(0, item);
        else 
            this._fullDataSource.Add(item);
        
        if (raiseEvents)
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        if (trimList)
            this.TrimList();
    }

    /// <summary>
    /// Clears the collection
    /// </summary>
    public void Clear()
    {
        this._fullDataSource.Clear();
        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    
    /// <summary>
    /// Removes an item from the collection
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(T item)
    {
        var result = this._fullDataSource.Remove(item);
        
        if (result == true)
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        
        return result;
    }
    
    /// <summary>
    /// Inserts an item into the collection
    /// </summary>
    /// <param name="index">The index to insert the item at</param>
    /// <param name="item">The item to insert</param>
    public void Insert(int index, T item)
    {
        this._fullDataSource.Insert(index, item);
        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    /// <summary>
    /// Removes an item at the specified index
    /// </summary>
    /// <param name="index">The 0 based index to remove the item at</param>
    public void RemoveAt(int index)
    {
        this._fullDataSource.RemoveAt(index);
        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    #endregion

    #region Generic List Methods
    /// <summary>
    /// Gets a value indicating if the collection is read only
    /// </summary>
    public bool IsReadOnly => false;
    
    /// <summary>
    /// Gets the collection enumerator
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        return _fullDataSource.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    /// <summary>
    /// Checks if the collection contains an item
    /// </summary>
    /// <param name="item">The item to search for</param>
    /// <returns></returns>
    public bool Contains(T item)
    {
        return _fullDataSource.Contains(item);
    }
    
    /// <summary>
    /// Copies the collection data to a new array
    /// </summary>
    /// <param name="array">The destination array</param>
    /// <param name="arrayIndex">The index to start copying from</param>
    public void CopyTo(T[] array, int arrayIndex)
    {
        _fullDataSource.CopyTo(array, arrayIndex);
    }
    
    /// <summary>
    /// Gets the index of an item
    /// </summary>
    /// <param name="item">The item to find</param>
    /// <returns></returns>
    public int IndexOf(T item)
    {
        return _fullDataSource.IndexOf(item);
    }
    
    /// <summary>
    /// Default Accessor for the collection
    /// </summary>
    /// <param name="index"></param>
    public T this[int index]
    {
        get => _fullDataSource[index];
        set => _fullDataSource[index] = value;
    }

    /// <summary>
    /// Checks if an item exists
    /// </summary>
    /// <param name="match">The find predicate</param>
    /// <returns></returns>
    public bool Exists(Predicate<T> match)
    {
        return _fullDataSource.ToList().Exists(match);
    }
    #endregion

    #region Dispose
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
    
}