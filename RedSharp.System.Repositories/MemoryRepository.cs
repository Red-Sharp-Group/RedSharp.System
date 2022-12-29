using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Repositories.Abstracts;
using RedSharp.Sys.Repositories.Enums;
using RedSharp.Sys.Repositories.Interfaces;

namespace RedSharp.Sys.Repositories
{
    public class MemoryRepository<TIdentifier, TItem> : MemoryRepositoryBase<TIdentifier, TItem>
    {
        private Func<TItem, TIdentifier> _getter;

        public MemoryRepository(RepositoryCapabilities caps = RepositoryCapabilities.Reading, Func<TItem, TIdentifier> getter = null)
            : this(new Dictionary<TIdentifier, TItem>(), caps, getter)
        { }

        public MemoryRepository(IDictionary<TIdentifier, TItem> collection, RepositoryCapabilities caps = RepositoryCapabilities.Reading, Func<TItem, TIdentifier> getter = null)
            : base(collection, caps)
        {
            if (Capabilities.HasFlag(RepositoryCapabilities.Adding))
                ArgumentsGuard.ThrowIfNull(getter);

            _getter = getter;
        }

        protected override TIdentifier GetIdentifier(TItem item)
        {
            return _getter.Invoke(item);
        }
    }
}
