using Akka.Actor;
using System;

namespace GithubActors.Messages
{
    sealed class CreateDispatcherActor
    {
        private CreateDispatcherActor(Type actorType, object[] ctorParams, string name)
        {
            ActorType = actorType;
            ConstructorParams = ctorParams;
            ActorName = name;
        }

        public Type ActorType { get; }
        public object[] ConstructorParams { get; }
        public string ActorName { get; }

        public static CreateDispatcherActor Build<TActor>(string name = null, params object[] parameters)
            where TActor : ActorBase
        {
            return new CreateDispatcherActor(typeof(TActor), parameters, name);
        }
    }
}
