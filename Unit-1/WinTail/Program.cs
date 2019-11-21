using System;
﻿using Akka.Actor;

namespace WinTail
{
    class Program
    {
        public static ActorSystem MyActorSystem = ActorSystem.Create("ThisIsActorSystem1HowCopyOver");

        static void Main(string[] args)
        {
            var consoleWriterActorProps = Props.Create<ConsoleWriterActor>();
            var consoleWriterActor = MyActorSystem.ActorOf(consoleWriterActorProps, "consoleWriterActor");

            var tailCoordinatorProps = Props.Create<TailCoordinatorActor>();
            MyActorSystem.ActorOf(tailCoordinatorProps, "tailCoordinatorActor");

            var fileValidatorActorProps = Props.Create<FileValidatorActor>(consoleWriterActor);
            MyActorSystem.ActorOf(fileValidatorActorProps, "validatorActor");

            var consoleReaderActorProps = Props.Create<ConsoleReaderActor>();
            var consoleReaderActor = MyActorSystem.ActorOf(consoleReaderActorProps, "consoleReaderActor");

            // tell console reader to begin
            consoleReaderActor.Tell(ConsoleReaderActor.StartCommand);

            // blocks the main thread from exiting until the actor system is shut down
            MyActorSystem.WhenTerminated.Wait();
        }
    }
}
