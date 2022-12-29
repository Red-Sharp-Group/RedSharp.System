using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Repositories.Interfaces;
using RedSharp.Sys.Repositories.Models;

namespace RedSharp.Sys.Repositories.Abstracts
{
    public class NotifyingRepositoryBase<TItem> : INotifyRepositoryChanged<TItem>
    {
        public event RepositoryChangedEventHandler<TItem> RepositoryChanged;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RaiseRepositoryChanged(RepositoryEventArgs<TItem> arguments)
        {
            try
            {
                RepositoryChanged.Invoke(this, arguments);
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception.Message);
                Trace.WriteLine(exception.StackTrace);
            }
        }


        protected void RaiseAdding(params TItem[] items)
        {
            RaiseAdding((IEnumerable<TItem>)items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RaiseAdding(IEnumerable<TItem> items)
        {
            if (RepositoryChanged == null)
                return;

            RaiseRepositoryChanged(RepositoryEventArgs<TItem>.Add(items));
        }


        protected void RaiseUpdating(params TItem[] items)
        {
            RaiseUpdating((IEnumerable<TItem>)items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RaiseUpdating(IEnumerable<TItem> items)
        {
            if (RepositoryChanged == null)
                return;

            RaiseRepositoryChanged(RepositoryEventArgs<TItem>.Update(items));
        }


        protected void RaiseRemoving(params TItem[] items)
        {
            RaiseRemoving((IEnumerable<TItem>)items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void RaiseRemoving(IEnumerable<TItem> items)
        {
            if (RepositoryChanged == null)
                return;

            RaiseRepositoryChanged(RepositoryEventArgs<TItem>.Remove(items));
        }


        protected void RaiseReset()
        {
            if (RepositoryChanged == null)
                return;

            RaiseRepositoryChanged(RepositoryEventArgs<TItem>.Reset());
        }
    }
}
