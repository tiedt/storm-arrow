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

    public class DragRaceRepositoryOracle: TableBaseRepositoryOracle, IDragRaceRepository{

        public DragRaceRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        private async Task<int> GetProximoID(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<int>(
                $@"SELECT {TBL_DRAGRACE.ID.SEQUENCE}.NEXTVAL FROM DUAL");
        }

        public async Task<int> GetRaceDragRace(){
            string Sql = $@"SELECT * FROM VW_CORRIDA_ATUAL";
            return await Connection.QueryFirstOrDefaultAsync<int>(Sql);
        }

        public async Task<int> GetSequenceDragRace(int Corrida){
            string Sql = $@"SELECT NVL((SELECT MAX(DR.{TBL_DRAGRACE.SEQUENCIA}) FROM {TBL_DRAGRACE.NAME} DR WHERE DR.{TBL_DRAGRACE.CORRIDA} = :{TBL_DRAGRACE.CORRIDA}), 0)+1 FROM DUAL";
            return await Connection.QueryFirstOrDefaultAsync<int>(Sql, new { Corrida } );
        }

        public Task<bool> Delete(DragRace Model){
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DragRace>> GetAll(){
            throw new NotImplementedException();
        }

        public Task<bool> Update(DragRace Model){
            throw new NotImplementedException();
        }

        public async Task<bool> Insert(DragRace Model){
            Model.Id = await GetProximoID();
            Model.Corrida = await GetRaceDragRace();
            Model.sequencia = await GetSequenceDragRace(Model.Corrida);
            string Sql = $@"INSERT INTO {TBL_DRAGRACE.NAME}
                                    ({TBL_DRAGRACE.ID},
                                     {TBL_DRAGRACE.CORRIDA},
                                     {TBL_DRAGRACE.SEQUENCIA},
                                     {TBL_DRAGRACE.VITCAR1},
                                     {TBL_DRAGRACE.VITCAR2},
                                     {TBL_DRAGRACE.VITCAR3},
                                     {TBL_DRAGRACE.VITCAR4})
                            VALUES ({await GetNextValSequence(TBL_DRAGRACE.ID.SEQUENCE)},
                                    :{TBL_DRAGRACE.CORRIDA},
                                    :{TBL_DRAGRACE.SEQUENCIA},
                                    :{TBL_DRAGRACE.VITCAR1},
                                    :{TBL_DRAGRACE.VITCAR2},
                                    :{TBL_DRAGRACE.VITCAR3},
                                    :{TBL_DRAGRACE.VITCAR4})";
            return await Connection.ExecuteAsync(Sql, new { Model.Corrida, 
                                                            Model.sequencia, 
                                                            Model.VitCar1, 
                                                            Model.VitCar2, 
                                                            Model.VitCar3, 
                                                            Model.VitCar4 }) > 0;
             
        }

    }
}
