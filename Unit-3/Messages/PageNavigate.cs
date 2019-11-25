using System.ComponentModel;
using System.Windows;

namespace GithubActors.Messages
{
    sealed class PageNavigate
    {
        private PageNavigate(Page page, string title)
        {
            Page = page;
            Title = title;
        }

        public Page Page { get; }
        public string Title { get; }

        public static PageNavigate Create<TView, TContext>(string title)
            where TView : FrameworkElement, new()
            where TContext : class, INotifyPropertyChanged, new()
        {
            return new PageNavigate(
                new Page(
                    new TView(),
                    new TContext()),
                title);
        }
    }
}
