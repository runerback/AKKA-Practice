using Akka.Actor;
using System;
using WinTail.Messages.Tail;

namespace WinTail
{
    sealed class TailCoordinatorActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is StartTail startTailMessage)
            {
                // here we are creating our first parent/child relationship!
                // the TailActor instance created here is a child
                // of this instance of TailCoordinatorActor
                Context.ActorOf(Props.Create<TailActor>(startTailMessage.ReporterActor, startTailMessage.FilePath));
            }
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                10, // maxNumberOfRetries
                TimeSpan.FromSeconds(30), // withinTimeRange
                x => // localOnlyDecider
                {
                    //Maybe we consider ArithmeticException to not be application critical
                    //so we just ignore the error and keep going.
                    if (x is ArithmeticException) return Directive.Resume;

                    //Error that we cannot recover from, stop the failing actor
                    else if (x is NotSupportedException) return Directive.Stop;

                    //In all other cases, just restart the failing actor
                    else return Directive.Restart;
                });
        }
    }
}
