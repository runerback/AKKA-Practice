using Akka.Actor;
using GithubActors.Messages;
using System;

namespace GithubActors.Actors
{
    sealed class PageTitleActor : ReceiveActor
    {
        private readonly Action<string> setTitle;

        public PageTitleActor(Action<string> setTitle)
        {
            ActorPathPrinter.Print(Self);

            this.setTitle = setTitle;

            Receive<PageTitle>(title => this.setTitle(title.Value));
        }
    }
}
