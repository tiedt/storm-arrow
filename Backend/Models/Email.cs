
using System.Collections.Generic;
using System.Net.Mail;

namespace SIMP.Classes
{
    public class Email{

        public Email(){
            this.Recipients = new List<string>();
            this.CcEmails = new List<string>();
        }

        public Email(string Body, string Subject, string[] Recipients){
            this.Body = Body;
            this.Subject = Subject;
            this.Recipients = new List<string>(Recipients);
            this.CcEmails = new List<string>();
        }

        public string ServerAddress { get; set; } // Endereço do servidor
        public int Port { get; set; } // Porta do serviço SMTP
        public string Username { get; set; } // Email do usuário
        public string Password { get; set; } // Senha do email do usuário
        public string FromEmail { get; set; } // Remetente
        public List<string> Recipients { get; set; } // Destinatórios
        public string Subject { get; set; } // Asunto
        public string Body { get; set; } // Corpo
        public List<string> CcEmails { get; set; } // Cópias
        public bool IsBodyHtml { get; set; } // Corpo possui codificação em HTML?
        public MailPriority Priority { get; set; } // Prioridade do email
        public bool EnableSsl { get; set; } // Usa SSL?
        
    }
}
