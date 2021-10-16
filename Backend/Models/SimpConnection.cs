using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using SIMP.Repositories;
using System;
using System.Data;
using System.Data.Common;

namespace Simp.Models{

    public class SimpConnection : ISimpConnection{
        
        // Atenção
        // Não pode ser criado uma conexão com banco do tipo Singleton (instância única)
        // Pois esse objeto de conexão com o banco sempre é liberado da memória em algum momento aleatório durante a execução (Garbage Collector talvez)
        // Gerando nullPointerException em todo o projeto.
        // Ou seja, para cada solicitação ao banco, é estanciado um novo objeto em memória.
        // Com isso eliminou-se os nullPointerException
        protected readonly IDbConnection Connection;
        private IDbTransaction Transaction;

        public SimpConnection(IConfiguration configuration){
            Connection = new OracleConnection(configuration.GetConnectionString("oracle"));
            Transaction = null;
        }

        public string ConnectionString { get => Connection.ConnectionString; set => throw new NotImplementedException(); }

        public int ConnectionTimeout => Connection.ConnectionTimeout;

        public string Database => Connection.Database;

        public ConnectionState State => Connection.State;

        public IDbTransaction BeginTransaction(IsolationLevel il){
            if(TransactionActive())
                throw new Exception("Transação já iniciada.");
            Transaction = Connection.BeginTransaction(il);
            return Transaction;
        }

        public void ChangeDatabase(string databaseName) => throw new NotImplementedException();

        public void Close() => Connection.Close();

        public void Commit(bool closeConnection = true){
            if(!TransactionActive())
                throw new Exception("Transação não iniciada.");
            Transaction.Commit();
            Transaction.Dispose();
            if(closeConnection) Connection.Close();
            Transaction = null;
        }

        public IDbCommand CreateCommand(){
            var cmd = Connection.CreateCommand();
            cmd.Transaction = Transaction;
            return cmd;
        }

        public void Dispose() => Connection.Dispose();

        public void Open(){
            if(Connection.State != ConnectionState.Open)
                Connection.Open();
        }

        public void RollBack(bool closeConnection = true){
            if(!TransactionActive())
                throw new Exception("Transação não iniciada.");
            Transaction.Rollback();
            Transaction.Dispose();
            if(closeConnection) Close();
            Transaction = null;
        }
        public bool TransactionActive() => Transaction != null;

        IDbTransaction IDbConnection.BeginTransaction(){
            if(TransactionActive())
                throw new Exception("Transação já iniciada.");
            Transaction = Connection.BeginTransaction();
            return Transaction;
        }
    }
}
