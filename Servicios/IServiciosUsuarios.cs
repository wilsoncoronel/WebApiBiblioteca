using Microsoft.AspNetCore.Identity;

namespace HolaMundoWebAPI.Servicios
{
    public interface IServiciosUsuarios
    {
        Task<IdentityUser?> ObtenerUsuario();
    }
}