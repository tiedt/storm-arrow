using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IInteresseRepository{
        public Task<IEnumerable<Universitario>> ListAllByProposals(int Id_proposta);
        public Task<bool> Insert(Interesse Model);
        public Task<bool> Delete(Interesse Model);
        public Task<bool> AcceptInterest(Interesse Model);
        public Task<IEnumerable<Proposta>> ListAllByUniversity(int Id_universitario);
        public Task<Interesse> GetByProposalsUniversity(int Id_proposta, int Id_usuario);

    }
}
