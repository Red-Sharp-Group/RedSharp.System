using System.Collections.Generic;
using System.Collections.Specialized;

namespace RedSharp.Sys.Collections.Interfaces
{
    /// <summary>
    /// Basic class for all "notifiable" collections, 
    /// that fix mistake from the system library, 
    /// where these two interfaces are existed separately.
    /// </summary>
    public interface IObservableEnumerable<TItem> : INotifyCollectionChanged, IEnumerable<TItem>
    { }
}
