using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheets.DataAccessLayer.Interfaces;
using Timesheets.DataAccessLayer.Models;

namespace Timesheets.DataAccessLayer.Interfaces
{
    public interface IContractRepository
    {
        int RegisterContract(ContractDto contract);
        ContractDto GetContract(int id, int contractid);
        ContractDto GetContract(int contractid);
        List<ContractDto> GetContractAll(int id);
    }
}