using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class EstoqueRepositoryOracle : TableBaseRepositoryOracle, IEstoqueRepository{

        public EstoqueRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<IEnumerable<Estoque>> GetAll(){
            return await Connection.QueryAsync<Estoque>(
                $@"SELECT * FROM {TBL_ESTOQUE.NAME}");
        }

        public async Task<IEnumerable<Estoque>> GetAllByProduto(int IdProduto){
            return await Connection.QueryAsync<Estoque>(
                $@"SELECT * FROM {TBL_ESTOQUE.NAME}
                        WHERE {TBL_ESTOQUE.ID_PRODUTO} = {IdProduto}");
        }

        public async Task<bool> Insert(Estoque model){
            string Sql = $@"INSERT INTO {TBL_ESTOQUE.NAME}
                            ({TBL_ESTOQUE.ID},
                             {TBL_ESTOQUE.ID_PRODUTO},
                             {TBL_ESTOQUE.SALDO})
                    VALUES ({await GetNextValSequence(TBL_ESTOQUE.ID.SEQUENCE)},
                            :{TBL_ESTOQUE.ID_PRODUTO},
                            :{TBL_ESTOQUE.SALDO})";
            return await Connection.ExecuteAsync(Sql, new { model.Id_produto, model.Saldo }) > 0;
        }

        public async Task<bool> Update(Estoque model){
            return await Connection.ExecuteAsync(
                $@"UPDATE {TBL_ESTOQUE.NAME} SET {TBL_ESTOQUE.SALDO} = {model.Saldo}
                        WHERE {TBL_ESTOQUE.ID_PRODUTO} = {model.Id_produto}") > 0;
        }
    }
}
