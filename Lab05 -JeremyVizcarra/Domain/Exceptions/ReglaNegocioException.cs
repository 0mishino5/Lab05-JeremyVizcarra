namespace Lab05_JeremyVizcarra.Domain.Exceptions;

public class ReglaNegocioException : Exception
{
    public ReglaNegocioException(string mensaje) : base(mensaje)
    {
    }
}
