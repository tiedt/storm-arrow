using Microsoft.Extensions.Configuration;
using SIMP.Models;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SIMP.Constants;

namespace SIMP.Services.Oracle {
    
    public class PerfilRepositoryOracle : TableBaseRepositoryOracle, IPerfilRepository {

        public PerfilRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> Insert(Perfil model) {
            string Sql = $@"INSERT INTO {TBL_PERFIL.NAME} 
                                       ({TBL_PERFIL.ID},
                                        {TBL_PERFIL.ENDERECO_MAC},
                                        {TBL_PERFIL.NOME},
                                        {TBL_PERFIL.PONTUACAO_TOTAL})
                               VALUES (:{TBL_PERFIL.ID},       
                                       '{model.Endereco_Mac}',
                                       '{model.Nome}',
                                       :{TBL_PERFIL.PONTUACAO_TOTAL})";
            return await Connection.ExecuteAsync(Sql, new {Id = await this.GetNextValSequence(TBL_PERFIL.ID.SEQUENCE), model.Pontuacao_Total}) > 0;
        }

        public async Task<bool> Update(Perfil model) {
            string Sql = $@"UPDATE {TBL_PERFIL.NAME} SET {TBL_PERFIL.PONTUACAO_TOTAL} = :{TBL_PERFIL.PONTUACAO_TOTAL}
                               WHERE {TBL_PERFIL.ID} = :{TBL_PERFIL.ID}";
            return await Connection.ExecuteAsync(Sql, new {model.Pontuacao_Total, model.Id}) > 0;
        }

        public async Task<bool> Delete(int id) {
            string Sql = $@"DELETE FROM {TBL_PERFIL.NAME} 
                                WHERE {TBL_PERFIL.ID} = :{TBL_PERFIL.ID}";
            return await Connection.ExecuteAsync(Sql, new {id}) > 0;
        }

        public async Task<IEnumerable<Perfil>> ListaAll() {
            string Sql = $@"SELECT * FROM {TBL_PERFIL.NAME}";
            return await Connection.QueryAsync<Perfil>(Sql);
        }

        public async Task<IEnumerable<Perfil>> ListAllMacAddress(string endereco_mac) {
            string Sql = $@"SELECT * FROM {TBL_PERFIL.NAME}
                               WHERE {TBL_PERFIL.ENDERECO_MAC} = :{TBL_PERFIL.ENDERECO_MAC}";
            return await Connection.QueryAsync<Perfil>(Sql, new {endereco_mac});
        }

        public async Task<Perfil> GetByMacAddrressName(string endereco_mac, string nome) {
            string Sql = $@"SELECT * FROM {TBL_PERFIL.NAME}
                              WHERE {TBL_PERFIL.ENDERECO_MAC} = :{TBL_PERFIL.ENDERECO_MAC}
                                AND {TBL_PERFIL.NOME} = :{TBL_PERFIL.NOME}";
            return await Connection.QueryFirstOrDefaultAsync<Perfil>(Sql, new {endereco_mac, nome});
        }

    }
}
