using System;
using System.Collections.Generic;

namespace Clarity.Core.Utilities 
{ 
    public static class EmailSender 
    { 
        public static void Send(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentNullException(nameof(to));
            
            // In a real app, this would use SmtpClient or SendGrid
            Logger.LogInfo($"Sending email to {to}: {subject}");
            Logger.LogInfo($"Body length: {body?.Length ?? 0} chars");
        }
        
        public static void SendTemplate(string to, string templateName, Dictionary<string, string> parameters)
        {
             Logger.LogInfo($"Sending template {templateName} to {to}");
             
             string body = $"[Template: {templateName}]\n";
             if (parameters != null)
             {
                 foreach(var param in parameters)
                 {
                     body += $"{param.Key}: {param.Value}\n";
                 }
             }
             
             Send(to, $"Template: {templateName}", body);
        }
    } 
}
