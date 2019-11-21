namespace WinTail.Messages.Console
{
    /// <summary>
    /// User provided blank input.
    /// </summary>
    sealed class NullInputError : InputError
    {
        public NullInputError(string reason) : base(reason) { }
    }
}
