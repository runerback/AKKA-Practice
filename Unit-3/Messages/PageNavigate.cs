using System.ComponentModel;
using System.Windows;

namespace GithubActors.Messages
{
    sealed class PageNavigate
    {
        private PageNavigate(Page page, string title, bool stash)
        {
            Page = page;
            Title = title;
            Stash = stash;
        }

        public Page Page { get; }
        public string Title { get; }
        public bool Stash { get; }

        /// <summary>
        /// Create new PageNavigate message.
        /// </summary>
        /// <typeparam name="TView">view</typeparam>
        /// <typeparam name="TContext">corresponding view model</typeparam>
        /// <param name="title">window title</param>
        /// <param name="stash">whether stash this page for navigating back.</param>
        public static PageNavigate Create<TView, TContext>(string title = null, bool stash = false)
            where TView : FrameworkElement, new()
            where TContext : class, INotifyPropertyChanged, new()
        {
            return new PageNavigate(
                new Page(
                    new TView(),
                    new TContext()),
                title,
                stash);
        }
    }
}
