using SIMP.Models;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IThingRepository{        
        public Task<int> ListAll(string Nome);
        public Task<bool> Insert(Thing Model);

    }

}
