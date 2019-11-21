namespace WinTail.Messages.Tail
{
    /// <summary>
    /// Signal that the OS had an error accessing the file.
    /// </summary>
    sealed class FileError
    {
        public FileError(string fileName, string reason)
        {
            FileName = fileName;
            Reason = reason;
        }

        public string FileName { get; }

        public string Reason { get; }
    }
}
