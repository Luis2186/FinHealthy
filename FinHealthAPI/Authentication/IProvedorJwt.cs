using Dominio.Usuarios;

namespace FinHealthAPI.Authentication
{
    public interface IProvedorJwt
    {
        string Generate(Usuario usuario);
    }
}
