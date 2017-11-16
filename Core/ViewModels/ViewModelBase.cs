using PlatformBindings.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlatformBindings.ViewModels
{
    /// <summary>
    /// Viewmodel Base Class, Inherit your ViewModels from this class in order to automatically fetch the Default UI Binding, or store a UI Binding of the ViewModel, and Perform work on the UI Thread.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase()
        {
            UIBinding = AppServices.UI.DefaultUIBinding;
        }

        /// <summary>
        /// Constructor for ViewModelBase, use this to Provide a Custom UI Binding, such as on UWP, it can be used to hold a reference to the Current Open Content Dialog, in order to safely switch Dialogs.
        /// </summary>
        /// <param name="UIBinding"></param>
        public ViewModelBase(IUIBindingInfo UIBinding)
        {
            this.UIBinding = UIBinding;
        }

        /// <summary>
        /// The current UI Context of this ViewModel. This contains methods to Dispatch work on the UI Thread.
        /// </summary>
        public IUIBindingInfo UIBinding { get; }

        /// <summary>
        /// The PropertyChanged Event for INotifyPropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Updates the Specified Property, using INotifyPropertyChanged.
        /// </summary>
        /// <param name="PropertyName">Property to Update</param>
        protected void UpdateProperty([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}