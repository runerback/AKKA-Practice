using Akka.Actor;
using GithubActors.Messages;
using Octokit;

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
                    .ActorSelection(ActorPaths.DispatcherCoordinator)
                    .Tell(PageNavigate.Create<Views.LauncherForm, ViewModels.LauncherForm>(
                        "Who Starred This Repo?", true));

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
            Receive<ValidateRepo>(repoAddress =>
            {
                GetRepoStatusCoordinator().Tell(repoAddress);
                repoValidatorActor.Tell(repoAddress);
            });

            // something went wrong while querying github
            Receive<InvalidRepo>(repo => GetRepoStatusCoordinator().Tell(repo));

            // Octokit was able to retrieve this repository
            Receive<Repository>(repository =>
            {
                GetRepoStatusCoordinator().Tell(new ValidRepo(repository.HtmlUrl));

                //ask the GithubCommander if we can accept this job
                var canAccessJob = new CanAcceptJob(repository.Name, repository.Owner.Login);

                Context.ActorSelection(ActorPaths.GithubCommander).Tell(canAccessJob);

                GetRepoStatusCoordinator().Tell(canAccessJob);
            });

            // REPO is valid, but there already has job running
            Receive<UnableToAcceptJob>(job => GetRepoStatusCoordinator().Tell(job));
            
            Receive<AbleToAcceptJob>(job =>
            {
                GetRepoStatusCoordinator().Tell(job);

                var mainformActor = Context.ActorOf(
                    Props.Create<MainFormActor>(),
                    ActorNames.MainForm);
                mainformActor.Tell(new ProcessRepo(job));
            });
        }

        private ActorSelection GetRepoStatusCoordinator()
        {
            return Context.ActorSelection(ActorPaths.RepoStatusCoordinator);
        }

        IStash IActorStash.Stash { get; set; }
    }
}
