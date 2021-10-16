using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IInstituicaoRepository {
        public Task<IEnumerable<Instituicao>> ListAll(string Ds_ramo = "");
        public Task<bool> Insert(Instituicao Model);
        public Task<Instituicao> GetById(int Id);
        public Task<Instituicao> GetByIdUser(int Id);
        public Task<Instituicao> GetByEmailPasswordUser(string Email, string Password);
        public Task<bool> Update(Instituicao Model);

    }
}
