using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections.Abstracts
{
    public abstract class NotifiableWrapperBase<TInput, TOutput> : NotifiableCollectionBase<TOutput>
    {
        private IEnumerable<TInput> _originalCollection;

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs arguments)
        {
            var newItems = arguments.NewItems as IEnumerable<TInput>;

            if (newItems == null && arguments.NewItems != null)
                newItems = arguments.NewItems.Cast<TInput>();

            var oldItems = arguments.OldItems as IEnumerable<TInput>;

            if (oldItems == null && arguments.OldItems != null)
                oldItems = arguments.OldItems.Cast<TInput>();

            switch (arguments.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    InternalAddItems(newItems);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    //TODO temporary solution
                    InternalRemoveItems(oldItems);
                    InternalAddItems(newItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    InternalRemoveItems(oldItems);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    InternalClearItems();
                    break;
            }
        }

        protected IEnumerable<TInput> AssociatedCollection => _originalCollection;

        protected void AssociateCollection(INotifyCollectionChanged notifyCollection)
        {
            ArgumentsGuard.ThrowIfNull(notifyCollection);
            ArgumentsGuard.ThrowIfNotType(notifyCollection, out IEnumerable<TInput> originalCollection);

            if (_originalCollection != null)
                DissociateCollection();

            _originalCollection = originalCollection;

            notifyCollection.CollectionChanged += OnCollectionChanged;

            InternalAddItems(_originalCollection);
        }

        protected void DissociateCollection()
        {
            ((INotifyCollectionChanged)_originalCollection).CollectionChanged -= OnCollectionChanged;

            InternalClearItems();
        }

        protected abstract void InternalAddItems(IEnumerable<TInput> items);

        protected abstract void InternalRemoveItems(IEnumerable<TInput> items);

        protected abstract void InternalClearItems();
    }
}
