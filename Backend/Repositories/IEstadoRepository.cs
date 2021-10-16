using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IEstadoRepository{
        public Task<IEnumerable<Estado>> ListAll(int? Id_pais);
        public Task<Estado> GetById(int Id);

    }

}
