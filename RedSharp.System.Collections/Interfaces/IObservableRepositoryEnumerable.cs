using System.Collections.Specialized;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IObservableRepositoryEnumerable<TItem> : IRepositoryEnumerable<TItem>, INotifyCollectionChanged
    { }
}
