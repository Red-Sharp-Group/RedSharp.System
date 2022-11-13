using System.ComponentModel;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IObservableRepositoryCollection<TItem> : IRepositoryCollection<TItem>, IObservableRepositoryEnumerable<TItem>, INotifyPropertyChanging, INotifyPropertyChanged
        where TItem : IReadableIndication
    {
        /// <summary>
        /// Basically, this is <see cref="IRepositoryCollection{TItem}.GetCount"/> without parameter in form of property.
        /// </summary>
        int Count { get; }
    }
}
