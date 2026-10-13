namespace Lab05_JeremyVizcarra.Domain.Exceptions;

public class CredencialesInvalidasException : Exception
{
    public CredencialesInvalidasException(string mensaje) : base(mensaje)
    {
    }
}
