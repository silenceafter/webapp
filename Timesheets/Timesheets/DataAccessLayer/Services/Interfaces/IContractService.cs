using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Requests;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IContractService
    {
        ContractModel RegisterContract(ContractRequest contract);
        ContractModel GetContract(int id, int contractid);
        ContractModel GetContract(int contractid);
        List<ContractModel> GetContractAll(int id);
    }
}
