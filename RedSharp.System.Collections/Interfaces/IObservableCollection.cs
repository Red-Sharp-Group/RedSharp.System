using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Collections.Interfaces
{
    /// <summary>
    /// Implementation of <see cref="ICollection{T}"/> with reactivity
    /// </summary>
    /// <remarks>
    /// The property <see cref="IObservableCollection{T}.Count"/> can be observed.
    /// </remarks>
    public interface IObservableCollection<TItem> : ICollection<TItem>, IObservableEnumerable<TItem>, INotifyPropertyChanging, INotifyPropertyChanged, IReadableIndication
    { }
}
