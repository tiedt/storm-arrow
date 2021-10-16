using Dapper;
using Microsoft.Extensions.Configuration;
using SIMP.Constants;
using SIMP.Models;
using SIMP.Repositories;
using System.Data;
using System.Threading.Tasks;

namespace SIMP.Services.Oracle{

    public class LogAcessoRepositoryOracle : TableBaseRepositoryOracle, ILogAcessoRepository{

        public LogAcessoRepositoryOracle(IConfiguration configuration) : base(configuration) { }

        public async Task<bool> Insert(LogAcesso Model){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
            Model.Nr_id = await GetNextValSequence(TBL_LOG_ACESSO.NR_ID.SEQUENCE);
            return await Connection.ExecuteAsync(
                $@"INSERT INTO {TBL_LOG_ACESSO.NAME}
                           ({TBL_LOG_ACESSO.NR_ID},
                            {TBL_LOG_ACESSO.NR_ID_PROPOSTA},
                            {TBL_LOG_ACESSO.NR_ID_USUARIO})
                    VALUES ({Model.Nr_id},
                            {Model.Nr_id_proposta},
                            {Model.Nr_id_usuario})") > 0;
        }
    }
}
