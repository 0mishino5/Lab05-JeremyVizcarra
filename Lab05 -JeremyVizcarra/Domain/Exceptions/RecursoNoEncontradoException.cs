namespace Lab05_JeremyVizcarra.Domain.Exceptions;

public class RecursoNoEncontradoException : Exception
{
    public RecursoNoEncontradoException(string mensaje) : base(mensaje)
    {
    }
}
