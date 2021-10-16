using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Simp.Models{
    
    public class DataConnection{

        private readonly IConfiguration configuration;
        protected IDbConnection Connection { get { return new OracleConnection(configuration.GetConnectionString("oracle")); } }
        public DataConnection(IConfiguration configuration){
            this.configuration = configuration;
        }

    }
}
