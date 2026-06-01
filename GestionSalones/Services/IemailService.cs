namespace GestionSalones.Services
{
    public interface IEmailService
    {
        Task EnviarAsignacionCreadaAsync(
            string emailDocente,
            string nombreDocente,
            string materia,
            string salon,
            string dia,
            string horaInicio,
            string horaFin
        );

    }
}
