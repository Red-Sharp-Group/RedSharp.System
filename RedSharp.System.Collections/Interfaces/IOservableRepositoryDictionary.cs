namespace RedSharp.Sys.Collections.Interfaces
{
    public interface IOservableRepositoryDictionary<TIdentifier, TItem> : IRepositoryDictionary<TIdentifier, TItem>, IObservableRepositoryCollection<TItem> 
        where TItem : IRepositoryItem<TIdentifier>
    {
        /// <summary>
        /// Basically, this is <see cref="GetByIdentifier"/> in form of indexer.
        /// </summary>
        /// <remarks>
        /// And yeah, it will execute synchronously.
        /// </remarks>
        public TItem this[TIdentifier identifier] { get; }
    }
}
