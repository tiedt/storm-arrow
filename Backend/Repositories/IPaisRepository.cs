using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IPaisRepository{
        public Task<IEnumerable<Pais>> ListAll();
        public Task<Pais> GetById(int Id);

    }

}
