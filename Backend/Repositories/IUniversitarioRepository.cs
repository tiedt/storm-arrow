using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IUniversitarioRepository{
        public Task<IEnumerable<Universitario>> ListAll();
        public Task<bool> Insert(Universitario Model);
        public Task<Universitario> GetById(int Id);
        public Task<Universitario> GetByIdUser(int Id);
        public Task<Universitario> GetByEmailPasswordUser(string Email, string Password);
        public Task<bool> Update(Universitario Model);
    }
}
