namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Special interface to mark that the object is already initialized.
    /// </summary>
    public interface IInitializeIndication
    {
        /// <summary>
        /// Show that the object is initialized or not.
        /// </summary>
        bool IsInitialized { get; }
    }
}
