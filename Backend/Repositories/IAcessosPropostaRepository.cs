using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IAcessosPropostaRepository{

        public Task<IEnumerable<AcessosProposta>> ListAll();

        public Task<AcessosProposta> GetById(int Id);

    }

}
