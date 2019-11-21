namespace WinTail.Messages.Tail
{
    /// <summary>
    /// Signal to read the initial contents of the file at actor startup.
    /// </summary>
    sealed class InitialRead
    {
        public InitialRead(string fileName, string text)
        {
            FileName = fileName;
            Text = text;
        }

        public string FileName { get; }
        public string Text { get; }
    }
}
