using System.ComponentModel;

namespace GithubActors
{
    abstract class ViewModelBase : INotifyPropertyChanged
    {
        private PropertyChangedEventHandler propertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return;

            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { propertyChanged += value; }
            remove { propertyChanged -= value; }
        }
    }
}
