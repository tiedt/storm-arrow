using SIMP.Models;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IDragRaceRepository{ 
        
        public Task<bool> Insert(DragRace Model);

    }

}
