using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface ISequenceRepository{
        
        public Task<int> GetCurrValSequence(string SequenceName);
        public Task<int> GetNextValSequence(string SequenceName);
    }
}
