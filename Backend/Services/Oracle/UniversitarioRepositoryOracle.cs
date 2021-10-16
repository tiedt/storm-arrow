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

    public class UniversitarioRepositoryOracle : TableBaseRepositoryOracle, IUniversitarioRepository{

        private readonly IAgrupadorArquivoRepository agrupadorArquivoRepository;
        public UniversitarioRepositoryOracle(IConfiguration configuration, IAgrupadorArquivoRepository agrupadorArquivoRepository) : base(configuration){ 
            this.agrupadorArquivoRepository = agrupadorArquivoRepository;
        }

        private void CheckModel(Universitario Model){
            if(Model.Nr_id_instituicao <= 0 || Model.Nr_id_cidade <= 0 || Model.Nr_id_estado <= 0)
                throw new Exception("Campos obrigatórios não foram informados.");
        }
        
        public async Task<bool> Insert(Universitario Model){
            Model.Nr_id = await GetNextValSequence(TBL_UNIVERSITARIO.NR_ID.SEQUENCE);
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.ExecuteAsync(
                $@"INSERT INTO {TBL_UNIVERSITARIO.NAME}
                            ({TBL_UNIVERSITARIO.NR_ID},
                            {TBL_UNIVERSITARIO.NR_ID_USUARIO},
                            {TBL_UNIVERSITARIO.NR_ID_INSTITUICAO},
                            {TBL_UNIVERSITARIO.NR_ID_CIDADE},
                            {TBL_UNIVERSITARIO.NR_ID_ESTADO},
                            {TBL_UNIVERSITARIO.DS_NOME},
                            {TBL_UNIVERSITARIO.DS_SOBRENOME},
                            {TBL_UNIVERSITARIO.DS_TELEFONE},
                            {TBL_UNIVERSITARIO.DS_GRAU})
                    VALUES ({Model.Nr_id},
                            {Model.Nr_id_usuario},
                            {Model.Nr_id_instituicao},
                            {Model.Nr_id_cidade},
                            {Model.Nr_id_estado},
                            '{Model.Ds_nome}',
                            '{Model.Ds_sobrenome}',
                            '{Model.Ds_telefone}',
                            '{Model.Ds_grau}')") > 0;
        }

        public async Task<Universitario> GetById(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Universitario Model = await Connection.QueryFirstOrDefaultAsync<Universitario>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          UNI.* FROM {TBL_USUARIO.NAME} USU, {TBL_UNIVERSITARIO.NAME} UNI
                    WHERE USU.{TBL_USUARIO.NR_ID} = UNI.{TBL_UNIVERSITARIO.NR_ID_USUARIO}
                      AND UNI.{TBL_UNIVERSITARIO.NR_ID} = {Id}");
            if(Model != null)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Model;
        }

        public async Task<Universitario> GetByEmailPasswordUser(string Email, string Password){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Universitario Model = await Connection.QueryFirstOrDefaultAsync<Universitario>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          UNI.* FROM {TBL_USUARIO.NAME} USU, {TBL_UNIVERSITARIO.NAME} UNI
                    WHERE USU.{TBL_USUARIO.NR_ID} = UNI.{TBL_UNIVERSITARIO.NR_ID_USUARIO}
                      AND USU.{TBL_USUARIO.DS_EMAIL} = '{Email}'
                      AND USU.{TBL_USUARIO.DS_SENHA} = '{Hash.EncryptStringSalt(Password, Email)}'");
            if(Model != null)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Model;
        }

        public async Task<Universitario> GetByIdUser(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Universitario Model = await Connection.QueryFirstOrDefaultAsync<Universitario>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          UNI.* FROM {TBL_USUARIO.NAME} USU, {TBL_UNIVERSITARIO.NAME} UNI
                    WHERE USU.{TBL_USUARIO.NR_ID} = UNI.{TBL_UNIVERSITARIO.NR_ID_USUARIO}
                      AND USU.{TBL_USUARIO.NR_ID} = {Id}");
            if(Model != null)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Model;
        }

        public async Task<IEnumerable<Universitario>> ListAll(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            IEnumerable<Universitario> Models = await Connection.QueryAsync<Universitario>(
                $@"SELECT USU.{TBL_USUARIO.DS_EMAIL},
                          USU.{TBL_USUARIO.DS_SENHA},
                          USU.{TBL_USUARIO.DS_NOME_EXIBIDO},
                          USU.{TBL_USUARIO.NR_AGRUPADOR_ARQUIVO},
                          UNI.* FROM {TBL_USUARIO.NAME} USU, {TBL_UNIVERSITARIO.NAME} UNI
                    WHERE USU.{TBL_USUARIO.NR_ID} = UNI.{TBL_UNIVERSITARIO.NR_ID_USUARIO}");
            foreach(Universitario Model in Models)
                Model.AgrupadorArquivo = await agrupadorArquivoRepository.ListAllByAgrupador(Model.Nr_agrupador_arquivo);
            return Models;
        }

        public async Task<bool> Update(Universitario Model){
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            string Sql = $@"UPDATE {TBL_UNIVERSITARIO.NAME}
                        SET {TBL_UNIVERSITARIO.NR_ID_CIDADE} = {Model.Nr_id_cidade},
                            {TBL_UNIVERSITARIO.NR_ID_INSTITUICAO} = {Model.Nr_id_instituicao},
                            {TBL_UNIVERSITARIO.NR_ID_ESTADO} = {Model.Nr_id_estado},
                            {TBL_UNIVERSITARIO.DS_NOME} = '{Model.Ds_nome}',
                            {TBL_UNIVERSITARIO.DS_SOBRENOME} = '{Model.Ds_sobrenome}',
                            {TBL_UNIVERSITARIO.DS_TELEFONE} = '{Model.Ds_telefone}',
                            {TBL_UNIVERSITARIO.DS_GRAU} = '{Model.Ds_grau}' ";
            if(Model.Nr_id <= 0)
                Sql += $@"WHERE {TBL_UNIVERSITARIO.NR_ID_USUARIO} = {Model.Nr_id_usuario}";
            else
                Sql += $@"WHERE {TBL_UNIVERSITARIO.NR_ID} = {Model.Nr_id}";
            return await Connection.ExecuteAsync(Sql) > 0;
        }

    }
}
