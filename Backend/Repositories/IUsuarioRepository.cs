using Microsoft.AspNetCore.Http;
using SIMP.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Repositories{
    
    public interface IUsuarioRepository{
        public Task<IEnumerable<Object>> ListAll();        
        public Task<Object> Insert(Object Obj);
        public Task<Object> GetById(int Id);
        public Task<Object> GetByEmailPassword(string Email, string Password);
        public Task<Object> Update(Object Obj);
        public Task<IEnumerable<AgrupadorArquivo>> GetImages(int Id);
        public Task<bool> InsertImage(int Id, int Nr_agrupador, List<IFormFile> Files);
        public Task<bool> UpdateImage(int Id_arquivo, IFormFile File);
        public Task<bool> DeleteImage(int Id, int Id_arquivo);
    }

}
