using Microsoft.AspNetCore.Http;
using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IAgrupadorArquivoRepository{

        public Task<int> Insert(int Agrupador_arquivo, List<IFormFile> Files); // retorna o agrupador gerado
        public Task<bool> Update(int Id_arquivo, IFormFile File);
        public Task<bool> DeleteByFile(int Id_arquivo);
        public Task<bool> DeleteByAgrupador(int Id_arquivo);
        public Task<IEnumerable<AgrupadorArquivo>> ListAllByAgrupador(int Agrupador_arquivo);
        public Task<List<Microsoft.AspNetCore.Http.IFormFile>> ListAllFilesByAgrupador(int Agrupador_arquivo);
        public Task<AgrupadorArquivo> GetByIdArquivo(int Id_arquivo);

    }

}
