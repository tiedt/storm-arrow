using Microsoft.AspNetCore.Http;
using SIMP.Models;
using System.Threading.Tasks;

namespace SIMP.Repositories{

    public interface IArquivoRepository{
        public Task<Arquivo> GetById(int Id);
        public Task<int> Insert(IFormFile File);
        public Task<bool> Update(int Id, IFormFile File);
        public Task<bool> Delete(int Id);

    }

}
