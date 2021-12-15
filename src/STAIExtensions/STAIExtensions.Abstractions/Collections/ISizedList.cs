using System.Collections;
using System.Collections.Specialized;

namespace STAIExtensions.Abstractions.Collections;

public interface ISizedList<T> : IList<T>, INotifyCollectionChanged 
{
    void AddRange(IEnumerable<T> items);

    bool Exists(Predicate<T> match);
    
    bool ExistsDeep(Predicate<T> match);
    
    Action? CollectionChangedCallback { get; set; }
    
}