using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class PaisRepositoryOracle : TableBaseRepositoryOracle, IPaisRepository{

        public PaisRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<Pais> GetById(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<Pais>(
                @$"SELECT * FROM {TBL_PAIS.NAME}
                    WHERE {TBL_PAIS.NR_ID} = {Id}");
        }

        public async Task<IEnumerable<Pais>> ListAll(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryAsync<Pais>(
                @$"SELECT * FROM {TBL_PAIS.NAME}
                ORDER BY {TBL_PAIS.DS_NOME_PT}");
        }

    }
}
