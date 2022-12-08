using RedSharp.Sys.Collections.Models;

namespace RedSharp.Sys.Collections.Interfaces
{

    public delegate void NotifyRepositoryChangedEventHandler(object sender, NotifyRepositoryChangedEventArgs arguments);

    public interface INotifyRepositoryChanged
    {
        event NotifyRepositoryChangedEventHandler RepositoryChanged;
    }
}
