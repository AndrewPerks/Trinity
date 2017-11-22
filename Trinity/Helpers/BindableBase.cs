using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Trinity.Helpers
{
    /// <summary>
    /// Provides support for the INotifyPropertyChanged interface and capabilities for easy implementation of bindable properties with the OnPropertyChanged and SetProperty methods.
    /// </summary>
    public class BindableBase : INotifyPropertyChanged
    {
        protected virtual void SetProperty<T>(ref T member, T val, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate {};
    }
}
