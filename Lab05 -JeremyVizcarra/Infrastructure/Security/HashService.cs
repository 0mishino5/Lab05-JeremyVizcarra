using System.Security.Cryptography;
using Lab05_JeremyVizcarra.Application.Ports.Out;

namespace Lab05_JeremyVizcarra.Infrastructure.Security;

public class HashService : IHashService
{
    private const int Iteraciones = 100000;
    private const int TamanoSal = 16;
    private const int TamanoHash = 32;

    public string Generar(string contrasena)
    {
        byte[] sal = RandomNumberGenerator.GetBytes(TamanoSal);

        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, Iteraciones, HashAlgorithmName.SHA256, TamanoHash);

        return $"{Iteraciones}.{Convert.ToBase64String(sal)}.{Convert.ToBase64String(hash)}";
    }

    public bool Verificar(string contrasena, string hashGuardado)
    {
        string[] partes = hashGuardado.Split('.');

        if (partes.Length != 3)
        {
            return false;
        }

        int iteraciones = int.Parse(partes[0]);
        byte[] sal = Convert.FromBase64String(partes[1]);
        byte[] esperado = Convert.FromBase64String(partes[2]);

        byte[] calculado = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, iteraciones, HashAlgorithmName.SHA256, esperado.Length);

        return CryptographicOperations.FixedTimeEquals(calculado, esperado);
    }
}
