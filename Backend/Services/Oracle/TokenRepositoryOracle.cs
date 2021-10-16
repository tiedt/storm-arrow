using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{
    
    public class TokenRepositoryOracle : TableBaseRepositoryOracle, ITokenRepository{

        public TokenRepositoryOracle(IConfiguration configuration) : base(configuration){ }

        private async Task<string> GetNewGuid(){
            return await Task.Run(() => Guid.NewGuid().ToString());
        }

        public async Task<Token> GetById(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<Token>(
                $@"SELECT * FROM {TBL_TOKEN.NAME}
                        WHERE {TBL_TOKEN.NR_ID} = {Id}");
        }

        public async Task<Token> GetByToken(string Token){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<Token>(
                $@"SELECT * FROM {TBL_TOKEN.NAME}
                        WHERE {TBL_TOKEN.DS_TOKEN} = '{Token}'");
        }

        public async Task<Token> Insert(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Token model = new Token(){ Nr_id = await GetNextValSequence(TBL_TOKEN.NR_ID.SEQUENCE), 
                            Ds_token = await GetNewGuid(), Dt_data_limite = DateTime.Now.AddHours(1)};
            string Sql = $@"INSERT INTO {TBL_TOKEN.NAME}
                                        ({TBL_TOKEN.NR_ID},
                                        {TBL_TOKEN.DS_TOKEN},
                                        {TBL_TOKEN.DT_DATA_LIMITE})
                                VALUES (:{TBL_TOKEN.NR_ID},
                                        :{TBL_TOKEN.DS_TOKEN},
                                        :{TBL_TOKEN.DT_DATA_LIMITE})";
            await Connection.ExecuteAsync(Sql, new { model.Nr_id, model.Ds_token, model.Dt_data_limite });
            return model;
        }
    }
}
