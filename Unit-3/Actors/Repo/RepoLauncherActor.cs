using Akka.Actor;
using GithubActors.Messages;
using Octokit;
using System;

namespace GithubActors.Actors
{
    sealed class RepoLauncherActor : ReceiveActor, IWithUnboundedStash
    {
        private IActorRef repoValidatorActor;

        public RepoLauncherActor()
        {
            Initializing();
        }

        private void Initializing()
        {
            Receive<ValidateRepo>(repo => ((IWithUnboundedStash)this).Stash.Stash());

            Receive<AuthenticationSuccess>(auth =>
            {
                App.UIActors
                    .ActorSelection(ActorPaths.PageNavigator)
                    ?.Tell(PageNavigate.Create<Views.LauncherForm, ViewModels.LauncherForm>("Who Starred This Repo?"));

                repoValidatorActor = App.GithubActors.ActorOf(
                    Props.Create<GetHubRepoValidatorActor>(GithubClientFactory.GetClientFactory(auth.Token)),
                    ActorNames.RepoValidator);

                BecomeReady();
            });
        }

        private void BecomeReady()
        {
            Become(Ready);

            ((IWithUnboundedStash)this).Stash.UnstashAll();
        }

        private void Ready()
        {
            //Outright invalid URLs
            Receive<ValidateRepo>(
                repo => string.IsNullOrEmpty(repo.URL) || !Uri.IsWellFormedUriString(repo.URL, UriKind.Absolute),
                repo => Sender.Tell(new InvalidRepo(repo.URL, "Not a valid absolute URI")));

            //Repos that at least have a valid absolute URL
            Receive<ValidateRepo>(repoAddress =>
            {
                repoValidatorActor.Tell(repoAddress);
            });

            // something went wrong while querying github, sent to ourselves via PipeTo
            // however - Sender gets preserved on the call, so it's safe to use Forward here.
            Receive<InvalidRepo>(repo => Sender.Forward(repo));

            // Octokit was able to retrieve this repository
            Receive<Repository>(repository =>
            {
                //ask the GithubCommander if we can accept this job
                Context.ActorSelection(ActorPaths.GithubCommander).Tell(
                    new CanAcceptJob(repository.Name, repository.Owner.Login));
            });

            /* REPO is valid, but can we process it at this time? */

            //yes
            Receive<UnableToAcceptJob>(job => Context.ActorSelection(ActorPaths.MainForm).Tell(job));

            //no
            Receive<AbleToAcceptJob>(job => Context.ActorSelection(ActorPaths.MainForm).Tell(job));
        }

        IStash IActorStash.Stash { get; set; }
    }
}
