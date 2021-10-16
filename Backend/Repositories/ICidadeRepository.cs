using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface ICidadeRepository{
        public Task<IEnumerable<Cidade>> ListAll(int? Id_estado);
        public Task<Cidade> GetById(int Id);

    }

}
