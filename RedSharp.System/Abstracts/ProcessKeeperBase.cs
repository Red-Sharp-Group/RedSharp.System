using System;
using System.Diagnostics;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Interfaces.Entities;

namespace RedSharp.Sys.Abstracts
{
    /// <summary>
    /// object that represents external application in this application.
    /// </summary>
    /// <remarks>
    /// By default this object has to be used as a wrapper, it cannot start or kill the process. 
    /// </remarks>
    public abstract class ProcessKeeperBase : NotifiableCriticalDisposableBase, IProcessKeeper
    {
        private string _exitApplicationError;

        /// <inheritdoc/>
        public bool IsProcessOwner { get; private set; }

        /// <inheritdoc/>
        public Process AssociatedProcess { get; private set; }

        /// <summary>
        /// Associates the process with this object.
        /// </summary>
        /// <remarks>
        /// If object has associated process it will be removed from this object.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If process is null.</exception>
        /// <exception cref="ArgumentException">If process is exited.</exception>
        protected virtual void Associate(Process process, bool isProcessOwner)
        {
            ThrowIfDisposed();
            ArgumentsGuard.ThrowIfNull(process, nameof(process));

            if (process.HasExited)
                throw new ArgumentException("Process is already exited.");

            //Before check to make possible to change owning with the same process
            IsProcessOwner = isProcessOwner;

            if (AssociatedProcess == process)
                return;

            if (AssociatedProcess != null)
                AssociatedProcess.Exited -= OnAssociatedProcessExited;

            AssociatedProcess = process;
            AssociatedProcess.EnableRaisingEvents = true;
            AssociatedProcess.Exited += OnAssociatedProcessExited;
        }

        private void OnAssociatedProcessExited(object sender, EventArgs arguments)
        {
            AssociatedProcess.Exited -= OnAssociatedProcessExited;

            if (AssociatedProcess.ExitCode != 0)
                _exitApplicationError = $"The application is failed with error: 0x{AssociatedProcess.ExitCode.ToString("X8")}";
            else
                _exitApplicationError = $"The application is exited without error, it was ended without disposing from this application";

            Dispose();
        }

        /// <inheritdoc/>
        protected override void InternalDispose(bool manual)
        {
            if (AssociatedProcess != null)
            {
                if (!AssociatedProcess.HasExited && IsProcessOwner)
                {
                    try
                    {
                        AssociatedProcess.EnableRaisingEvents = false;

                        KillProcess();

                        _exitApplicationError = "The application was terminated by disposing of this object";
                    }
                    catch (Exception exception)
                    {
                        Trace.WriteLine(exception.Message);
                        Trace.WriteLine(exception.StackTrace);
                    }
                }

                AssociatedProcess.Exited -= OnAssociatedProcessExited;
                AssociatedProcess = null;
            }

            base.InternalDispose(manual);
        }

        /// <summary>
        /// Contains logic of the process killing
        /// </summary>
        protected virtual void KillProcess()
        {
            AssociatedProcess.Kill();
        }

        protected override void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(IDisposable), _exitApplicationError ?? DisposableBase.ObjectDisposedError);
        }
    }
}
