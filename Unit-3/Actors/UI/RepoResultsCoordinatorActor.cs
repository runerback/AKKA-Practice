using Akka.Actor;
using GithubActors.Messages;
using GithubActors.Model;
using System;
using System.Collections.Generic;

namespace GithubActors.Actors
{
    sealed class RepoResultsCoordinatorActor : ReceiveActor
    {
        private readonly IActorRef resultsActor;
        private readonly IActorRef statusActor;
        
        public RepoResultsCoordinatorActor(
            Action<Repo> addRepoResult,
            Func<RepoRequestProgress, RepoRequestProgress> updateProgress,
            Action<string> updateStatus)
        {
            ActorPathPrinter.Print(Self);

            resultsActor = Context.ActorOf(
                Props.Create<RepoResultsActor>(addRepoResult),
                ActorNames.RepoResults);

            statusActor = Context.ActorOf(
                Props.Create<RepoResultsStatusActor>(updateProgress, updateStatus),
                ActorNames.RepoProgress);
            
            Receive<GithubProgressStats>(stats =>
            {
                statusActor.Tell(stats);
            });

            Receive<IEnumerable<SimilarRepo>>(repos =>
           {
               resultsActor.Tell(repos);
           });

            Receive<JobFailed>(failed =>
            {
                statusActor.Tell(failed);
            });
        }
    }
}
