using Dapper;
using Microsoft.Extensions.Configuration;
using Simp.Models;
using SIMP.Repositories;
using System;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class TableBaseRepositoryOracle : DataConnection, ISequenceRepository{
        
        public TableBaseRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<int> GetCurrValSequence(string SequenceName){
            try{
                return await Connection.QueryFirstOrDefaultAsync<int>(
                    $@"SELECT {SequenceName}.CURRVAL FROM DUAL");
            }catch(Exception){
                return 0;
            }
        }

        public async Task<int> GetNextValSequence(string SequenceName){
            return await Connection.QueryFirstOrDefaultAsync<int>(
                $@"SELECT {SequenceName}.NEXTVAL FROM DUAL");
        }
    }
}
