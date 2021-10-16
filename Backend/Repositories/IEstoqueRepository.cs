using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IEstoqueRepository{

        public Task<bool> Insert(Estoque model);

        public Task<bool> Update(Estoque model);

        public Task<IEnumerable<Estoque>> GetAllByProduto(int IdProduto);

        public Task<IEnumerable<Estoque>> GetAll();


    }
}
