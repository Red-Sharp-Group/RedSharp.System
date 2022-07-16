namespace RedSharp.Sys.Interfaces.Shared
{
    /// <summary>
    /// Uses in a pair with <see cref="IFreezable"/> to indicate current state to others.
    /// </summary>
    public interface IFreezeIndication
    {
        /// <summary>
        /// Shows that the object was frozen and cannot be changed or vice versa.
        /// </summary>
        bool IsFrozen { get; }
    }
}
