using Akka.Actor;
using GithubActors.Messages;
using Octokit;
using System;

namespace GithubActors.Actors
{
    sealed class GetHubRepoValidatorActor : ReceiveActor
    {
        private readonly Func<IGitHubClient> gitHubClientFactory;

        private IGitHubClient githubClient;

        public GetHubRepoValidatorActor(Func<IGitHubClient> gitHubClientFactory)
        {
            this.gitHubClientFactory = gitHubClientFactory ??
                throw new ArgumentNullException(nameof(gitHubClientFactory));

            //Outright invalid URLs
            Receive<ValidateRepo>(
                repo => string.IsNullOrEmpty(repo.URL) || !Uri.IsWellFormedUriString(repo.URL, UriKind.Absolute),
                repo => Sender.Tell(new InvalidRepo(repo.URL, "Not a valid absolute URI")));

            //Repos that at least have a valid absolute URL
            Receive<ValidateRepo>(repoAddress =>
            {
                var repo = RepoKey.FromAddress(repoAddress);

                //close over the sender in an instance variable
                var sender = Sender;
                githubClient.Repository.Get(repo.Owner, repo.Repo)
                    .ContinueWith<object>(t =>
                    {
                        //Rule #1 of async in Akka.NET - turn exceptions into messages your actor understands
                        if (t.IsCanceled)
                        {
                            return new InvalidRepo(repoAddress.URL, "Repo lookup timed out");
                        }
                        if (t.IsFaulted)
                        {
                            return new InvalidRepo(
                                repoAddress.URL, 
                                t.Exception != null ? t.Exception.GetBaseException().Message : "Unknown Octokit error");
                        }

                        return t.Result;
                    })
                    .PipeTo(Self, sender);
            });
        }

        protected override void PreStart()
        {
            githubClient = gitHubClientFactory();

            base.PreStart();
        }
    }
}
