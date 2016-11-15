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

        public ProgressWindowViewModel()
        {
            this.StatusMessage = "Please wait...";
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
    }
}