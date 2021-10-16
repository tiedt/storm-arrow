using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class ProdutoRepositoryOracle :  TableBaseRepositoryOracle, IProdutoRepository{

        public ProdutoRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> Delete(int Id){
            return await Connection.ExecuteAsync(
                $@"DELETE FROM {TBL_PRODUTO.NAME} 
                        WHERE {TBL_PRODUTO.ID} = {Id}") > 0;
        }

        public async Task<IEnumerable<Produto>> GetAll(){
            return await Connection.QueryAsync<Produto>(
                $@"SELECT * FROM {TBL_PRODUTO.NAME}");
        }

        public async Task<bool> Insert(Produto model){
            string Sql = $@"INSERT INTO {TBL_PRODUTO.NAME}
                                   ({TBL_PRODUTO.ID},
                                    {TBL_PRODUTO.DESCRICAO})
                            VALUES ({await GetNextValSequence(TBL_PRODUTO.ID.SEQUENCE)},
                                    :{TBL_PRODUTO.DESCRICAO})";
            return await Connection.ExecuteAsync(Sql, new { model.Descricao }) > 0;
        }

        public async Task<bool> Update(Produto model){
            return await Connection.ExecuteAsync(
                $@"UPDATE {TBL_PRODUTO.NAME} SET {TBL_PRODUTO.DESCRICAO} = {model.Descricao}
                        WHERE {TBL_PRODUTO.ID} = {model.Id}") > 0;
        }
    }
}
