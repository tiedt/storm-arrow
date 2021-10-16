using SIMP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace SIMP.Repositories{

    public interface IPropostaRepository{
        public Task<IEnumerable<Proposta>>ListAll();
        public Task<Proposta> Insert(Proposta Model, List<IFormFile> Files);
        public Task<Proposta> GetById(int Id, int? Id_usuario);
        public Task<Proposta> Update(Proposta Model, List<IFormFile> Files);
        public Task<IEnumerable<Proposta>> GetMostAccessed();
        public Task<IEnumerable<Proposta>> GetRecommendations(int Id_usuario);
        public Task<IEnumerable<Proposta>> GetNewProposals();
        public Task<IEnumerable<Proposta>> GetByCategory(int Id_categoria);
        public Task<IEnumerable<Proposta>> GetByInstitution(int Id_instituicao);
        public Task<IEnumerable<Proposta>> GetByParams(DateTime? Dt_geracaoIni, DateTime? Dt_geracaoFim, string? Ds_tipo, int? Id_curso, int? Qt_participantes, string? Ds_nome_ds_desc_projeto);
        public Task<IEnumerable<AgrupadorArquivo>> GetImages(int Id);
        public Task<bool> InsertImage(int Id, int Nr_agrupador, List<IFormFile> Files);
        public Task<bool> UpdateImage(int Id_arquivo, IFormFile File);
        public Task<bool> DeleteImage(int Id, int Id_arquivo);

    }
}
