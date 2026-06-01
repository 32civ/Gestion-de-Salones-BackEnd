namespace GestionSalones.Settings
{
    public class Emailsettings
    {
        public string From { get; set; } = string.Empty; // tu correo Gmail
        public string Password { get; set; } = string.Empty; // App Password de Gmail
        public string SmtpHost { get; set; } = "smtp.gmail.com";
        public int SmtpPort { get; set; } = 587;
        public string FromName { get; set; } = "Gestión Salones";
    }
}
