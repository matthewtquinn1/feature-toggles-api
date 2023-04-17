namespace FeatureToggle.Application.Common.Exceptions;

public sealed class DuplicateFoundException : Exception
{
    public DuplicateFoundException() : base()
    {
    }

    public DuplicateFoundException(string message) : base(message)
    {
    }

    public DuplicateFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DuplicateFoundException(string entityName, string propertyName, object key) 
        : base($"\"{entityName}\" already exists with {propertyName} as ({key}).")
    {
    }
}
