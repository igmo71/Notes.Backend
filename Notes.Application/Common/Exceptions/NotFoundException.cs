namespace Notes.Application.Common.Exceptions
{
    internal class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) not found.")
        { }
    }
}
