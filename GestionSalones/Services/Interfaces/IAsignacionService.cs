using GestionSalones.DTOs;

namespace GestionSalones.Services.Interfaces
{
    public interface IAsignacionService
    {
        Task<IEnumerable<AsignacionListDTO>> GetAll();
        Task<AsignacionDetalleDTO?> GetById(int id);
        Task<object> AsignacionAutomatica(AsignacionAutomaticaDTO dto);
        Task<object> AsignacionManual(AsignacionManualDTO dto);
        Task<bool> Cancelar(int id);
    }
}
