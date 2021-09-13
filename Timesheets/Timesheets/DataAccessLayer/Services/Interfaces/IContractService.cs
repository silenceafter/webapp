using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.Controllers.Responses;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Services.Interfaces
{
    public interface IContractService
    {
        ContractResponse RegisterContract(ContractModel contract);
        ContractResponse GetContract(int id, int contractid);
        List<ContractResponse> GetContractAll(int id);
        ContractModel GetModel(ContractResponse contractResponse);
    }
}
