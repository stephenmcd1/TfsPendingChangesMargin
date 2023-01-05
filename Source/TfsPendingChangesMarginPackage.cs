using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AlekseyNagovitsyn.TfsPendingChangesMargin.Settings;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace AlekseyNagovitsyn.TfsPendingChangesMargin
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true
#if VS2022
        , AllowsBackgroundLoading = true
#endif
    )]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [Guid("9ec6e3aa-48cd-4953-b3b7-1a203bfded7f")]
    [ProvideOptionPage(typeof(GeneralSettingsPage), "Tfs Pending Changes Margin", "General", 0, 0, true)]
    public sealed class TfsPendingChangesMarginPackage :
#if VS2022
        AsyncPackage
#else
        Package
#endif
    {
        /// <summary>
        /// Returns the settings from the Tools|Options... dialog's Tfs Pending Changes Margin|General section
        /// </summary>
        public static EnvDTE.Properties GetGeneralSettings()
        {
            var dte = (DTE)GetGlobalService(typeof(DTE));
            return dte.Properties["Tfs Pending Changes Margin", "General"];
        }

        /// <summary>
        /// Event raised when the settings on the <see cref="GeneralSettingsPage"/> are changed
        /// </summary>
        public static event Action GeneralSettingsChanged;

        /// <summary>
        /// Raises the <see cref="GeneralSettingsChanged"/> event.
        /// </summary>
        public static void RaiseGeneralSettingsChanged()
        {
            if (GeneralSettingsChanged != null)
                GeneralSettingsChanged();
        }

#if VS2022

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
        }

#endif
    }
}
