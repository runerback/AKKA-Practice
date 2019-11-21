namespace WinTail.Messages.Console
{
    /// <summary>
    /// Base class for signalling that user input was invalid.
    /// </summary>
    class InputError
    {
        public InputError(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }
}
