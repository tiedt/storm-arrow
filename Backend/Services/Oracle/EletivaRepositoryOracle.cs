using SIMP.Models;
using SIMP.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SIMP.Constants;
using Microsoft.Extensions.Configuration;

namespace SIMP.Services.Oracle {
    public class EletivaRepositoryOracle : TableBaseRepositoryOracle, IEletivaRepository {

        public EletivaRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<int> Insert(Eletiva model) {
            string Sql = $@"INSERT INTO {TBL_ELETIVA.NAME} 
                                        ({TBL_ELETIVA.ID},
                                         {TBL_ELETIVA.POTENCIOMETRO}) 
                                VALUES (:{TBL_ELETIVA.ID},
                                        :{TBL_ELETIVA.POTENCIOMETRO})";
            await Connection.ExecuteAsync(Sql, new { Id = await GetNextValSequence(TBL_ELETIVA.ID.SEQUENCE), Potenciometro = model.Potenciometro });
            return await GetCountRecords();
        }

        public async Task<int> GetCountRecords() {
            string Sql = $@"SELECT COUNT(*) 
                              FROM {TBL_ELETIVA.NAME}";
            return await Connection.QueryFirstOrDefaultAsync<int>(Sql);
        }
    }
}
