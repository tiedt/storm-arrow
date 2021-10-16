using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{
    
    public class ThingRepositoryOracle: TableBaseRepositoryOracle, IThingRepository{

        public ThingRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<int> ListAll(string Nome){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<int>(
                $@"SELECT COUNT(*) FROM {TBL_THING.NAME}
                        WHERE TRIM({TBL_THING.NOME}) = '{Nome.Trim()}'");
        }

        private void CheckModel(Thing Model){
            if(String.IsNullOrEmpty(Model.Nome))
                throw new Exception("Nome inválido");
        }
        
        public async Task<bool> Insert(Thing Model){
            CheckModel(Model);
            if(Connection.State != ConnectionState.Open)
                Connection.Open();

            string Sql = $@"INSERT INTO {TBL_THING.NAME} 
                        ({TBL_THING.ID},
                            {TBL_THING.NOME},
                            {TBL_THING.TEMPERATURA},
                            {TBL_THING.UMIDADE},
                            {TBL_THING.LUZ_DIA},
                            {TBL_THING.MILI_CHUVA},
                            {TBL_THING.BAROMETRO},
                            {TBL_THING.PRESSAO},
                            {TBL_THING.VELOCIDADE_VENTO},
                            {TBL_THING.DIRECAO_VENTO},
                            {TBL_THING.DATA}) 
                    VALUES ({await GetNextValSequence(TBL_THING.ID.SEQUENCE)},
                            :{TBL_THING.NOME},
                            :{TBL_THING.TEMPERATURA},
                            :{TBL_THING.UMIDADE},
                            :{TBL_THING.LUZ_DIA},
                            :{TBL_THING.MILI_CHUVA},
                            :{TBL_THING.BAROMETRO},
                            :{TBL_THING.PRESSAO},
                            :{TBL_THING.VELOCIDADE_VENTO},
                            :{TBL_THING.DIRECAO_VENTO},
                            SYSDATE)";
            return await Connection.ExecuteAsync(Sql, new { Model.Nome, Model.Temperatura, Model.Umidade,
                Model.Luz_Dia, Model.Mili_Chuva, Model.Barometro, Model.Pressao, 
                Model.Velocidade_Vento, Model.Direcao_Vento }) > 0;
        }


    }
}
