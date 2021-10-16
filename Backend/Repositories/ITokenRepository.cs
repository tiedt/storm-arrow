using SIMP.Models;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface ITokenRepository{

        public Task<Token> GetById(int Id);
        public Task<Token> GetByToken(string Token);
        public Task<Token> Insert();
    }
}
