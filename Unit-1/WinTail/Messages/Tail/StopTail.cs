namespace WinTail.Messages.Tail
{
    /// <summary>
    /// Stop tailing the file at user-specified path.
    /// </summary>
    sealed class StopTail
    {
        public StopTail(string filePath)
        {
            FilePath = filePath;
        }

        public string FilePath { get; }
    }
}
