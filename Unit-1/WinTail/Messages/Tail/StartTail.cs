using Akka.Actor;

namespace WinTail.Messages.Tail
{
    /// <summary>
    /// Start tailing the file at user-specified path.
    /// </summary>
    sealed class StartTail
    {
        public StartTail(string filePath, IActorRef reporterActor)
        {
            FilePath = filePath;
            ReporterActor = reporterActor;
        }

        public string FilePath { get; }

        public IActorRef ReporterActor { get; }
    }
}
