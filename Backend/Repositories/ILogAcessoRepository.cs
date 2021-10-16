using SIMP.Models;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface ILogAcessoRepository{
        public Task<bool> Insert(LogAcesso Model);

    }

}
