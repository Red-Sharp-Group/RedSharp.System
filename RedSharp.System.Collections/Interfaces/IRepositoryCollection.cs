﻿using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IRepositoryCollection<TItem> : IRepositoryEnumerable<TItem>
    {
        /// <summary>
        /// True if the repository supports add, update, remove operations.
        /// </summary>
        bool IsReadOnly { get; }


        /// <summary>
        /// Returns exact number of items on the invocation moment.
        /// <br/> If the parameter is null has to return total number of items.
        /// <br/> Uses <see cref="Expression"/> because it can be compiled into <see cref="Delegate"/> but otherwise - not.
        /// </summary>
        int Count(Expression<Func<TItem, bool>> predicate = null);

        /// <inheritdoc cref="Count"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<int> CountAsync(Expression<Func<TItem, bool>> predicate = null, CancellationToken token = default);


        /// <summary>
        /// Adds new item to the repository.
        /// </summary>
        void Add(TItem item);

        /// <inheritdoc cref="Add"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask AddAsync(TItem item, CancellationToken token = default);


        /// <summary>
        /// Check item existing in the repository
        /// <br/>Returns true if the item was actually removed.
        /// </summary>
        bool Contains(TItem item);

        /// <inheritdoc cref="Contains"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<bool> ContainsAsync(TItem item, CancellationToken token = default);


        /// <summary>
        /// Apply changes for the given item.
        /// </summary>
        void Update(TItem item);

        /// <inheritdoc cref="Update"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask UpdateAsync(TItem item, CancellationToken token = default);


        /// <summary>
        /// Removes item from the repository.
        /// <br/>Returns true if the item was actually removed.
        /// </summary>
        bool Remove(TItem item);

        /// <inheritdoc cref="Remove"/>
        /// <remarks>
        /// Async version.
        /// </remarks>
        ValueTask<bool> RemoveAsync(TItem item, CancellationToken token = default);
    }
}