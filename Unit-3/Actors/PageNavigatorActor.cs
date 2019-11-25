using Akka.Actor;
using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class PageNavigatorActor : ReceiveActor
    {
        private readonly Action<Page> navigateTo;

        public PageNavigatorActor(Action<Page> navigateTo)
        {
            this.navigateTo = navigateTo ?? throw new ArgumentNullException(nameof(navigateTo));

            Receive<PageNavigate>(data =>
            {
                this.navigateTo(data.Page);

                App.UIActors
                    .ActorSelection(ActorPaths.PageTitle)
                    ?.Tell(new PageTitle(data.Title));
            });
        }
    }
}
