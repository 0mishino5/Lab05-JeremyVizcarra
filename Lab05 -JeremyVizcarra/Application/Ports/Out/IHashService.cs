namespace Lab05_JeremyVizcarra.Application.Ports.Out;

public interface IHashService
{
    string Generar(string contrasena);
    bool Verificar(string contrasena, string hashGuardado);
}
