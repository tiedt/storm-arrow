using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories {
    
    public interface IPerfilConfiguracoesRepository {

        public Task<bool> Insert(PerfilConfiguracoes model);

        public Task<bool> Update(PerfilConfiguracoes model);

        public Task<bool> Delete(int IdPerfil);

        public Task<IEnumerable<PerfilConfiguracoes>> ListByPerfilConfig(int IdPerfil);

    }

}
