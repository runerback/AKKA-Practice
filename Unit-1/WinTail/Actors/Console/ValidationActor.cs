using Akka.Actor;
using WinTail.Messages.Console;

namespace WinTail
{
    sealed class ValidationActor : UntypedActor
    {
        private readonly IActorRef consoleWriterActor;

        public ValidationActor(IActorRef consoleWriterActor)
        {
            this.consoleWriterActor = consoleWriterActor;
        }

        protected override void OnReceive(object message)
        {
            var msg = message as string;
            if (string.IsNullOrEmpty(msg))
            {
                // signal that the user needs to supply an input
                consoleWriterActor.Tell(
                  new NullInputError("No input received."));
            }
            else
            {
                var valid = IsValid(msg);
                if (valid)
                {
                    // send success to console writer
                    consoleWriterActor.Tell(new InputSuccess("Thank you! Message was valid."));
                }
                else
                {
                    // signal that input was bad
                    consoleWriterActor.Tell(new ValidationError("Invalid: input had odd number of characters."));
                }
            }

            // tell sender to continue doing its thing
            // (whatever that may be, this actor doesn't care)
            Sender.Tell(new ContinueProcessing());
        }

        /// <summary>
        /// Determines if the message received is valid.
        /// Checks if number of chars in message received is even.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private static bool IsValid(string msg)
        {
            var valid = msg.Length % 2 == 0;
            return valid;
        }
    }
}
