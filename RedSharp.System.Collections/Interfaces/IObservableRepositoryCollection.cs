using System.ComponentModel;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IObservableRepositoryCollection<TItem> : IRepositoryCollection<TItem>, IObservableRepositoryEnumerable<TItem>, INotifyPropertyChanging, INotifyPropertyChanged
    {
        /// <summary>
        /// Basically, this is <see cref="IRepositoryCollection{TItem}.GetCount"/> without parameter in form of property.
        /// </summary>
        int Count { get; }
    }
}
