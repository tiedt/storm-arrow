using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories {
    
    public interface IPerfilRepository {

        public Task<bool> Insert(Perfil model);

        public Task<bool> Update(Perfil model);

        public Task<bool> Delete(int id);

        public Task<IEnumerable<Perfil>> ListaAll();

        public Task<IEnumerable<Perfil>> ListAllMacAddress(string endereco_mac);

        public Task<Perfil> GetByMacAddrressName(string endereco_mac, string nome);

    }

}
