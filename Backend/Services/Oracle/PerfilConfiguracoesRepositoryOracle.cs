using Microsoft.Extensions.Configuration;
using SIMP.Models;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SIMP.Constants;

namespace SIMP.Services.Oracle {
    
    public class PerfilConfiguracoesRepositoryOracle : TableBaseRepositoryOracle, IPerfilConfiguracoesRepository{

        public PerfilConfiguracoesRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> Insert(PerfilConfiguracoes model) {
            string Sql = $@"INSERT INTO {TBL_PERFIL_CONFIGURACOES.NAME}
                                             ({TBL_PERFIL_CONFIGURACOES.ID},
                                              {TBL_PERFIL_CONFIGURACOES.IDPERFIL},
                                              {TBL_PERFIL_CONFIGURACOES.CONFIG},
                                              {TBL_PERFIL_CONFIGURACOES.VALOR})
                               VALUES (:{TBL_PERFIL_CONFIGURACOES.ID},
                                       :{TBL_PERFIL_CONFIGURACOES.IDPERFIL},
                                       :{TBL_PERFIL_CONFIGURACOES.CONFIG},
                                       :{TBL_PERFIL_CONFIGURACOES.VALOR})";
            return await Connection.ExecuteAsync(Sql, new {Id = await this.GetNextValSequence(TBL_PERFIL_CONFIGURACOES.ID.SEQUENCE), 
                                                           model.IdPerfil,
                                                           model.Config,
                                                           model.Valor}) > 0;
        }
        
        public async Task<bool> Update(PerfilConfiguracoes model) {
            string Sql = $@"UPDATE {TBL_PERFIL_CONFIGURACOES.NAME} 
                               SET {TBL_PERFIL_CONFIGURACOES.VALOR} = :{TBL_PERFIL_CONFIGURACOES.VALOR}
                             WHERE {TBL_PERFIL_CONFIGURACOES.IDPERFIL} = :{TBL_PERFIL_CONFIGURACOES.IDPERFIL}
                               AND {TBL_PERFIL_CONFIGURACOES.CONFIG} = :{TBL_PERFIL_CONFIGURACOES.CONFIG}";
            return await Connection.ExecuteAsync(Sql, new { model.Valor, model.IdPerfil, model.Config }) > 0;
        }

        public async Task<bool> Delete(int IdPerfil) {
            string Sql = $@"DELETE FROM {TBL_PERFIL_CONFIGURACOES.NAME}
                               WHERE {TBL_PERFIL_CONFIGURACOES.IDPERFIL} = :{TBL_PERFIL_CONFIGURACOES.IDPERFIL}";
            return await Connection.ExecuteAsync(Sql, new { IdPerfil }) > 0;
        }

        public async Task<IEnumerable<PerfilConfiguracoes>> ListByPerfilConfig(int IdPerfil) {
            string Sql = $@"SELECT * FROM {TBL_PERFIL_CONFIGURACOES.NAME}
                              WHERE {TBL_PERFIL_CONFIGURACOES.IDPERFIL} = :{TBL_PERFIL_CONFIGURACOES.IDPERFIL}";
            return await Connection.QueryAsync<PerfilConfiguracoes>(Sql, new { IdPerfil });
        }
     
    }

}
