using Akka.Actor;
using GithubActors.Messages;
using System;
using System.Collections.Generic;

namespace GithubActors.Actors
{
    sealed class PageNavigatorActor : ReceiveActor
    {
        private readonly Action<Page> navigateTo;

        private readonly Stack<PageNavigate> pages = new Stack<PageNavigate>();

        public PageNavigatorActor(Action<Page> navigateTo)
        {
            this.navigateTo = navigateTo ?? throw new ArgumentNullException(nameof(navigateTo));

            Receive<PageNavigate>(navigation =>
            {
                this.navigateTo(navigation.Page);

                App.UIActors
                    .ActorSelection(ActorPaths.PageTitle)
                    .Tell(new PageTitle(navigation.Title));

                if (navigation.Stash)
                    pages.Push(navigation);
            });

            Receive<PageNavigateBack>(_ =>
            {
                if (pages.Count > 0)
                {
                    Self.Tell(pages.Pop());
                }
            });
        }
    }
}
