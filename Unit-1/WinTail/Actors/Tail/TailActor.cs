using Akka.Actor;
using System.IO;
using System.Text;
using WinTail.Messages.Tail;

namespace WinTail
{
    /// <summary>
    /// Monitors the file at <see cref="filePath"/> for changes and sends
    /// file updates to console.
    /// </summary>
    sealed class TailActor : UntypedActor
    {
        private readonly IActorRef reporterActor;
        private readonly string filePath;

        private FileObserver observer;
        private Stream fileStream;
        private StreamReader fileStreamReader;

        public TailActor(IActorRef reporterActor, string filePath)
        {
            this.reporterActor = reporterActor;
            this.filePath = filePath;
        }

        protected override void OnReceive(object message)
        {
            if (message is FileWrite)
            {
                // move file cursor forward
                // pull results from cursor to end of file and write to output
                // (this is assuming a log file type format that is append-only)
                var text = fileStreamReader.ReadToEnd();
                if (!string.IsNullOrEmpty(text))
                {
                    reporterActor.Tell(text);
                }
            }
            else if (message is FileError fe)
            {
                reporterActor.Tell(string.Format("Tail error: {0}", fe.Reason));
            }
            else if (message is InitialRead ir)
            {
                reporterActor.Tell(ir.Text);
            }
        }

        protected override void PreStart()
        {
            // start watching file for changes
            observer = new FileObserver(Self, Path.GetFullPath(filePath));
            observer.Start();

            // open the file stream with shared read/write permissions
            // (so file can be written to while open)
            fileStream = new FileStream(Path.GetFullPath(filePath),
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileStreamReader = new StreamReader(fileStream, Encoding.UTF8);

            // read the initial contents of the file and send it to console as first msg
            var text = fileStreamReader.ReadToEnd();
            Self.Tell(new InitialRead(filePath, text));
        }

        protected override void PostStop()
        {
            observer.Dispose();
            observer = null;

            fileStreamReader.Close();
            fileStreamReader.Dispose();
            fileStream = null;
            fileStreamReader = null;

            base.PostStop();
        }
    }
}
