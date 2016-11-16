#region Imports

using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;

using CleanStart.ViewModels;

using EnvDTE80;

using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

#endregion

namespace VisualStudioCleanRestart
{
    /// <summary>
    ///     Command handler
    /// </summary>
    internal sealed class RestartVisualStudioCommand
    {
        #region Constants

        /// <summary>
        ///     Command ID.
        /// </summary>
        public const int COMMAND_ID = 0x0100;

        #endregion

        #region Fields

        /// <summary>
        ///     Command menu group (command set GUID).
        /// </summary>
        private static readonly Guid _commandSet = new Guid("0b5342e7-d6f2-45cf-8ed2-821097006467");

        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="RestartVisualStudioCommand" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private RestartVisualStudioCommand(Package package)
        {
            if (package == null)
                throw new ArgumentNullException(nameof(package));

            this.Package = package;

            var commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(_commandSet, COMMAND_ID);
                var menuItem = new MenuCommand(this.MenuItemCallback, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static RestartVisualStudioCommand Instance { get; private set; }

        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        public Package Package { get; }

        /// <summary>
        ///     Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider => this.Package;

        #endregion

        #region Method

        /// <summary>
        ///     Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Package package)
        {
            Instance = new RestartVisualStudioCommand(package);
        }

        #endregion

        #region Subroutines

        private ProcessStartInfo GetScriptProcess(string solutionPath, string vsPath)
        {
            var assemblyLocation = typeof(ProgressWindowViewModel).Assembly.Location;
            return new ProcessStartInfo
            {
                FileName = assemblyLocation,
                ErrorDialog = true,
                UseShellExecute = true,
                Arguments = $"\"{vsPath}\" \"{solutionPath}\""
            };
        }

        /// <summary>
        ///     This function is the callback used to execute the command when the menu item is clicked.
        ///     See the constructor to see how the menu item is associated with this function using
        ///     OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var dte2 = Package.GetGlobalService(typeof(SDTE)) as DTE2;
            if (dte2 == null)
                return;
            var currentProcess = Process.GetCurrentProcess();
            var vsFile = currentProcess.MainModule.FileName;
            var fullName = dte2.Solution.FullName;

            var process = this.GetScriptProcess(fullName, vsFile);
            Process.Start(process);
            dte2.Quit();
        }

        #endregion
    }
}