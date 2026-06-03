using System.Net;
using System.Net.Mail;
using GestionSalones.Settings;
using Microsoft.Extensions.Options;

namespace GestionSalones.Services 
{
    public class EmailServices : IEmailService
    {

        private readonly Emailsettings _settings;

        public EmailServices(IOptions<Emailsettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task EnviarAsignacionCreadaAsync(
            string emailDocente,
            string nombreDocente,
            string materia,
            string salon,
            string dia,
            string horaInicio,
            string horaFin)
        {
            var subject = $"📋 Nueva asignación de salón — {materia}";
            var body = ConstruirHtml(nombreDocente, materia, salon, dia, horaInicio, horaFin);

            using var smtp = new SmtpClient(_settings.SmtpHost, _settings.SmtpPort)
            {
                Credentials = new NetworkCredential(_settings.From, _settings.Password),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
            };

            using var mensaje = new MailMessage
            {
                From = new MailAddress(_settings.From, _settings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mensaje.To.Add(emailDocente);

            await smtp.SendMailAsync(mensaje);
        }

        // ── Plantilla HTML del correo ──────────────────────────────────────
        private static string ConstruirHtml(
            string docente, string materia, string salon,
            string dia, string horaInicio, string horaFin) => $"""
            <!DOCTYPE html>
            <html lang="es">
            <head><meta charset="UTF-8"><meta name="viewport" content="width=device-width,initial-scale=1"></head>
            <body style="margin:0;padding:0;background:#f5f4f2;font-family:'Segoe UI',Arial,sans-serif;">
              <table width="100%" cellpadding="0" cellspacing="0" style="background:#f5f4f2;padding:32px 0;">
                <tr><td align="center">
                  <table width="560" cellpadding="0" cellspacing="0" style="background:#ffffff;border-radius:16px;overflow:hidden;box-shadow:0 4px 20px rgba(0,0,0,0.08);">
 
                    <!-- Header naranja -->
                    <tr>
                      <td style="background:#E8611A;padding:28px 36px;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                          <tr>
                            <td>
                              <p style="margin:0;font-size:20px;font-weight:800;color:#ffffff;">Gestión de Salones</p>
                              <p style="margin:4px 0 0;font-size:13px;color:rgba(255,255,255,0.75);">Sistema de asignación académica</p>
                            </td>
                            <td align="right">
                              <div style="width:44px;height:44px;background:rgba(255,255,255,0.2);border-radius:10px;display:inline-flex;align-items:center;justify-content:center;font-size:22px;">📋</div>
                            </td>
                          </tr>
                        </table>
                      </td>
                    </tr>
 
                    <!-- Cuerpo -->
                    <tr>
                      <td style="padding:32px 36px;">
                        <p style="margin:0 0 6px;font-size:22px;font-weight:800;color:#1C1917;">¡Hola, {docente}!</p>
                        <p style="margin:0 0 24px;font-size:14px;color:#6B6860;line-height:1.6;">
                          Se te ha asignado un nuevo salón para uno de tus cursos. Por favor revisa los detalles y acepta o rechaza la asignación desde tu panel.
                        </p>
 
                        <!-- Tarjeta de detalle -->
                        <table width="100%" cellpadding="0" cellspacing="0" style="background:#FFF0E6;border-radius:12px;border:1.5px solid #E8611A25;margin-bottom:24px;">
                          <tr><td style="padding:20px 24px;">
                            <p style="margin:0 0 16px;font-size:12px;font-weight:700;color:#C4511A;text-transform:uppercase;letter-spacing:0.08em;">Detalle de la asignación</p>
                            {FilaDetalle("📖", "Materia", materia)}
                            {FilaDetalle("🏫", "Salón", salon)}
                            {FilaDetalle("📅", "Día", dia)}
                            {FilaDetalle("🕐", "Horario", $"{horaInicio} – {horaFin}")}
                          </td></tr>
                        </table>
 
                        <!-- CTA -->
                        <table width="100%" cellpadding="0" cellspacing="0">
                          <tr>
                            <td align="center">
                              <a href="https://youtu.be/y_U6GGH7MSk?si=mHunbSq7-LBp5sh5" style="display:inline-block;padding:12px 32px;background:#E8611A;color:#ffffff;text-decoration:none;border-radius:10px;font-size:14px;font-weight:700;">
                                Ver mi asignación
                              </a>
                            </td>
                          </tr>
                        </table>
 
                        <p style="margin:24px 0 0;font-size:12px;color:#B0ADA6;text-align:center;line-height:1.6;">
                          Este es un mensaje automático del sistema de Gestión de Salones.<br>
                          Por favor no respondas a este correo.
                        </p>
                      </td>
                    </tr>
 
                    <!-- Footer -->
                    <tr>
                      <td style="background:#F5F4F2;padding:16px 36px;border-top:1px solid #E8E6E1;">
                        <p style="margin:0;font-size:11px;color:#B0ADA6;text-align:center;">
                          © {DateTime.Now.Year} Gestión de Salones · Sistema académico institucional
                        </p>
                      </td>
                    </tr>
 
                  </table>
                </td></tr>
              </table>
            </body>
            </html>
            """;

        private static string FilaDetalle(string icono, string label, string valor) => $"""
            <table width="100%" cellpadding="0" cellspacing="0" style="margin-bottom:10px;">
              <tr>
                <td width="24" style="font-size:15px;vertical-align:middle;">{icono}</td>
                <td style="font-size:12px;color:#6B6860;font-weight:600;width:80px;vertical-align:middle;">{label}</td>
                <td style="font-size:13px;color:#1C1917;font-weight:700;vertical-align:middle;">{valor}</td>
              </tr>
            </table>
            """;
    }
}
