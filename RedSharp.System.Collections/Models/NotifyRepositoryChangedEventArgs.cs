using System;
using System.Collections;
using System.Collections.Generic;
using RedSharp.Sys.Collections.Enums;
using RedSharp.Sys.Helpers;

namespace RedSharp.Sys.Collections.Models
{
    public class NotifyRepositoryChangedEventArgs
    {
        private readonly NotifyRepositoryChangedAction _action;
        private readonly IEnumerable _newItems;
        private readonly IEnumerable _oldItems;

        public NotifyRepositoryChangedAction Action => _action;

        public IEnumerable NewItems => _newItems;

        public IEnumerable OldItems => _oldItems;

        public static NotifyRepositoryChangedEventArgs Add<TItem>(params TItem[] items)
        {
            return Add((IEnumerable<TItem>)items);
        }

        public static NotifyRepositoryChangedEventArgs Add<TItem>(IEnumerable<TItem> items)
        {
            ArgumentsGuard.ThrowIfNull(items);

            return new NotifyRepositoryChangedEventArgs(NotifyRepositoryChangedAction.Add, items, Array.Empty<TItem>());
        }

        public static NotifyRepositoryChangedEventArgs Update<TItem>(params TItem[] items)
        {
            return Update((IEnumerable<TItem>)items);
        }

        public static NotifyRepositoryChangedEventArgs Update<TItem>(IEnumerable<TItem> items)
        {
            ArgumentsGuard.ThrowIfNull(items);

            return new NotifyRepositoryChangedEventArgs(NotifyRepositoryChangedAction.Update, items, items);
        }

        public static NotifyRepositoryChangedEventArgs Remove<TItem>(params TItem[] items)
        {
            return Remove((IEnumerable<TItem>)items);
        }

        public static NotifyRepositoryChangedEventArgs Remove<TItem>(IEnumerable<TItem> items)
        {
            ArgumentsGuard.ThrowIfNull(items);

            return new NotifyRepositoryChangedEventArgs(NotifyRepositoryChangedAction.Remove, Array.Empty<TItem>(), items);
        }

        public static NotifyRepositoryChangedEventArgs Reset()
        {
            return new NotifyRepositoryChangedEventArgs(NotifyRepositoryChangedAction.Reset, Array.Empty<object>(), Array.Empty<object>());
        }

        private NotifyRepositoryChangedEventArgs(NotifyRepositoryChangedAction action, IEnumerable newItems, IEnumerable oldItems)
        {
            _action = action;
            _newItems = newItems;
            _oldItems = oldItems;
        }
    }
}
