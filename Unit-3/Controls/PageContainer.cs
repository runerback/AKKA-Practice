using Akka.Actor;
using GithubActors.Actors;
using GithubActors.Messages;
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

            App.UIActors
                .ActorSelection(ActorPaths.DispatcherCoordinator)
                .Tell(CreateDispatcherActor.Build<PageNavigatorActor>(
                    ActorNames.PageNavigator, navigateToPage));
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
