using System.Collections.Generic;

namespace RedSharp.Sys.Collections.Interfaces
{
    /// <summary>
    /// Implementation of <see cref="IList{T}"/> with reactivity
    /// </summary>
    /// <remarks>
    /// The property <see cref="IObservableList{T}.Count"/> can be observed.
    /// </remarks>
    public interface IObservableList<TItem> : IList<TItem>, IObservableCollection<TItem>
    { }
}
