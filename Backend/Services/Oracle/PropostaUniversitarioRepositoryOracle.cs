using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class PropostaUniversitarioRepositoryOracle : TableBaseRepositoryOracle, IPropostaUniversitarioRepository{

        private readonly ICursoRepository cursoRepository;
        private readonly IInstituicaoRepository instituicaoRepository;
        private readonly IAgrupadorArquivoRepository agrupadorArquivoRepository;
        public PropostaUniversitarioRepositoryOracle(ICursoRepository cursoRepository, IInstituicaoRepository instituicaoRepository, IAgrupadorArquivoRepository agrupadorArquivoRepository, IConfiguration configuration) : base(configuration){ 
            this.cursoRepository = cursoRepository;
            this.instituicaoRepository = instituicaoRepository;
            this.agrupadorArquivoRepository = agrupadorArquivoRepository;
        }

        private void CheckModel(PropostaUniversitario Model){
            if(Model == null
            || (Model.Nr_id_proposta <= 0
                && Model.Nr_id_universitario <= 0))
                throw new Exception("Campos obrigatórios não foram informados");
        }

        public async Task<bool> Insert(PropostaUniversitario Model){
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.ExecuteAsync(
                $@"INSERT INTO {TBL_PROPOSTA_UNIVERSITARIO.NAME}
                            ({TBL_PROPOSTA_UNIVERSITARIO.NR_ID},
                            {TBL_PROPOSTA_UNIVERSITARIO.NR_ID_UNIVERSITARIO},
                            {TBL_PROPOSTA_UNIVERSITARIO.NR_ID_PROPOSTA})
                    VALUES ({await GetNextValSequence(TBL_PROPOSTA_UNIVERSITARIO.NR_ID.SEQUENCE)},
                            {Model.Nr_id_universitario},
                            {Model.Nr_id_proposta})") > 0;
        }

        public async Task<bool> Delete(PropostaUniversitario Model){
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            string sql = $@"DELETE FROM {TBL_PROPOSTA_UNIVERSITARIO.NAME} ";
            if(Model.Nr_id_proposta > 0){
                if(sql.Contains("WHERE"))
                    sql += "AND ";
                else 
                    sql += "WHERE ";
                sql += $@"{TBL_PROPOSTA_UNIVERSITARIO.NR_ID_PROPOSTA} = {Model.Nr_id_proposta} ";
            }
            if(Model.Nr_id_universitario > 0){
                if(sql.Contains("WHERE"))
                    sql += "AND ";
                else 
                    sql += "WHERE ";
                sql += $@"{TBL_PROPOSTA_UNIVERSITARIO.NR_ID_UNIVERSITARIO} = {Model.Nr_id_universitario} ";
            }
            return await Connection.ExecuteAsync(sql) > 0;
        }

        public async Task<IEnumerable<Proposta>> ListAllByUniversity(int Id_universitario, string Status){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            string sql = $@"SELECT PO.* FROM {TBL_PROPOSTA_UNIVERSITARIO.NAME} PU, {TBL_PROPOSTA.NAME} PO
                            WHERE PU.{TBL_PROPOSTA_UNIVERSITARIO.NR_ID_PROPOSTA} = PO.{TBL_PROPOSTA.NR_ID}
                                AND PU.{TBL_PROPOSTA_UNIVERSITARIO.NR_ID_UNIVERSITARIO} = {Id_universitario} ";

            if(!string.IsNullOrEmpty(Status))
                sql += $@"AND PO.{TBL_PROPOSTA.CD_STATUS} = '{TBL_PROPOSTA.CD_STATUS}' ";
            IEnumerable<Proposta> Models = await Connection.QueryAsync<Proposta>(sql);
            foreach(Proposta Model in Models){
                Model.Curso = await cursoRepository.GetById(Model.Nr_id_curso);
                Model.Instituicao = await instituicaoRepository.GetById(Model.Nr_id_instituicao);
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
                Model.Universitarios = await this.ListAllByProposals(Model.Nr_id);
            }
            return Models;
        }

        public async Task<IEnumerable<Universitario>> ListAllByProposals(int Id_proposta){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Universitario> Models = await Connection.QueryAsync<Universitario>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          UNI.* FROM {TBL_PROPOSTA_UNIVERSITARIO.NAME} PU, {TBL_UNIVERSITARIO.NAME} UNI, {TBL_USUARIO.NAME} USU
                    WHERE PU.{TBL_PROPOSTA_UNIVERSITARIO.NR_ID_UNIVERSITARIO} = UNI.{TBL_UNIVERSITARIO.NR_ID}
                      AND UNI.{TBL_UNIVERSITARIO.NR_ID_USUARIO} = USU.{TBL_USUARIO.NR_ID}
                      AND PU.{TBL_PROPOSTA_UNIVERSITARIO.NR_ID_PROPOSTA} = {Id_proposta}");
            if(Models.AsList<Universitario>().Count <= 0)
                return null;
            foreach(Universitario Model in Models)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Models;
        }

    }

}
