using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IPropostaUniversitarioRepository{
        public Task<bool> Insert(PropostaUniversitario Model);
        public Task<bool> Delete(PropostaUniversitario Model);
        public Task<IEnumerable<Proposta>> ListAllByUniversity(int Id_universitario, string Status);
        public Task<IEnumerable<Universitario>> ListAllByProposals(int Id_proposta);

    }
}
