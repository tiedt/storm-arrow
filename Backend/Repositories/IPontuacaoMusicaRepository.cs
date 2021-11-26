using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories {

    public interface IPontuacaoMusicaRepository {

        public Task<bool> Insert(PontuacaoMusica model);

        public Task<bool> Update(PontuacaoMusica model);
        
        public Task<bool> Delete(int id);

        public Task<IEnumerable<PontuacaoMusica>> ListAll();

        public Task<IEnumerable<PontuacaoMusica>> ListAllByProfile(int idPerfil);


    }
}
