namespace Eksdom.Integration.EskomSePush;

[Serializable]
public class EksdomException : Exception
{
    public ExceptionTypes ExceptionType { get; init; }

    public EksdomException()
    {
    }

    public EksdomException(string message)
        : base(message)
    {
    }

    public EksdomException(string message, ExceptionTypes exceptionType)
    {
        ExceptionType = exceptionType;
    }

    public EksdomException(string message, Exception inner)
        : base(message, inner)
    {
    }

    public EksdomException(string message, Exception inner, ExceptionTypes exceptionType)
        : base(message, inner)
    {
        ExceptionType = exceptionType;
    }
}
