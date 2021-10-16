using Dapper;
using System.Collections.Generic;
using SIMP.Repositories;
using SIMP.Models;
using SIMP.Constants;
using System.Threading.Tasks;
using System;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace SIMP.Services.Oracle{

    public class InstituicaoRepositoryOracle : TableBaseRepositoryOracle, IInstituicaoRepository{

        private readonly IAgrupadorArquivoRepository agrupadorArquivoRepository;

        public InstituicaoRepositoryOracle(IAgrupadorArquivoRepository agrupadorArquivoRepository, IConfiguration configuration) : base(configuration){ 
            this.agrupadorArquivoRepository = agrupadorArquivoRepository;
        }

        private async Task<bool> CheckModel(Instituicao Model){
            if(Model.Nr_id_cidade <= 0 || Model.Nr_id_estado <= 0)
                throw new Exception("Campos obrigatórios não foram informados.");
            if(await Task.Run(() => !ValidatingClass.CNPJ(Model.Cd_cnpj)))
                throw new Exception("CNPJ informado não é válido.");
            return true;
        }

        public async Task<bool> Insert(Instituicao Model){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Model.Nr_id = await GetNextValSequence(TBL_INSTITUICAO.NR_ID.SEQUENCE);
            await CheckModel(Model);
            string Sql = $@"INSERT INTO {TBL_INSTITUICAO.NAME}
                        ({TBL_INSTITUICAO.NR_ID},
                            {TBL_INSTITUICAO.NR_ID_CIDADE},
                            {TBL_INSTITUICAO.NR_ID_ESTADO},
                            {TBL_INSTITUICAO.NR_ID_USUARIO},
                            {TBL_INSTITUICAO.DS_RAZAO_SOCIAL},
                            {TBL_INSTITUICAO.DS_RAMO},
                            {TBL_INSTITUICAO.CD_CNPJ},
                            {TBL_INSTITUICAO.DS_RESUMO},
                            {TBL_INSTITUICAO.DS_DESCRICAO},
                            {TBL_INSTITUICAO.DS_EMAIL_CONTATO},
                            {TBL_INSTITUICAO.DS_TELEFONE},
                            {TBL_INSTITUICAO.DS_HORARIO_FUNCIONAMENTO})
                    VALUES ({Model.Nr_id},
                            {Model.Nr_id_cidade},
                            {Model.Nr_id_estado},
                            {Model.Nr_id_usuario},
                            '{Model.Ds_razao_social}',
                            '{Model.Ds_ramo}',
                            '{Model.Cd_cnpj}',
                            '{Model.Ds_resumo}',
                            '{Model.Ds_descricao}',
                            '{Model.Ds_email_contato}',
                            '{Model.Ds_telefone}',
                            '{Model.Ds_horario_funcionamento}')";
            return await Connection.ExecuteAsync(Sql) > 0;
        }

        public async Task<bool> Update(Instituicao Model){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            await CheckModel(Model);
            string Sql = $@"UPDATE {TBL_INSTITUICAO.NAME} 
                                    SET {TBL_INSTITUICAO.NR_ID_CIDADE} = {Model.Nr_id_cidade},
                                        {TBL_INSTITUICAO.NR_ID_ESTADO} = {Model.Nr_id_estado},
                                        {TBL_INSTITUICAO.NR_ID_USUARIO} = {Model.Nr_id_usuario},
                                        {TBL_INSTITUICAO.DS_RAZAO_SOCIAL} = '{Model.Ds_razao_social}',
                                        {TBL_INSTITUICAO.DS_RAMO} = '{Model.Ds_ramo}',
                                        {TBL_INSTITUICAO.CD_CNPJ} = '{Model.Cd_cnpj}',
                                        {TBL_INSTITUICAO.DS_RESUMO} = '{Model.Ds_resumo}',
                                        {TBL_INSTITUICAO.DS_DESCRICAO} = '{Model.Ds_descricao}',
                                        {TBL_INSTITUICAO.DS_EMAIL_CONTATO} = '{Model.Ds_email_contato}',
                                        {TBL_INSTITUICAO.DS_TELEFONE} = '{Model.Ds_telefone}',
                                        {TBL_INSTITUICAO.DS_HORARIO_FUNCIONAMENTO} = '{Model.Ds_horario_funcionamento}' ";
            if(Model.Nr_id <= 0)
                Sql += $@"WHERE {TBL_INSTITUICAO.NR_ID_USUARIO} = {Model.Nr_id_usuario} ";
            else
                Sql += $@"WHERE {TBL_INSTITUICAO.NR_ID} = {Model.Nr_id} ";
            return await Connection.ExecuteAsync(Sql) > 0;
        }

        public async Task<Instituicao> GetById(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Instituicao Model = await Connection.QueryFirstOrDefaultAsync<Instituicao>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          INS.* FROM {TBL_USUARIO.NAME} USU, 
                                     {TBL_INSTITUICAO.NAME} INS
                        WHERE USU.{TBL_USUARIO.NR_ID} = INS.{TBL_INSTITUICAO.NR_ID_USUARIO}
                          AND INS.{TBL_INSTITUICAO.NR_ID} = {Id}");
            if(Model != null)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Model;
        }

        public async Task<Instituicao> GetByEmailPasswordUser(string Email, string Password){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Instituicao Model = await Connection.QueryFirstOrDefaultAsync<Instituicao>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          INS.* FROM {TBL_USUARIO.NAME} USU, 
                                     {TBL_INSTITUICAO.NAME} INS
                        WHERE USU.{TBL_USUARIO.NR_ID} = INS.{TBL_INSTITUICAO.NR_ID_USUARIO}
                          AND USU.{TBL_USUARIO.DS_EMAIL} = '{Email}'
                          AND USU.{TBL_USUARIO.DS_SENHA} = '{Hash.EncryptStringSalt(Password, Email)}'");
            if(Model != null)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Model;
        }

        public async Task<Instituicao> GetByIdUser(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Instituicao Model = await Connection.QueryFirstOrDefaultAsync<Instituicao>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          INS.* FROM {TBL_USUARIO.NAME} USU, 
                                     {TBL_INSTITUICAO.NAME} INS
                        WHERE USU.{TBL_USUARIO.NR_ID} = INS.{TBL_INSTITUICAO.NR_ID_USUARIO}
                          AND USu.{TBL_USUARIO.NR_ID} = {Id}");
            if(Model != null)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Model;
        }

        public async Task<IEnumerable<Instituicao>> ListAll(string Ds_ramo = ""){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Instituicao> Models;
            if(!string.IsNullOrEmpty(Ds_ramo))
                Models = await Connection.QueryAsync<Instituicao>(
                    $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                              USU.{TBL_USUARIO.DS_SENHA},
                              USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                              USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                              INS.* FROM {TBL_USUARIO.NAME} USU, 
                                         {TBL_INSTITUICAO.NAME} INS
                            WHERE USU.{TBL_USUARIO.NR_ID} = INS.{TBL_INSTITUICAO.NR_ID_USUARIO}
                              AND INS.{TBL_INSTITUICAO.DS_RAMO} LIKE '%{Ds_ramo}%'");
            else
                Models = await Connection.QueryAsync<Instituicao>(
                    $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                              USU.{TBL_USUARIO.DS_SENHA},
                              USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                              USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                              INS.* FROM {TBL_USUARIO.NAME} USU,
                                         {TBL_INSTITUICAO.NAME} INS
                            WHERE USU.{TBL_USUARIO.NR_ID} = INS.{TBL_INSTITUICAO.NR_ID_USUARIO}");
            foreach(Instituicao Model in Models)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Models;
        }
    }
}
