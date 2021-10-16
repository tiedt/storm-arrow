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
    
    public class ServidorEmailRepositoryOracle : TableBaseRepositoryOracle, IServidorEmailRepository{

        public ServidorEmailRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<ServidorEmail> GetById(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<ServidorEmail>(
                $@"SELECT * FROM {TBL_SERVIDOR_EMAIL.NAME}
                        WHERE {TBL_SERVIDOR_EMAIL.NR_ID} = {Id}");
        }

        public async Task<ServidorEmail> GetByName(string Nome){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<ServidorEmail>(
                $@"SELECT * FROM {TBL_SERVIDOR_EMAIL.NAME}
                        WHERE {TBL_SERVIDOR_EMAIL.DS_NOME} = '{Nome}'");
        }

        private void CheckModel(ServidorEmail Model){
            if(Model == null
            || Model.Nr_id <= 0
            || String.IsNullOrEmpty(Model.Ds_nome)
            || String.IsNullOrEmpty(Model.Ds_endereco_smtp)
            || Model.Nr_porta <= 0
            || Model.Nr_usa_ssl < 0
            || Model.Nr_usa_ssl > 1)
                throw new Exception("Campos obrigatórios não foram informados");
        }

        public async Task<bool> Insert(ServidorEmail Model){
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Model.Nr_id = await GetNextValSequence(TBL_SERVIDOR_EMAIL.NR_ID.SEQUENCE);
            return await Connection.ExecuteAsync(
                $@"INSERT INTO {TBL_SERVIDOR_EMAIL.NAME}
                            ({TBL_SERVIDOR_EMAIL.NR_ID},
                                {TBL_SERVIDOR_EMAIL.DS_NOME}
                                {TBL_SERVIDOR_EMAIL.DS_ENDERECO_SMTP},
                                {TBL_SERVIDOR_EMAIL.NR_PORTA},
                                {TBL_SERVIDOR_EMAIL.NR_USA_SSL})
                    VALUES ({Model.Nr_id},
                            '{Model.Ds_nome}',
                            '{Model.Ds_endereco_smtp}',
                            {Model.Nr_porta},
                            {Model.Nr_usa_ssl})") > 0;
        }

        public async Task<IEnumerable<ServidorEmail>> ListAll(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryAsync<ServidorEmail>(
                $@"SELECT * FROM {TBL_SERVIDOR_EMAIL.NAME}");
        }
    }
}
