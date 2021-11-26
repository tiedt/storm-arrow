using Microsoft.Extensions.Configuration;
using SIMP.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SIMP.Constants;
using SIMP.Models;

namespace SIMP.Services.Oracle {

    public class PontuacaoMusicaRepositoryOracle : TableBaseRepositoryOracle, IPontuacaoMusicaRepository{

        public PontuacaoMusicaRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> Insert(PontuacaoMusica model) {
            string Sql = $@"INSERT INTO {TBL_PONTUACAO_MUSICA.NAME}
                                    ({TBL_PONTUACAO_MUSICA.ID},
                                     {TBL_PONTUACAO_MUSICA.IDPERFIL},
                                     {TBL_PONTUACAO_MUSICA.ESTILO},
                                     {TBL_PONTUACAO_MUSICA.MUSICA},
                                     {TBL_PONTUACAO_MUSICA.PONTUACAO})
                              VALUES (:{TBL_PONTUACAO_MUSICA.ID},
                                      :{TBL_PONTUACAO_MUSICA.IDPERFIL},
                                      :{TBL_PONTUACAO_MUSICA.ESTILO},
                                      :{TBL_PONTUACAO_MUSICA.MUSICA},
                                      :{TBL_PONTUACAO_MUSICA.PONTUACAO})";
            return await Connection.ExecuteAsync(Sql, new {Id = await GetNextValSequence(TBL_PONTUACAO_MUSICA.ID.SEQUENCE),
                                                           model.IdPerfil, model.Estilo, model.Musica, model.Pontuacao} ) > 0;
        }

        public async Task<bool> Update(PontuacaoMusica model) {
            string Sql = $@"UPDATE {TBL_PONTUACAO_MUSICA.NAME} SET {TBL_PONTUACAO_MUSICA.PONTUACAO} = :{TBL_PONTUACAO_MUSICA.PONTUACAO}
                              WHERE {TBL_PONTUACAO_MUSICA.IDPERFIL} = :{TBL_PONTUACAO_MUSICA.IDPERFIL}
                                AND {TBL_PONTUACAO_MUSICA.ESTILO} = :{TBL_PONTUACAO_MUSICA.ESTILO}
                                AND {TBL_PONTUACAO_MUSICA.MUSICA} = :{TBL_PONTUACAO_MUSICA.MUSICA}";
            return await Connection.ExecuteAsync(Sql, new {model.Pontuacao, model.IdPerfil, model.Estilo, model.Musica}) > 0;
        }
        
        public async Task<bool> Delete(int id) {
            string Sql = $@"DELETE FROM {TBL_PONTUACAO_MUSICA.NAME}
                               WHERE {TBL_PONTUACAO_MUSICA.ID} = :{TBL_PONTUACAO_MUSICA.ID}";
            return await Connection.ExecuteAsync(Sql, new {id}) > 0;
        }

        public async Task<IEnumerable<PontuacaoMusica>> ListAll() {
            string Sql = $@"SELECT * FROM {TBL_PONTUACAO_MUSICA.NAME}";
            return await Connection.QueryAsync<PontuacaoMusica>(Sql);
        }

        public async Task<IEnumerable<PontuacaoMusica>> ListAllByProfile(int idPerfil) {
            string Sql = $@"SELECT * FROM {TBL_PONTUACAO_MUSICA.NAME}
                               WHERE {TBL_PONTUACAO_MUSICA.IDPERFIL} = :{TBL_PONTUACAO_MUSICA.IDPERFIL}";
            return await Connection.QueryAsync<PontuacaoMusica>(Sql, new {idPerfil});
        }

    }
}
