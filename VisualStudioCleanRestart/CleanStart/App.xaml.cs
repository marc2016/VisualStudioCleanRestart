using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using CleanStart.ViewModels;
using CleanStart.Views;

namespace CleanStart
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var pathToVisualStudio = e.Args[0];
            var pathToSolution = e.Args[1];

            var viewModel = new ProgressWindowViewModel(pathToVisualStudio, pathToSolution);
            var window = new ProgressWindow {DataContext = viewModel};

            window.Show();
        }
    }
}
