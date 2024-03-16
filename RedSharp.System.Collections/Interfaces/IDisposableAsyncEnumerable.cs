using System;
using System.Collections.Generic;
using RedSharp.Sys.Interfaces.Shared;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IDisposableAsyncEnumerable<TItem> : IDisposable, IDisposeIndication, IAsyncEnumerable<TItem>
    { }
}
