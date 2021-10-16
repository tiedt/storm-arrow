using SIMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMP.Repositories {

    public interface IEletivaRepository {

        public Task<int> Insert(Eletiva model);

        public Task<int> GetCountRecords();

    }
}
