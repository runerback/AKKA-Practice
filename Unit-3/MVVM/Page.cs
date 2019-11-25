using System;
using System.ComponentModel;
using System.Windows;

namespace GithubActors
{
    sealed class Page
    {
        public Page(FrameworkElement view, INotifyPropertyChanged viewModel)
        {
            View = view ?? throw new ArgumentNullException(nameof(view));
            Context = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            View.DataContext = Context;
        }

        public FrameworkElement View { get; }
        public INotifyPropertyChanged Context { get; }
    }
}
