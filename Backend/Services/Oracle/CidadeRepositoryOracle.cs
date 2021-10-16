using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class CidadeRepositoryOracle : TableBaseRepositoryOracle, ICidadeRepository{

        public CidadeRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<Cidade> GetById(int Id){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            return await Connection.QueryFirstOrDefaultAsync<Cidade>(
                @$"SELECT * FROM {TBL_CIDADE.NAME} 
                    WHERE {TBL_CIDADE.NR_ID} = {Id}");
        }

        public async Task<IEnumerable<Cidade>> ListAll(int? Id_estado){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            if(Id_estado != null
            && Id_estado > 0)
                return await Connection.QueryAsync<Cidade>(
                    @$"SELECT * FROM {TBL_CIDADE.NAME} 
                        WHERE {TBL_CIDADE.NR_ID_ESTADO} = {Id_estado}
                    ORDER BY {TBL_CIDADE.DS_NOME}");
            else
                return await Connection.QueryAsync<Cidade>(
                    @$"SELECT * FROM {TBL_CIDADE.NAME}
                    ORDER BY {TBL_CIDADE.DS_NOME}");
        }

    }

}
