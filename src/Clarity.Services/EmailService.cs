using Clarity.Core.Models;
namespace Clarity.Services {
    public class EmailService {
        public void SendWelcomeEmail(string email) {
            if (string.IsNullOrWhiteSpace(email)) return;
            
            string subject = "Welcome to Clarity CRM";
            string body = "Dear User,\n\nWelcome to Clarity CRM. We are excited to have you on board.";
            
            Clarity.Core.Utilities.EmailSender.Send(email, subject, body);
            Clarity.Core.Utilities.Logger.LogInfo($"Welcome email sent to {email}");
        }
    }
}
