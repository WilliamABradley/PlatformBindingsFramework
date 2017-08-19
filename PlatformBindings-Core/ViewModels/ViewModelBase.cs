using PlatformBindings.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PlatformBindings.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public ViewModelBase(IUIBindingInfo UIBinding = null)
        {
            this.UIBinding = UIBinding ?? AppServices.UI.DefaultUIBinding;
        }

        public IUIBindingInfo UIBinding { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void UpdateProperty([CallerMemberName]string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
    }
}