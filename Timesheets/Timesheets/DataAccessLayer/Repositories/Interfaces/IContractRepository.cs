using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Interfaces
{
    public interface IContractRepository : IRepository<ContractModel>
    {
        bool RegisterContract(ContractModel contract);
        ContractModel GetContract(int id, int contractid);
        List<ContractModel> GetContractAll(int id);
    }
}
