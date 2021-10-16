using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IProdutoRepository{

        public Task<bool> Insert(Produto model);

        public Task<bool> Update(Produto model);

        public Task<bool> Delete(int Id);

        public Task<IEnumerable<Produto>> GetAll();

    }
}
