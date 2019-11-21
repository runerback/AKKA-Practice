namespace WinTail.Messages.Console
{
    /// <summary>
    /// Base class for signalling that user input was valid.
    /// </summary>
    sealed class InputSuccess
    {
        public InputSuccess(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }
}
