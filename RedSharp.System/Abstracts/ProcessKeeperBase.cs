using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RedSharp.Sys.Helpers;
using RedSharp.Sys.Interfaces.Entities;

namespace RedSharp.Sys.Abstracts
{
    public abstract class ProcessKeeperBase : CriticalDisposableBase, IProcessKeeper
    {
        private String _exitApplicationError;

        public Process AssociatedProcess { get; private set; }

        protected virtual void Associate(Process process)
        {
            ArgumentsGuard.ThrowIfNull(process, nameof(process));

            if (process.HasExited)
                throw new ArgumentException("Process is already exited.");

            if (AssociatedProcess != process)
                return;

            if (AssociatedProcess != null)
                AssociatedProcess.Exited -= OnAssociatedProcessExited;

            AssociatedProcess = process;
            AssociatedProcess.EnableRaisingEvents = true;
            AssociatedProcess.Exited += OnAssociatedProcessExited;
        }

        private void OnAssociatedProcessExited(Object sender, EventArgs arguments)
        {
            AssociatedProcess.Exited -= OnAssociatedProcessExited;

            if (AssociatedProcess.ExitCode != 0)
                _exitApplicationError = $"The application is failed with error: 0x{AssociatedProcess.ExitCode.ToString("X8")}";
            else
                _exitApplicationError = $"The application is exited without error, it was ended without disposing from this application.";

            Dispose();
        }

        protected override void InternalDispose(bool manual)
        {
            if (AssociatedProcess != null)
            {
                AssociatedProcess.Exited -= OnAssociatedProcessExited;
                AssociatedProcess = null;
            }

            base.InternalDispose(manual);
        }

        protected override void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(nameof(IDisposable), _exitApplicationError ?? DisposableBase.ObjectDisposedError);
        }
    }
}
