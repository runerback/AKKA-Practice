using Akka.Actor;
using GithubActors.Actors;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GithubActors.Controls
{
    sealed class PageContainer : Control
    {
        private readonly IActorRef navigatorActor;

        public PageContainer()
        {
            Action<Page> navigateToPage = SetPage;
            navigatorActor = App.UIActors.ActorOf(
                Props.Create<PageNavigatorActor>(navigateToPage),
                ActorNames.PageNavigator);
        }
        
        public Page Page
        {
            get { return (Page)GetValue(PageProperty); }
        }

        static readonly DependencyPropertyKey PagePropertyKey =
            DependencyProperty.RegisterReadOnly(
                "Page",
                typeof(Page),
                typeof(PageContainer),
                new PropertyMetadata());

        public static readonly DependencyProperty PageProperty =
            PagePropertyKey.DependencyProperty;

        private void SetPage(Page value)
        {
            SetValue(PagePropertyKey, value);
        }
    }
}
