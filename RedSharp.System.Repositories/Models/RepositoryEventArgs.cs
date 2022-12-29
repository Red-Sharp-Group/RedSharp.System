using System;
using System.Collections.Generic;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Repositories.Enums;

namespace RedSharp.Sys.Repositories.Models
{
    public class RepositoryEventArgs<TParameter>
    {
        private readonly NotifyRepositoryChangedAction _action;
        private readonly IEnumerable<TParameter> _newItems;
        private readonly IEnumerable<TParameter> _oldItems;

        public NotifyRepositoryChangedAction Action => _action;

        public IEnumerable<TParameter> NewItems => _newItems;

        public IEnumerable<TParameter> OldItems => _oldItems;

        public static RepositoryEventArgs<TItem> Add<TItem>(params TItem[] items)
        {
            return Add((IEnumerable<TItem>)items);
        }

        public static RepositoryEventArgs<TItem> Add<TItem>(IEnumerable<TItem> items)
        {
            ArgumentsGuard.ThrowIfNull(items);

            return new RepositoryEventArgs<TItem>(NotifyRepositoryChangedAction.Add, items, Array.Empty<TItem>());
        }

        public static RepositoryEventArgs<TItem> Update<TItem>(params TItem[] items)
        {
            return Update((IEnumerable<TItem>)items);
        }

        public static RepositoryEventArgs<TItem> Update<TItem>(IEnumerable<TItem> items)
        {
            ArgumentsGuard.ThrowIfNull(items);

            return new RepositoryEventArgs<TItem>(NotifyRepositoryChangedAction.Update, items, items);
        }

        public static RepositoryEventArgs<TItem> Remove<TItem>(params TItem[] items)
        {
            return Remove((IEnumerable<TItem>)items);
        }

        public static RepositoryEventArgs<TItem> Remove<TItem>(IEnumerable<TItem> items)
        {
            ArgumentsGuard.ThrowIfNull(items);

            return new RepositoryEventArgs<TItem>(NotifyRepositoryChangedAction.Remove, Array.Empty<TItem>(), items);
        }

        public static RepositoryEventArgs<TParameter> Reset()
        {
            return new RepositoryEventArgs<TParameter>(NotifyRepositoryChangedAction.Reset, Array.Empty<TParameter>(), Array.Empty<TParameter>());
        }

        private RepositoryEventArgs(NotifyRepositoryChangedAction action, IEnumerable<TParameter> newItems, IEnumerable<TParameter> oldItems)
        {
            _action = action;
            _newItems = newItems;
            _oldItems = oldItems;
        }
    }
}
