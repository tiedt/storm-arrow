using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface ICursoRepository{
        public Task<IEnumerable<Curso>> ListAll();
        public Task<Curso> GetById(int Id);

    }
}
