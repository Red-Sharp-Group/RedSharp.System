using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Collections.Interfaces;

namespace RedSharp.Sys.Collections.Abstracts
{
    public abstract class NotifiableCollectionBase<TItem> : INotifyCollectionChanged, INotifyPropertyChanging, INotifyPropertyChanged
    {
        private const string IndexerProperty = "Item[]";
        private const string IndexerPropertyFormat = "Item[{0}]";

        /// <inheritdoc/>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// Occurs on the beginning of property changing right before it is actually changed.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        /// <inheritdoc/>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes a <see cref="CollectionChanged"/>.
        /// </summary>
        /// <remarks>
        /// Raises in the try {..} catch {..} statements.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs arguments)
        {
            try
            {
                CollectionChanged?.Invoke(this, arguments);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Has to be invoked on single item adding, possible with index (if it is an inserting).
        /// </summary>
        protected void RaiseAdding(TItem item, int? index = null)
        {
            if (index.HasValue)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index.Value));
            else
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        /// <summary>
        /// Using for range adding.
        /// </summary>
        /// <remarks>
        /// Currently I don't know a collection that can add a range by specific index, so I do not use it for this case.
        /// </remarks>
        protected void RaiseAdding(IList<TItem> items, int? index = null)
        {
            if (index.HasValue)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList)items, index.Value));
            else
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, (IList)items));
        }

        /// <summary>
        /// Has to be invoked or set by indexer, better with index.
        /// </summary>
        protected void RaiseReplacing(TItem oldItem, TItem newItem, int? index = null)
        {
            if (index.HasValue)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem, index.Value));
            else
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
        }

        /// <summary>
        /// Has to be invoked on single item removing, possible with index (if it is a removing by index).
        /// </summary>
        protected void RaiseRemoving(TItem item, int? index = null)
        {
            if (index.HasValue)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index.Value));
            else
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
        }

        /// <summary>
        /// Using for range removing.
        /// </summary>
        /// <remarks>
        /// Currently I don't know a collection that can remove a range by specific index, so I do not use it for this case.
        /// </remarks>
        protected void RaiseRemoving(IList<TItem> items, int? index = null)
        {
            if (index.HasValue)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (IList)items, index.Value));
            else
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (IList)items));
        }

        /// <summary>
        /// Used for as Microsoft documentation says "dramatic changing" of the collection as clearing f.e.
        /// </summary>
        protected void RaiseClearing()
        {
            RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        /// <summary>
        /// Invokes a <see cref="PropertyChanging"/> for the input property name.
        /// </summary>
        /// <remarks>
        /// Raises in the try {..} catch {..} statements.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RaisePropertyChanging([CallerMemberName] String name = null)
        {
            if (PropertyChanging == null)
                return;

            var eventArguments = new PropertyChangingEventArgs(name);

            try
            {
                PropertyChanging.Invoke(this, eventArguments);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Invokes a <see cref="PropertyChanging"/> for the indexer.
        /// </summary>
        protected void RaiseKeyChanging(object key = null)
        {
            if (key != null && key is string)
                RaisePropertyChanging(String.Format(IndexerPropertyFormat, key));

            RaisePropertyChanging(IndexerProperty);
        }

        /// <summary>
        /// Invokes a <see cref="PropertyChanged"/> for the input property name.
        /// </summary>
        /// <remarks>
        /// Raises in the try {..} catch {..} statements.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RaisePropertyChanged([CallerMemberName] String name = null)
        {
            if (PropertyChanged == null)
                return;

            var eventArguments = new PropertyChangedEventArgs(name);

            try
            {
                PropertyChanged.Invoke(this, eventArguments);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }

        /// <summary>
        /// Invokes a <see cref="PropertyChanged"/> for the indexer.
        /// </summary>
        protected void RaiseKeyChanged(object key = null)
        {
            if (key != null && key is string)
                RaisePropertyChanged(String.Format(IndexerPropertyFormat, key));

            RaisePropertyChanged(IndexerProperty);
        }
    }
}
