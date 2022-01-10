using System.Collections;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;

namespace STAIExtensions.Core.Collections;

public class SizedList<T> : Abstractions.Collections.ISizedList<T> 
{
    
    #region Events
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
    public Action? CollectionChangedCallback { get; set; }
    
    public int Count => _fullDataSource.Count();
    
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

    public void Clear()
    {
        this._fullDataSource.Clear();
        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    
    public bool Remove(T item)
    {
        var result = this._fullDataSource.Remove(item);
        
        if (result == true)
            this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        
        return result;
    }
    
    public void Insert(int index, T item)
    {
        this._fullDataSource.Insert(index, item);
        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public void RemoveAt(int index)
    {
        this._fullDataSource.RemoveAt(index);
        this.CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    #endregion

    #region Generic List Methods
    public bool IsReadOnly => false;
    public IEnumerator<T> GetEnumerator()
    {
        return _fullDataSource.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    public bool Contains(T item)
    {
        return _fullDataSource.Contains(item);
    }
    
    public void CopyTo(T[] array, int arrayIndex)
    {
        _fullDataSource.CopyTo(array, arrayIndex);
    }
    
    public int IndexOf(T item)
    {
        return _fullDataSource.IndexOf(item);
    }
    
    public T this[int index]
    {
        get => _fullDataSource[index];
        set => _fullDataSource[index] = value;
    }

    public bool Exists(Predicate<T> match)
    {
        return _fullDataSource.ToList().Exists(match);
    }

    public bool ExistsDeep(Predicate<T> match)
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