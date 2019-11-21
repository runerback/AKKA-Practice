using System;
using Akka.Actor;
using WinTail.Messages.Console;

namespace WinTail
{
    /// <summary>
    /// Actor responsible for serializing message writes to the console.
    /// (write one message at a time, champ :)
    /// </summary>
    class ConsoleWriterActor : UntypedActor
    {
        protected override void OnReceive(object message)
        {
            if (message is InputError inputErrorMessage)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(inputErrorMessage.Reason);
            }
            else if (message is InputSuccess inputSuccessMessage)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(inputSuccessMessage.Reason);
            }
            else
            {
                Console.WriteLine(message);
            }

            Console.ResetColor();
        }
    }
}
