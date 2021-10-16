using System.Data;

namespace SIMP.Repositories{

    public interface ISimpConnection : IDbConnection{

        public bool TransactionActive();
        public void Commit(bool closeConnection = true);
        public void RollBack(bool closeConnection = true);
    }
}
