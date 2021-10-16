using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IServidorEmailRepository{
        public Task<IEnumerable<ServidorEmail>> ListAll();
        public Task<ServidorEmail> GetById(int Id);
        public Task<ServidorEmail> GetByName(string Nome);
        public Task<bool> Insert(ServidorEmail Model);

    }
}
