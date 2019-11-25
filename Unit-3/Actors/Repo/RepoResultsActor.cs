using Akka.Actor;
using GithubActors.Messages;
using GithubActors.Model;
using System;
using System.Collections.Generic;

namespace GithubActors.Actors
{
    sealed class RepoResultsActor : ReceiveActor
    {
        private readonly Action<Repo> addRepoResult;

        public RepoResultsActor(Action<Repo> addRepoResult)
        {
            this.addRepoResult = addRepoResult ??
                throw new ArgumentNullException(nameof(addRepoResult));

            //user update
            Receive<IEnumerable<SimilarRepo>>(repos =>
            {
                var addRepo = this.addRepoResult;

                foreach (var similarRepo in repos)
                {
                    var repo = similarRepo.Repo;

                    addRepo(new Repo(repo.Name, repo.Owner.Login, repo.HtmlUrl)
                    {
                        SharedStarsCount = similarRepo.SharedStarrers,
                        OpenIssuesCount = repo.OpenIssuesCount,
                        StarsCount = repo.StargazersCount,
                        ForksCount = repo.ForksCount
                    });
                }
            });
        }
    }
}
