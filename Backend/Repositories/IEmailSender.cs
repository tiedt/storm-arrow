using SIMP.Classes;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IEmailSender{

        public Task<bool> SendEmail(string Service, Email Email);

    }
}
