using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Prism.Mvvm;

namespace CleanStart.ViewModels
{
    /// <summary>
    ///     Class ProgessWindowViewModel.
    /// </summary>
    public class ProgressWindowViewModel : BindableBase
    {
        #region Fields

        private string statusMessage;
        private double progress;

        #endregion

        #region Constructor

        public ProgressWindowViewModel(string pathToVisualStudio, string pathToSolution)
        {
            this.StatusMessage = "Please wait...";
            this.Action(pathToVisualStudio, pathToSolution);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Gets or sets the progress.
        /// </summary>
        /// <value>The progress.</value>
        public double Progress
        {
            get { return this.progress; }
            set { this.SetProperty(ref this.progress, value); }
        }

        /// <summary>
        ///     Gets or sets the status message.
        /// </summary>
        /// <value>The status message.</value>
        public string StatusMessage
        {
            get { return this.statusMessage; }
            set { this.SetProperty(ref this.statusMessage, value); }
        }

        #endregion

        #region Subroutines

        private async void Action(string pathToVisualStudio, string pathToSolution)
        {
            await Task.Delay(3000);
            var dirsToClean = GetDiretories(Path.GetDirectoryName(pathToSolution), "bin", "obj").ToList();
            foreach (var dir in dirsToClean)
            {
                if (!Directory.Exists(dir))
                    continue;
                var directoryName = Path.GetDirectoryName(pathToSolution);
                if (directoryName != null)
                    this.StatusMessage = $"Delete {dir.Substring(directoryName.Length)}";
                Directory.Delete(dir, true);
                await Task.Delay(500);
            }
            this.StatusMessage = "Solution cleaned. Starting Visual Studio...";
            await Task.Delay(3000);

            var startInfo = new ProcessStartInfo(pathToVisualStudio, $"\"{pathToSolution}\"") {UseShellExecute = true};

            Process.Start(startInfo);

            Application.Current.Shutdown();
        }

        private static IEnumerable<string> GetDiretories(string sDir, params string[] patterns)
        {
            var allDirs = Directory.GetDirectories(sDir).ToList();
            var filteredDirs = allDirs;
            if (patterns.Length > 0)
                filteredDirs = allDirs.Where(p => patterns.Contains(Path.GetFileName(p))).ToList();
            return filteredDirs.Concat(allDirs.SelectMany(e => GetDiretories(e, patterns)));
        }

        #endregion
    }
}