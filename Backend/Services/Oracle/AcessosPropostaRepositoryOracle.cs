using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class AcessosPropostaRepositoryOracle : TableBaseRepositoryOracle, IAcessosPropostaRepository{
        
        private readonly ICursoRepository cursoRepository;
        private readonly IInstituicaoRepository instituicaoRepository;
        private readonly IAgrupadorArquivoRepository agrupadorArquivoRepository;
        private readonly IPropostaUniversitarioRepository propostaUniversitarioRepository;

        public AcessosPropostaRepositoryOracle(ICursoRepository cursoRepository, IInstituicaoRepository instituicaoRepository, IAgrupadorArquivoRepository agrupadorArquivoRepository, IPropostaUniversitarioRepository propostaUniversitarioRepository, IConfiguration configuration) : base(configuration) { 
            this.cursoRepository = cursoRepository;
            this.instituicaoRepository = instituicaoRepository;
            this.agrupadorArquivoRepository = agrupadorArquivoRepository;
            this.propostaUniversitarioRepository = propostaUniversitarioRepository;           
        }

        public async Task<AcessosProposta> GetById(int id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            AcessosProposta model = await Connection.QueryFirstOrDefaultAsync<AcessosProposta>(
                $@"SELECT PRO.*, NVL((SELECT COUNT(*) FROM {TBL_LOG_ACESSO.NAME} LA
                                       WHERE LA.{TBL_LOG_ACESSO.NR_ID_PROPOSTA} = PRO.{TBL_PROPOSTA.NR_ID}
                                     GROUP BY LA.{TBL_LOG_ACESSO.NR_ID_PROPOSTA}),0) QT_ACESSOS FROM {TBL_PROPOSTA.NAME} PRO
                    WHERE {TBL_PROPOSTA.NR_ID} = {id}");
            if(model != null){
                model.Curso = await cursoRepository.GetById(model.Nr_id_curso);
                model.Instituicao = await instituicaoRepository.GetById(model.Nr_id_instituicao);
                model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(model.Nr_agrupador_arquivo);
                model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(model.Nr_id);
            }
            return model;
        }

        public async Task<IEnumerable<AcessosProposta>> ListAll(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<AcessosProposta> models = await Connection.QueryAsync<AcessosProposta>(
                $@"SELECT PRO.*, NVL((SELECT COUNT(*) FROM {TBL_LOG_ACESSO.NAME} LA
                                       WHERE LA.{TBL_LOG_ACESSO.NR_ID_PROPOSTA} = PRO.{TBL_PROPOSTA.NR_ID}
                                     GROUP BY LA.{TBL_LOG_ACESSO.NR_ID_PROPOSTA}),0) QT_ACESSOS FROM {TBL_PROPOSTA.NAME} PRO");            
            foreach(AcessosProposta model in models){
                model.Curso = await cursoRepository.GetById(model.Nr_id_curso);
                model.Instituicao = await instituicaoRepository.GetById(model.Nr_id_instituicao);
                model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(model.Nr_agrupador_arquivo);
                model.Universitarios = await propostaUniversitarioRepository.ListAllByProposals(model.Nr_id);
            }
            return models;
        }

    }

}
