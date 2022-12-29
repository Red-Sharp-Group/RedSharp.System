using RedSharp.Sys.Repositories.Models;

namespace RedSharp.Sys.Repositories.Interfaces
{
    public delegate void RepositoryChangedEventHandler<TItem>(object sender, RepositoryEventArgs<TItem> arguments);

    public interface INotifyRepositoryChanged<TItem>
    {
        event RepositoryChangedEventHandler<TItem> RepositoryChanged;
    }
}
