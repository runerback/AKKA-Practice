namespace WinTail.Messages.Console
{
    /// <summary>
    /// User provided invalid input (currently, input w/ odd # chars)
    /// </summary>
    sealed class ValidationError : InputError
    {
        public ValidationError(string reason) : base(reason) { }
    }
}
