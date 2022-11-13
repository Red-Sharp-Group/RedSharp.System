namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Special interface to mark that the object can be modified or not.
    /// </summary>
    /// <note>
    /// This is not good practice when you have to show that a huge part of you functionality is not supported,
    /// but good practices and real-life do not often meet each other.
    /// </note>
    public interface IReadableIndication
    {
        /// <summary>
        /// True if this object supports only read operations.
        /// </summary>
        bool IsReadOnly { get; }
    }
}
