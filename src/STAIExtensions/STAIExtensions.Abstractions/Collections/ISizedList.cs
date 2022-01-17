using System.Collections;
using System.Collections.Specialized;

namespace STAIExtensions.Abstractions.Collections;

/// <summary>
/// Interface for the fixed size collection container. 
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISizedList<T> : IList<T>, INotifyCollectionChanged, IDisposable 
{
    
    /// <summary>
    /// Adds a range of items to the collection
    /// </summary>
    /// <param name="items">The items to add</param>
    void AddRange(IEnumerable<T> items);

    /// <summary>
    /// Checks if an item exists in the collection
    /// </summary>
    /// <param name="match">Match predicate</param>
    /// <returns></returns>
    bool Exists(Predicate<T> match);
    
    /// <summary>
    /// Gets or sets an action that is executed when the collection is updated
    /// </summary>
    Action? CollectionChangedCallback { get; set; }
    
}